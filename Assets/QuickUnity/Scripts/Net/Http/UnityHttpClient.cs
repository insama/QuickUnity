/*
 *	The MIT License (MIT)
 *
 *	Copyright (c) 2017 Jerry Lee
 *
 *	Permission is hereby granted, free of charge, to any person obtaining a copy
 *	of this software and associated documentation files (the "Software"), to deal
 *	in the Software without restriction, including without limitation the rights
 *	to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *	copies of the Software, and to permit persons to whom the Software is
 *	furnished to do so, subject to the following conditions:
 *
 *	The above copyright notice and this permission notice shall be included in all
 *	copies or substantial portions of the Software.
 *
 *	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *	IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *	FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *	AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *	LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *	OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 *	SOFTWARE.
 */

using CSharpExtensions;
using CSharpExtensions.Net.Http;
using QuickUnity.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_5_4_OR_NEWER

using UnityEngine.Networking;

#endif

namespace QuickUnity.Net.Http
{
    public class ErrorReceivedEvent : UnityEvent<UnityHttpClient, HttpErrorReceivedEventArgs> { }

    public class DownloadInProgressEvent : UnityEvent<UnityHttpClient, DownloadInProgressEventArgs> { }

    public class DownloadCompletedEvent : UnityEvent<UnityHttpClient, DownloadCompletedEventArgs> { }

    public class ExceptionCaughtEvent : UnityEvent<UnityHttpClient, ExceptionCaughtEventArgs> { }

    public class UnityHttpClient : IDisposable
    {
        private static readonly Dictionary<Type, Type> responseTypeMaps = new Dictionary<Type, Type>()
        {
            { typeof(DownloadHandlerBuffer), typeof(UnityHttpResponse) },
            { typeof(DownloadHandlerScript), typeof(UnityHttpResponse) },
            { typeof(DownloadHandlerAssetBundle), typeof(UnityHttpResponseAssetBundle) },
            { typeof(DownloadHandlerAudioClip), typeof(UnityHttpResponseAudioClip) },
            { typeof(DownloadHandlerTexture), typeof(UnityHttpResponseTexture) }
        };

        #region Event Members

        public ErrorReceivedEvent ErrorReceived;

        public DownloadInProgressEvent DownloadInProgress;

        public DownloadCompletedEvent DownloadCompleted;

        public ExceptionCaughtEvent ExceptionCaught;

        #endregion Event Members

        private bool disposed = false;

        private UnityWebRequest unityWebRequest;

        private UnityHttpRequest request;

        private IUnityHttpResponder responder;

        #region Static Methods

        public static UnityHttpClient Get(string url, IUnityHttpResponder responder = null)
        {
            UnityHttpRequest req = new UnityHttpRequest(url);
            UnityHttpClient client = new UnityHttpClient(responder);
            client.SendRequest(req);
            return client;
        }

        public static UnityHttpClient GetAssetBundle(string url, uint crc, IUnityHttpResponder responder = null)
        {
            UnityHttpRequest req = new UnityHttpRequest(url, crc);
            UnityHttpClient client = new UnityHttpClient(responder);
            client.SendRequest(req);
            return client;
        }

        public static UnityHttpClient GetAssetBundle(string url, uint version, uint crc, IUnityHttpResponder responder = null)
        {
            UnityHttpRequest req = new UnityHttpRequest(url, version, crc);
            UnityHttpClient client = new UnityHttpClient(responder);
            client.SendRequest(req);
            return client;
        }

        public static UnityHttpClient GetAssetBundle(string url, Hash128 hash, uint crc, IUnityHttpResponder responder = null)
        {
            UnityHttpRequest req = new UnityHttpRequest(url, hash, crc);
            UnityHttpClient client = new UnityHttpClient(responder);
            client.SendRequest(req);
            return client;
        }

        public static UnityHttpClient GetAudioClip(string url, AudioType audioType, IUnityHttpResponder responder = null)
        {
            UnityHttpRequest req = new UnityHttpRequest(url, audioType);
            UnityHttpClient client = new UnityHttpClient(responder);
            client.SendRequest(req);
            return client;
        }

        public static UnityHttpClient GetTexture(string url, bool readable, IUnityHttpResponder responder = null)
        {
            UnityHttpRequest req = new UnityHttpRequest(url, readable);
            UnityHttpClient client = new UnityHttpClient(responder);
            client.SendRequest(req);
            return client;
        }

        #endregion Static Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityHttpClient"/> class.
        /// </summary>
        public UnityHttpClient()
        {
            ErrorReceived = new ErrorReceivedEvent();
            DownloadInProgress = new DownloadInProgressEvent();
            DownloadCompleted = new DownloadCompletedEvent();
            ExceptionCaught = new ExceptionCaughtEvent();

            unityWebRequest = new UnityWebRequest();
        }

        private UnityHttpClient(IUnityHttpResponder responder)
            : this()
        {
            this.responder = responder;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="UnityHttpClient"/> class.
        /// </summary>
        ~UnityHttpClient()
        {
            Dispose(false);
        }

        #region Public Methods

        public void SendRequest(UnityHttpRequest request)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("unityWebRequest");
            }

            this.request = request;
            SetRequestHeaders();
            unityWebRequest.url = request.RequestUriText;
            unityWebRequest.method = request.MethodText;
            unityWebRequest.downloadHandler = request.DownloadHandler;
            Observable.FromCoroutine(SendRequest).Subscribe();
        }

        public void Abort()
        {
            if (disposed)
            {
                throw new ObjectDisposedException("unityWebRequest");
            }

            unityWebRequest.Abort();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion Public Methods

        #region Protected Methods

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                unityWebRequest.Dispose();

                ErrorReceived.RemoveAllListeners();
                ErrorReceived = null;

                DownloadInProgress.RemoveAllListeners();
                DownloadInProgress = null;

                DownloadCompleted.RemoveAllListeners();
                DownloadCompleted = null;

                ExceptionCaught.RemoveAllListeners();
                ExceptionCaught = null;
            }

            disposed = true;
        }

        #endregion Protected Methods

        #region Private Methods

        private IEnumerator SendRequest()
        {
            yield return unityWebRequest.Send();

            if (unityWebRequest.isError)
            {
                string errorMessage = unityWebRequest.error;
                DispatchErrorReceivedEvent(errorMessage);
                OnError(errorMessage);
            }
            else
            {
                if (!unityWebRequest.isDone)
                {
                    // In progress
                    long totalLength = (long)unityWebRequest.downloadedBytes * (long)unityWebRequest.downloadProgress;
                    DispatchDownloadInProgressEvent((long)unityWebRequest.downloadedBytes, totalLength);
                }
                else
                {
                    UnityHttpResponse response = CreateHttpResponse();
                    DispatchDownloadCompletedEvent(response);
                    OnResult(response);
                }
            }
        }

        private void SetRequestHeaders()
        {
            if (request != null)
            {
                for (int i = 0, length = request.Headers.Count; i < length; i++)
                {
                    string key = request.Headers.GetKey(i);
                    string value = request.Headers.Get(i);
                    unityWebRequest.SetRequestHeader(key, value);
                }
            }
        }

        private UnityHttpResponse CreateHttpResponse()
        {
            Type downloadHandlerType = unityWebRequest.downloadHandler.GetType();
            Type responseType = responseTypeMaps[downloadHandlerType];
            return UnityReflectionUtil.CreateClassInstance<UnityHttpResponse>(responseType.FullName, new object[1] { unityWebRequest });
        }

        private void OnResult(UnityHttpResponse response)
        {
            if (responder != null)
            {
                responder.OnResult(response);
            }
        }

        private void OnError(string errorMessage)
        {
            if (responder != null)
            {
                responder.OnError(errorMessage);
            }
        }

        private void DispatchErrorReceivedEvent(string errorMessage)
        {
            if (ErrorReceived != null)
            {
                ErrorReceived.Invoke(this, new HttpErrorReceivedEventArgs(errorMessage));
            }
        }

        private void DispatchDownloadInProgressEvent(long bytesRead, long totalLength)
        {
            if (DownloadInProgress != null)
            {
                DownloadInProgress.Invoke(this, new DownloadInProgressEventArgs(bytesRead, totalLength));
            }
        }

        private void DispatchDownloadCompletedEvent(UnityHttpResponse response)
        {
            if (DownloadCompleted != null)
            {
                DownloadCompleted.Invoke(this, new DownloadCompletedEventArgs(response));
            }
        }

        private void DispatchExceptionCaughtEvent(Exception exception)
        {
            if (ExceptionCaught != null)
            {
                ExceptionCaught.Invoke(this, new ExceptionCaughtEventArgs(exception));
            }
        }

        #endregion Private Methods
    }
}
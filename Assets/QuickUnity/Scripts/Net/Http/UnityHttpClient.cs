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
using System.Reflection;
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

        private Action<UnityHttpClient, UnityHttpResponse> resultCallback;

        private Action<UnityHttpClient, string> errorCallback;

        #region Static Methods

        public static UnityHttpClient Get(string url, Action<UnityHttpClient, UnityHttpResponse> resultCallback = null, Action<UnityHttpClient, string> errorCallback = null)
        {
            UnityHttpRequest req = new UnityHttpRequest(url);
            UnityHttpClient client = new UnityHttpClient(resultCallback, errorCallback);
            client.SendRequest(req);
            return client;
        }

        public static UnityHttpClient GetAssetBundle(string url, uint crc, Action<UnityHttpClient, UnityHttpResponse> resultCallback = null, Action<UnityHttpClient, string> errorCallback = null)
        {
            UnityHttpRequest req = new UnityHttpRequest(url, crc);
            UnityHttpClient client = new UnityHttpClient(resultCallback, errorCallback);
            client.SendRequest(req);
            return client;
        }

        public static UnityHttpClient GetAssetBundle(string url, uint version, uint crc, Action<UnityHttpClient, UnityHttpResponse> resultCallback = null, Action<UnityHttpClient, string> errorCallback = null)
        {
            UnityHttpRequest req = new UnityHttpRequest(url, version, crc);
            UnityHttpClient client = new UnityHttpClient(resultCallback, errorCallback);
            client.SendRequest(req);
            return client;
        }

        public static UnityHttpClient GetAssetBundle(string url, Hash128 hash, uint crc, Action<UnityHttpClient, UnityHttpResponse> resultCallback = null, Action<UnityHttpClient, string> errorCallback = null)
        {
            UnityHttpRequest req = new UnityHttpRequest(url, hash, crc);
            UnityHttpClient client = new UnityHttpClient(resultCallback, errorCallback);
            client.SendRequest(req);
            return client;
        }

        public static UnityHttpClient GetAudioClip(string url, AudioType audioType, Action<UnityHttpClient, UnityHttpResponse> resultCallback = null, Action<UnityHttpClient, string> errorCallback = null)
        {
            UnityHttpRequest req = new UnityHttpRequest(url, audioType);
            UnityHttpClient client = new UnityHttpClient(resultCallback, errorCallback);
            client.SendRequest(req);
            return client;
        }

        public static UnityHttpClient GetTexture(string url, bool readable = false, Action<UnityHttpClient, UnityHttpResponse> resultCallback = null, Action<UnityHttpClient, string> errorCallback = null)
        {
            UnityHttpRequest req = new UnityHttpRequest(url, readable);
            UnityHttpClient client = new UnityHttpClient(resultCallback, errorCallback);
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
            AutoDispose = true;
        }

        private UnityHttpClient(Action<UnityHttpClient, UnityHttpResponse> resultCallback, Action<UnityHttpClient, string> errorCallback)
            : this()
        {
            this.resultCallback = resultCallback;
            this.errorCallback = errorCallback;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="UnityHttpClient"/> class.
        /// </summary>
        ~UnityHttpClient()
        {
            Dispose(false);
        }

        #region Properties

        public UnityHttpRequest Request
        {
            get { return request; }
        }

        public bool AutoDispose
        {
            get;
            set;
        }

        #endregion Properties

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

            if (request.CanDownloadData)
            {
                unityWebRequest.downloadHandler = request.DownloadHandler;
            }
            else
            {
                unityWebRequest.downloadHandler = null;
            }

            Observable.FromCoroutine(SendRequest).Subscribe();
        }

        public void Abort()
        {
            if (unityWebRequest != null)
            {
                unityWebRequest.Abort();
            }
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
                unityWebRequest = null;

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
            if (DownloadInProgress.GetPersistentEventCount() > 0)
            {
                // Need to show progress of response.
                yield return new WaitForEndOfFrame();
                AsyncOperation op = unityWebRequest.Send();

                int displayProgress = 0;
                int toProgress = 0;

                while (!op.isDone)
                {
                    toProgress = (int)op.progress * 100;

                    while (displayProgress < toProgress)
                    {
                        ++displayProgress;
                        long totalLength = (long)unityWebRequest.downloadedBytes * (long)unityWebRequest.downloadProgress;
                        DispatchDownloadInProgressEvent((long)unityWebRequest.downloadedBytes, totalLength, (float)displayProgress / 100);
                        yield return new WaitForEndOfFrame();
                    }
                }
            }
            else
            {
                yield return new WaitForEndOfFrame();
                yield return unityWebRequest.Send();
            }

            // Handle response.
            if (unityWebRequest.isError)
            {
                string errorMessage = unityWebRequest.error;
                DispatchErrorReceivedEvent(errorMessage);
                OnError(errorMessage);
            }
            else
            {
                UnityHttpResponse response = CreateHttpResponse();
                DispatchDownloadCompletedEvent(response);
                OnResult(response);
            }

            if (AutoDispose)
            {
                Dispose();
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
            return (UnityHttpResponse)UnityReflectionUtil.CreateInstance(responseType.FullName, BindingFlags.Instance | BindingFlags.NonPublic,
                new object[] { unityWebRequest, request.StateObject });
        }

        private void OnResult(UnityHttpResponse response)
        {
            if (resultCallback != null)
            {
                resultCallback.Invoke(this, response);
            }
        }

        private void OnError(string errorMessage)
        {
            if (errorCallback != null)
            {
                errorCallback.Invoke(this, errorMessage);
            }
        }

        private void DispatchErrorReceivedEvent(string errorMessage)
        {
            if (ErrorReceived != null)
            {
                ErrorReceived.Invoke(this, new HttpErrorReceivedEventArgs(errorMessage));
            }
        }

        private void DispatchDownloadInProgressEvent(long bytesRead, long totalLength, float progress = 0)
        {
            if (DownloadInProgress != null)
            {
                DownloadInProgress.Invoke(this, new DownloadInProgressEventArgs(bytesRead, totalLength, progress));
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
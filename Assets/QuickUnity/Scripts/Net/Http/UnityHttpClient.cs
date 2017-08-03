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
using System;
using System.Collections;
using UniRx;
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
        #region Event Members

        public ErrorReceivedEvent ErrorReceived;

        public DownloadInProgressEvent DownloadInProgress;

        public DownloadCompletedEvent DownloadCompleted;

        public ExceptionCaughtEvent ExceptionCaught;

        #endregion Event Members

        private bool disposed = false;

        private UnityWebRequest unityWebRequest;

        private UnityHttpRequest request;

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
            unityWebRequest.url = request.RequestUriText;
            unityWebRequest.method = request.MethodText;
            unityWebRequest.downloadHandler = request.GetDownloadHandler();
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
                DispatchErrorReceivedEvent(unityWebRequest.error);
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
                    UnityHttpResponse response = UnityHttpResponse.CreateResponse(request.DataType, unityWebRequest);
                    response.Data = unityWebRequest.downloadHandler.data;
                    DispatchDownloadCompletedEvent(response);
                }
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
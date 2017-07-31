/*
 *  The MIT License (MIT)
 *
 *  Copyright (c) 2017 Jerry Lee
 *
 *  Permission is hereby granted, free of charge, to any person obtaining a copy
 *  of this software and associated documentation files (the "Software"), to deal
 *  in the Software without restriction, including without limitation the rights
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *  copies of the Software, and to permit persons to whom the Software is
 *  furnished to do so, subject to the following conditions:
 *
 *  The above copyright notice and this permission notice shall be included in all
 *  copies or substantial portions of the Software.
 *
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 *  SOFTWARE.
 */

using CSharpExtensions.Net.Http;
using QuickUnity.Events;

namespace QuickUnity.Net.Http
{
    public partial class MonoHttpClient : ThreadEventDispatcher, IMonoHttpClient
    {
        private IMonoHttpClient httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="MonoHttpClient"/> class.
        /// </summary>
        public MonoHttpClient(bool useUnityHttpClient = true)
            : base()
        {
            if (useUnityHttpClient)
            {
                httpClient = new UnityHttpClient();
            }
            else
            {
                httpClient = new CSharpHttpClient();
            }

            httpClient.AddEventListener(HttpEvent.HttpStatusCodeReceived, OnHttpClientEventReceived);
            httpClient.AddEventListener(HttpEvent.HttpDownloadInProgress, OnHttpClientEventReceived);
            httpClient.AddEventListener(HttpEvent.HttpDownloadCompleted, OnHttpClientEventReceived);
            httpClient.AddEventListener(HttpEvent.HttpExceptionCaught, OnHttpClientEventReceived);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="MonoHttpClient"/> class.
        /// </summary>
        ~MonoHttpClient()
        {
            if (httpClient != null)
            {
                httpClient.RemoveEventListener(HttpEvent.HttpStatusCodeReceived, OnHttpClientEventReceived);
                httpClient.RemoveEventListener(HttpEvent.HttpDownloadInProgress, OnHttpClientEventReceived);
                httpClient.RemoveEventListener(HttpEvent.HttpDownloadCompleted, OnHttpClientEventReceived);
                httpClient.RemoveEventListener(HttpEvent.HttpExceptionCaught, OnHttpClientEventReceived);
            }
        }

        #region Public Methods

        /// <summary>
        /// Update is called every frame.
        /// </summary>
        public override void Update()
        {
            httpClient.Update();
            base.Update();
        }

        /// <summary>
        /// Sends the <see cref="HttpRequest"/>.
        /// </summary>
        /// <param name="request">The <see cref="HttpRequest"/> instance.</param>
        public void SendRequest(HttpRequest request)
        {
            httpClient.SendRequest(request);
        }

        /// <summary>
        /// Aborts the progress of HTTP request.
        /// </summary>
        public void Abort()
        {
            httpClient.Abort();
        }

        #endregion Public Methods

        #region Private Methods

        private void OnHttpClientEventReceived(Event e)
        {
            e.Context = this;
            DispatchEvent(e);
        }

        #endregion Private Methods
    }
}
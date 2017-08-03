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

using System;
using System.Net;
using CSharpExtensions.Net.Http;
using QuickUnity.Events;

namespace QuickUnity.Net.Http
{
    /// <summary>
    /// Provides a class for sending HTTP requests and receiving HTTP responses from a resource identified by a URI for Mono environment in Unity engine.
    /// </summary>
    /// <seealso cref="HttpClientBase"/>
    /// <seealso cref="IThreadEventDispatcher"/>
    public class MonoHttpClient : HttpClientBase, IThreadEventDispatcher
    {
        private IThreadEventDispatcher eventDispatcher;

        public MonoHttpClient()
            : base()
        {
            eventDispatcher = new ThreadEventDispatcher();
        }

        #region Public Methods

        #region IThreadEventDispatcher Interface

        public void Update()
        {
            eventDispatcher.Update();
        }

        public void AddEventListener(string eventType, Action<Event> listener)
        {
            eventDispatcher.AddEventListener(eventType, listener);
        }

        public void DispatchEvent(Event eventObject)
        {
            eventDispatcher.DispatchEvent(eventObject);
        }

        public bool HasEventListener(string eventType, Action<Event> listener)
        {
            return eventDispatcher.HasEventListener(eventType, listener);
        }

        public void RemoveEventListener(string eventType, Action<Event> listener)
        {
            eventDispatcher.RemoveEventListener(eventType, listener);
        }

        #endregion IThreadEventDispatcher Interface

        #endregion Public Methods

        #region Protected Methods

        protected override void DispatchDownloadInProgressEvent(long loaded, long totalLength)
        {
            DispatchEvent(new MonoHttpEvent(MonoHttpEvent.DownloadInProgress, this, loaded, totalLength));
        }

        protected override void DispatchDownloadCompletedEvent(HttpResponse response)
        {
            DispatchEvent(new MonoHttpEvent(MonoHttpEvent.DownloadCompleted, this, response));
        }

        protected override void DispatchResponseDataReceivedEvent(byte[] bytes)
        {
            // Prevents raising the event HttpResponseDataReceived.
        }

        protected override void DispatchExceptionCaughtEvent(Exception ex)
        {
            DispatchEvent(new MonoHttpEvent(MonoHttpEvent.ExceptionCaught, this, ex));
        }

        #endregion Protected Methods
    }
}
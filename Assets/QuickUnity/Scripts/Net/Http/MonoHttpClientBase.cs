using CSharpExtensions.Net.Http;
using QuickUnity.Events;
using System;
using System.Net;

namespace QuickUnity.Net.Http
{
    public partial class MonoHttpClient : ThreadEventDispatcher, IMonoHttpClient
    {
        private abstract class MonoHttpClientBase : HttpClientBase, IMonoHttpClient
        {
            private IThreadEventDispatcher eventDispatcher;

            protected MonoHttpClientBase()
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

            protected override void DispatchHttpStatusCodeReceivedEvent(HttpStatusCode code)
            {
                DispatchEvent(new HttpEvent(HttpEvent.HttpStatusCodeReceived, code));
            }

            protected override void DispatchHttpDownloadInProgressEvent(long loaded, long totalLength)
            {
                DispatchEvent(new HttpEvent(HttpEvent.HttpDownloadInProgress, loaded, totalLength));
            }

            protected override void DispatchHttpDownloadCompletedEvent(HttpResponse response)
            {
                DispatchEvent(new HttpEvent(HttpEvent.HttpDownloadCompleted, response));
            }

            protected override void DispatchHttpResponseDataReceivedEvent(byte[] bytes)
            {
                throw new NotImplementedException();
            }

            protected override void DispatchHttpExceptionCaughtEvent(Exception ex)
            {
                DispatchEvent(new HttpEvent(HttpEvent.HttpExceptionCaught, ex));
            }

            #endregion Protected Methods
        }
    }
}
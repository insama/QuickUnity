using QuickUnity.Threading;
using RestSharp;
using System;
using System.Collections.Generic;

namespace QuickUnity.Net.Http
{
    /// <summary>
    /// Client to translate RestRequests into Http requests and process response result for Mono in
    /// Unity Engine.
    /// </summary>
    /// <seealso cref="RestClient"/>
    /// <seealso cref="ISyncObject"/>
    public class MonoRestClient : RestClient, IMonoRestClient, ISynchronizedObject
    {
        #region Fields

        private List<MonoRestRequestAsyncContext> contexts;

        private bool disposed = false;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MonoRestClient"/> class.
        /// </summary>
        public MonoRestClient()
            : base()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MonoRestClient"/> class with <see
        /// cref="Uri"/> represent for the base URL to request.
        /// </summary>
        /// <param name="baseUrl">The <see cref="Uri"/> represent for the base URL to request.</param>
        public MonoRestClient(Uri baseUrl)
            : base(baseUrl)
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MonoRestClient"/> class with <see
        /// cref="string"/> represent for the base URL string to request.
        /// </summary>
        /// <param name="baseUrl">
        /// The <see cref="string"/> represent for the base URL string to request.
        /// </param>
        public MonoRestClient(string baseUrl)
            : base(baseUrl)
        {
            Initialize();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting
        /// unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Sends the specified request and downloads the response data.
        /// </summary>
        /// <param name="request">The <see cref="IRestRequest"/> to send.</param>
        /// <param name="callback">
        /// Callback function to be executed upon completion providing access to the asynchronous handle.
        /// </param>
        /// <returns>The <see cref="RestRequestAsyncHandle"/> to handle asynchronous request.</returns>
        /// <exception cref="ObjectDisposedException">
        /// <c>UniSharper.Net.Http.MonoRestClient</c> is disposed.
        /// </exception>
        public RestRequestAsyncHandle DownloadDataAsync(IRestRequest request, Action<byte[], RestRequestAsyncHandle> callback)
        {
            if (disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }

            return ExecuteAsync(request, CreateContext(request, callback).GetResponse);
        }

        /// <summary>
        /// Sends the request and callback asynchronously, authenticating if needed.
        /// </summary>
        /// <param name="request"><see cref="IRestRequest"/> to be sent.</param>
        /// <param name="callback">
        /// Callback function to be executed upon completion providing access to the asynchronous handle.
        /// </param>
        /// <returns>The <see cref="RestRequestAsyncHandle"/> to handle asynchronous request.</returns>
        /// <exception cref="ObjectDisposedException">
        /// <c>UniSharper.Net.Http.MonoRestClient</c> is disposed.
        /// </exception>
        public RestRequestAsyncHandle SendRequestAsync(IRestRequest request, Action<IRestResponse, RestRequestAsyncHandle> callback)
        {
            if (disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }

            return ExecuteAsync(request, CreateContext(request, callback).GetResponse);
        }

        /// <summary>
        /// Sends a GET-style request and callback asynchronously, authenticating if needed.
        /// </summary>
        /// <param name="request"><see cref="IRestRequest"/> to be sent.</param>
        /// <param name="callback">
        /// Callback function to be executed upon completion providing access to the asynchronous handle.
        /// </param>
        /// <param name="method">The <see cref="Method"/> represents for HTTP method to execute.</param>
        /// <returns>The <see cref="RestRequestAsyncHandle"/> to handle asynchronous request.</returns>
        /// <exception cref="ObjectDisposedException">
        /// <c>UniSharper.Net.Http.MonoRestClient</c> is disposed.
        /// </exception>
        public RestRequestAsyncHandle SendRequestAsyncGet(IRestRequest request, Action<IRestResponse, RestRequestAsyncHandle> callback, Method method)
        {
            if (disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }

            string methodString = Enum.GetName(typeof(Method), method);
            return ExecuteAsyncGet(request, CreateContext(request, callback).GetResponse, methodString);
        }

        /// <summary>
        /// Sends a POST-style request and callback asynchronously, authenticating if needed.
        /// </summary>
        /// <param name="request"><see cref="IRestRequest"/> to be sent.</param>
        /// <param name="callback">
        /// Callback function to be executed upon completion providing access to the asynchronous handle.
        /// </param>
        /// <param name="method">The <see cref="Method"/> represents for HTTP method to execute.</param>
        /// <returns>RestRequestAsyncHandle.</returns>
        /// <exception cref="ObjectDisposedException">
        /// <c>UniSharper.Net.Http.MonoRestClient</c> is disposed.
        /// </exception>
        public RestRequestAsyncHandle SendRequestAsyncPost(IRestRequest request, Action<IRestResponse, RestRequestAsyncHandle> callback, Method method)
        {
            if (disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }

            string methodString = Enum.GetName(typeof(Method), method);
            return ExecuteAsyncPost(request, CreateContext(request, callback).GetResponse, methodString);
        }

        /// <summary>
        /// Synchronizes data between threads.
        /// </summary>
        public void Synchronize()
        {
            if (contexts == null)
            {
                return;
            }

            IList<MonoRestRequestAsyncContext> removedContexts = new List<MonoRestRequestAsyncContext>();

            for (int i = 0, length = contexts.Count; i < length; i++)
            {
                MonoRestRequestAsyncContext context = contexts[i];

                if (context.IsRequestCompleted)
                {
                    if (context.Callback != null)
                    {
                        context.Callback.Invoke(context.Response, context.AsyncHandle);
                    }
                    else if (context.DownloadDataCallback != null)
                    {
                        context.DownloadDataCallback.Invoke(context.DownloadedData, context.AsyncHandle);
                    }

                    removedContexts.Add(context);
                }
            }

            RemoveCompletedContexts(removedContexts);
            removedContexts = null;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only
        /// unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !disposed)
            {
                if (Synchronizer.Instance)
                {
                    Synchronizer.Instance.Remove(this);
                }

                contexts = null;
            }

            disposed = true;
        }

        /// <summary>
        /// Creates the <see cref="MonoRestRequestAsyncContext"/> for main thread of Unity to invoke callback.
        /// </summary>
        /// <param name="request">The <see cref="IRestRequest"/> to send.</param>
        /// <param name="callback">
        /// The Callback function to be executed upon completion providing access to the asynchronous handle.
        /// </param>
        /// <returns>
        /// The <see cref="MonoRestRequestAsyncContext"/> for main thread of Unity to invoke callback.
        /// </returns>
        private MonoRestRequestAsyncContext CreateContext(IRestRequest request, Action<byte[], RestRequestAsyncHandle> callback)
        {
            MonoRestRequestAsyncContext context = new MonoRestRequestAsyncContext(request, callback);
            contexts.Add(context);
            return context;
        }

        /// <summary>
        /// Creates the <see cref="MonoRestRequestAsyncContext"/> for main thread of Unity to invoke callback.
        /// </summary>
        /// <param name="request">The <see cref="IRestRequest"/> to send.</param>
        /// <param name="callback">
        /// The Callback function to be executed upon completion providing access to the asynchronous handle.
        /// </param>
        /// <returns>
        /// The <see cref="MonoRestRequestAsyncContext"/> for main thread of Unity to invoke callback.
        /// </returns>
        private MonoRestRequestAsyncContext CreateContext(IRestRequest request, Action<IRestResponse, RestRequestAsyncHandle> callback)
        {
            MonoRestRequestAsyncContext context = new MonoRestRequestAsyncContext(request, callback);
            contexts.Add(context);
            return context;
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private void Initialize()
        {
            contexts = new List<MonoRestRequestAsyncContext>();

            if (Synchronizer.Instance)
            {
                Synchronizer.Instance.Add(this);
            }
        }

        /// <summary>
        /// Removes contexts whose request is completed.
        /// </summary>
        /// <param name="list">The context list.</param>
        private void RemoveCompletedContexts(IList<MonoRestRequestAsyncContext> list)
        {
            if (contexts == null)
            {
                return;
            }

            foreach (MonoRestRequestAsyncContext item in list)
            {
                contexts.Remove(item);
            }
        }

        #endregion Methods

        #region Classes

        /// <summary>
        /// The <see cref="MonoRestRequestAsyncContext"/> represents for the context object for main
        /// thread of Unity to invoke callback.
        /// </summary>
        private class MonoRestRequestAsyncContext
        {
            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="MonoRestRequestAsyncContext"/> class.
            /// </summary>
            /// <param name="request">The <see cref="IRestRequest"/> to send.</param>
            /// <param name="callback">
            /// The Callback function to be executed upon completion providing access to the
            /// asynchronous handle.
            /// </param>
            public MonoRestRequestAsyncContext(IRestRequest request, Action<IRestResponse, RestRequestAsyncHandle> callback)
            {
                IsRequestCompleted = false;
                Request = request;
                Callback = callback;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="MonoRestRequestAsyncContext"/> class.
            /// </summary>
            /// <param name="request">The <see cref="IRestRequest"/> to send.</param>
            /// <param name="callback">
            /// The Callback function to be executed upon completion providing access to the
            /// asynchronous handle.
            /// </param>
            public MonoRestRequestAsyncContext(IRestRequest request, Action<byte[], RestRequestAsyncHandle> callback)
            {
                IsRequestCompleted = false;
                Request = request;
                DownloadDataCallback = callback;
            }

            #endregion Constructors

            #region Properties

            /// <summary>
            /// Gets or sets the asynchronous handle.
            /// </summary>
            /// <value>The asynchronous handle.</value>
            public RestRequestAsyncHandle AsyncHandle
            {
                get;
                private set;
            }

            /// <summary>
            /// Gets or sets the Callback function to be executed upon completion providing access to
            /// the asynchronous handle.
            /// </summary>
            /// <value>The callback function.</value>
            public Action<IRestResponse, RestRequestAsyncHandle> Callback
            {
                get;
                private set;
            }

            /// <summary>
            /// Gets or sets the data-download Callback function to be executed upon completion
            /// providing access to the asynchronous handle.
            /// </summary>
            /// <value>The data-download callback function.</value>
            public Action<byte[], RestRequestAsyncHandle> DownloadDataCallback
            {
                get;
                private set;
            }

            /// <summary>
            /// Gets the downloaded data.
            /// </summary>
            /// <value>The downloaded data.</value>
            public byte[] DownloadedData
            {
                get
                {
                    if (Response != null)
                    {
                        return Response.RawBytes;
                    }

                    return null;
                }
            }

            /// <summary>
            /// Gets or sets a value indicating whether the request is completed.
            /// </summary>
            /// <value><c>true</c> if the request is completed; otherwise, <c>false</c>.</value>
            public bool IsRequestCompleted
            {
                get;
                private set;
            }

            /// <summary>
            /// Gets or sets the <see cref="IRestRequest"/> to send.
            /// </summary>
            /// <value>The <see cref="IRestRequest"/> to send.</value>
            public IRestRequest Request
            {
                get;
                private set;
            }

            /// <summary>
            /// Gets or sets the <see cref="IRestResponse"/> received.
            /// </summary>
            /// <value>The <see cref="IRestResponse"/> received.</value>
            public IRestResponse Response
            {
                get;
                private set;
            }

            #endregion Properties

            #region Methods

            /// <summary>
            /// The callback of getting the <see cref="IRestResponse"/>.
            /// </summary>
            /// <param name="response">The <see cref="IRestResponse"/> received.</param>
            /// <param name="handle">The asynchronous handle.</param>
            public void GetResponse(IRestResponse response, RestRequestAsyncHandle handle)
            {
                Response = response;
                AsyncHandle = handle;
                IsRequestCompleted = true;
            }

            #endregion Methods
        }

        #endregion Classes
    }
}
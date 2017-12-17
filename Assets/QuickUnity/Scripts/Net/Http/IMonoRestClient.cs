using RestSharp;
using System;

namespace QuickUnity.Net.Http
{
    /// <summary>
    /// The <see cref="IMonoRestClient"/> interface defines methods for handling asynchronous REST
    /// request in Unity Engine.
    /// </summary>
    /// <seealso cref="IRestClient"/>
    /// <seealso cref="IDisposable"/>
    public interface IMonoRestClient : IRestClient, IDisposable
    {
        #region Methods

        /// <summary>
        /// Sends the specified request and downloads the response data.
        /// </summary>
        /// <param name="request">The <see cref="IRestRequest"/> to send.</param>
        /// <param name="callback">
        /// Callback function to be executed upon completion providing access to the asynchronous handle.
        /// </param>
        /// <returns>The <see cref="RestRequestAsyncHandle"/> to handle asynchronous request.</returns>
        RestRequestAsyncHandle DownloadDataAsync(IRestRequest request, Action<byte[], RestRequestAsyncHandle> callback);

        /// <summary>
        /// Sends the request and callback asynchronously, authenticating if needed.
        /// </summary>
        /// <param name="request"><see cref="IRestRequest"/> to be sent.</param>
        /// <param name="callback">
        /// Callback function to be executed upon completion providing access to the asynchronous handle.
        /// </param>
        /// <returns>The <see cref="RestRequestAsyncHandle"/> to handle asynchronous request.</returns>
        RestRequestAsyncHandle SendRequestAsync(IRestRequest request, Action<IRestResponse, RestRequestAsyncHandle> callback);

        /// <summary>
        /// Sends a GET-style request and callback asynchronously, authenticating if needed.
        /// </summary>
        /// <param name="request"><see cref="IRestRequest"/> to be sent.</param>
        /// <param name="callback">
        /// Callback function to be executed upon completion providing access to the asynchronous handle.
        /// </param>
        /// <param name="method">The <see cref="Method"/> represents for HTTP method to execute.</param>
        /// <returns>The <see cref="RestRequestAsyncHandle"/> to handle asynchronous request.</returns>
        RestRequestAsyncHandle SendRequestAsyncGet(IRestRequest request, Action<IRestResponse, RestRequestAsyncHandle> callback, Method method);

        /// <summary>
        /// Sends a POST-style request and callback asynchronously, authenticating if needed.
        /// </summary>
        /// <param name="request"><see cref="IRestRequest"/> to be sent.</param>
        /// <param name="callback">
        /// Callback function to be executed upon completion providing access to the asynchronous handle.
        /// </param>
        /// <param name="method">The <see cref="Method"/> represents for HTTP method to execute.</param>
        RestRequestAsyncHandle SendRequestAsyncPost(IRestRequest request, Action<IRestResponse, RestRequestAsyncHandle> callback, Method method);

        #endregion Methods
    }
}
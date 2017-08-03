using CSharpExtensions.Net.Http;
using QuickUnity.Utils;
using System;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace QuickUnity.Net.Http
{
    public enum UnityHttpRequestDataType
    {
        Buffer,
        AssetBundle,
        AudioClip,
        Script,
        Texture
    }

    public class UnityHttpRequest : HttpRequestBase
    {
        private static readonly Dictionary<UnityHttpRequestDataType, Type> downloadHandlersMap = new Dictionary<UnityHttpRequestDataType, Type>()
        {
            { UnityHttpRequestDataType.Buffer, typeof(DownloadHandlerBuffer) },
            { UnityHttpRequestDataType.AssetBundle, typeof(DownloadHandlerAssetBundle) },
            { UnityHttpRequestDataType.AudioClip, typeof(DownloadHandlerAudioClip) },
            { UnityHttpRequestDataType.Script, typeof(DownloadHandlerScript) },
            { UnityHttpRequestDataType.Texture, typeof(DownloadHandlerTexture) }
        };

        private UnityHttpRequestDataType dataType;

        #region Constructors

        internal UnityHttpRequest(Uri requestUri, HttpRequestMethod method = HttpRequestMethod.Get, UnityHttpRequestDataType dataType = UnityHttpRequestDataType.Buffer)
            : base(requestUri, method)
        {
            this.dataType = dataType;
        }

        internal UnityHttpRequest(string requestUriString, HttpRequestMethod method = HttpRequestMethod.Get, UnityHttpRequestDataType dataType = UnityHttpRequestDataType.Buffer)
            : base(requestUriString, method)
        {
            this.dataType = dataType;
        }

        #endregion Constructors

        public UnityHttpRequestDataType DataType
        {
            get { return dataType; }
            set { dataType = value; }
        }

        #region Public Methods

        /// <summary>
        /// Gets the instance of <see cref="DownloadHandler"/>.
        /// </summary>
        /// <returns>The instance of <see cref="DownloadHandler"/></returns>
        public DownloadHandler GetDownloadHandler()
        {
            Type classType = downloadHandlersMap[dataType];
            return UnityReflectionUtil.CreateClassInstance<DownloadHandler>(classType.FullName);
        }

        #endregion Public Methods
    }
}
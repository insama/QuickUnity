using CSharpExtensions.Net.Http;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace QuickUnity.Net.Http
{
    public class UnityHttpResponse : HttpResponseBase
    {
        public static UnityHttpResponse CreateResponse(UnityHttpRequestDataType dataType, UnityWebRequest unityWebRequest)
        {
            if (unityWebRequest == null)
            {
                throw new ArgumentNullException("unityWebRequest");
            }

            if (unityWebRequest.downloadHandler == null)
            {
                throw new ArgumentNullException("unityWebRequest.downloadHandler");
            }

            UnityHttpResponse responseInstance = null;
            DownloadHandler downloadHandler = unityWebRequest.downloadHandler;

            switch (dataType)
            {
                case UnityHttpRequestDataType.Buffer:
                    responseInstance = new UnityHttpResponse(unityWebRequest, unityWebRequest.downloadHandler.data);
                    break;

                case UnityHttpRequestDataType.AssetBundle:
                    responseInstance = new UnityHttpResponse(unityWebRequest, ((DownloadHandlerAssetBundle)downloadHandler).assetBundle);
                    break;

                case UnityHttpRequestDataType.AudioClip:
                    responseInstance = new UnityHttpResponse(unityWebRequest, ((DownloadHandlerAudioClip)downloadHandler).audioClip);
                    break;

                case UnityHttpRequestDataType.Script:
                    responseInstance = new UnityHttpResponse(unityWebRequest, ((DownloadHandlerScript)downloadHandler).text);
                    break;

                case UnityHttpRequestDataType.Texture:
                    responseInstance = new UnityHttpResponse(unityWebRequest, ((DownloadHandlerTexture)downloadHandler).texture);
                    break;
            }

            return responseInstance;
        }

        private UnityWebRequest unityWebRequest;

        #region Constructors

        private UnityHttpResponse(UnityWebRequest unityWebRequest)
            : base()
        {
            this.unityWebRequest = unityWebRequest;
        }

        private UnityHttpResponse(UnityWebRequest unityWebRequest, byte[] data)
            : this(unityWebRequest)
        {
            Data = data;
        }

        private UnityHttpResponse(UnityWebRequest unityWebRequest, AssetBundle assetBundle)
            : this(unityWebRequest)
        {
            AssetBundle = assetBundle;
        }

        private UnityHttpResponse(UnityWebRequest unityWebRequest, AudioClip audioClip)
            : this(unityWebRequest)
        {
            AudioClip = audioClip;
        }

        private UnityHttpResponse(UnityWebRequest unityWebRequest, string scriptText)
            : this(unityWebRequest)
        {
            ScriptText = scriptText;
        }

        private UnityHttpResponse(UnityWebRequest unityWebRequest, Texture2D texture)
            : this(unityWebRequest)
        {
            Texture = texture;
        }

        #endregion Constructors

        #region Properties

        public override string Text
        {
            get
            {
                return DownloadHandlerBuffer.GetContent(unityWebRequest);
            }
        }

        public AssetBundle AssetBundle
        {
            get;
            private set;
        }

        public AudioClip AudioClip
        {
            get;
            private set;
        }

        public string ScriptText
        {
            get;
            private set;
        }

        public Texture2D Texture
        {
            get;
            private set;
        }

        #endregion Properties
    }
}
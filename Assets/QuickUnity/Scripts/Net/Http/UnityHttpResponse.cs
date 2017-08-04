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

using CSharpExtensions.Net.Http;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;

namespace QuickUnity.Net.Http
{
    public class UnityHttpResponse : HttpResponseBase
    {
        private UnityWebRequest unityWebRequest;

        #region Constructors

        internal UnityHttpResponse(UnityWebRequest unityWebRequest)
            : base()
        {
            this.unityWebRequest = unityWebRequest;
            SetResponseHeaders();
            StatusCode = (HttpStatusCode)unityWebRequest.responseCode;
            Data = unityWebRequest.downloadHandler.data;
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

        public string ScriptText
        {
            get;
            private set;
        }

        #endregion Properties

        #region Private Methods

        private void SetResponseHeaders()
        {
            Dictionary<string, string> headers = unityWebRequest.GetResponseHeaders();

            foreach (KeyValuePair<string, string> kvp in headers)
            {
                AddHeader(kvp.Key, kvp.Value);
            }
        }

        private void AddHeader(string name, string value)
        {
            if (Headers == null)
            {
                Headers = new WebHeaderCollection();
            }

            Headers.Add(name, value);
        }

        #endregion Private Methods
    }

    public class UnityHttpResponseAssetBundle : UnityHttpResponse
    {
        public UnityHttpResponseAssetBundle(UnityWebRequest unityWebRequest)
            : base(unityWebRequest)
        {
            DownloadHandlerAssetBundle downloadHandler = (DownloadHandlerAssetBundle)unityWebRequest.downloadHandler;

            if (downloadHandler != null)
            {
                AssetBundle = downloadHandler.assetBundle;
            }
        }

        public AssetBundle AssetBundle
        {
            get;
            private set;
        }
    }

    public class UnityHttpResponseAudioClip : UnityHttpResponse
    {
        public UnityHttpResponseAudioClip(UnityWebRequest unityWebRequest)
            : base(unityWebRequest)
        {
            DownloadHandlerAudioClip downloadHandler = (DownloadHandlerAudioClip)unityWebRequest.downloadHandler;

            if (downloadHandler != null)
            {
                AudioClip = downloadHandler.audioClip;
            }
        }

        public AudioClip AudioClip
        {
            get;
            private set;
        }
    }

    public class UnityHttpResponseTexture : UnityHttpResponse
    {
        public UnityHttpResponseTexture(UnityWebRequest unityWebRequest)
            : base(unityWebRequest)
        {
            DownloadHandlerTexture downloadHandler = (DownloadHandlerTexture)unityWebRequest.downloadHandler;

            if (downloadHandler != null)
            {
                Texture = downloadHandler.texture;
            }
        }

        public Texture2D Texture
        {
            get;
            private set;
        }
    }
}
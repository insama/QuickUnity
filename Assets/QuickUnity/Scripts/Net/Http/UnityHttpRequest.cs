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
using UnityEngine;
using UnityEngine.Networking;

namespace QuickUnity.Net.Http
{
    public class UnityHttpRequest : HttpRequestBase
    {
        #region Constructors

        public UnityHttpRequest(string requestUriString)
            : base(requestUriString)
        {
            DownloadHandler = new DownloadHandlerBuffer();
            CanDownloadData = true;
        }

        #region AssetBundle

        public UnityHttpRequest(string requestUriString, uint crc)
            : base(requestUriString)
        {
            DownloadHandler = new DownloadHandlerAssetBundle(requestUriString, crc);
            CanDownloadData = true;
        }

        public UnityHttpRequest(string requestUriString, uint version, uint crc)
            : base(requestUriString)
        {
            DownloadHandler = new DownloadHandlerAssetBundle(requestUriString, version, crc);
            CanDownloadData = true;
        }

        public UnityHttpRequest(string requestUriString, Hash128 hash, uint crc)
            : base(requestUriString)
        {
            DownloadHandler = new DownloadHandlerAssetBundle(requestUriString, hash, crc);
            CanDownloadData = true;
        }

        #endregion AssetBundle

        #region AudioClip

        public UnityHttpRequest(string requestUriString, AudioType audioType)
            : base(requestUriString)
        {
            DownloadHandler = new DownloadHandlerAudioClip(requestUriString, audioType);
            CanDownloadData = true;
        }

        #endregion AudioClip

        #region Texture

        public UnityHttpRequest(string requestUriString, bool readable)
            : base(requestUriString)
        {
            DownloadHandler = new DownloadHandlerTexture(readable);
            CanDownloadData = true;
        }

        #region Script

        public UnityHttpRequest(string requestUriString, byte[] preallocatedBuffer)
            : base(requestUriString)
        {
            if (preallocatedBuffer == null || preallocatedBuffer.Length == 0)
            {
                DownloadHandler = new DownloadHandlerScript();
            }
            else
            {
                DownloadHandler = new DownloadHandlerScript(preallocatedBuffer);
            }

            CanDownloadData = true;
        }

        #endregion Script

        #endregion Texture

        #endregion Constructors

        #region Properties

        public object Async​State
        {
            get;
            set;
        }

        public bool CanDownloadData
        {
            get;
            set;
        }

        public DownloadHandler DownloadHandler
        {
            get;
            private set;
        }

        #endregion Properties
    }
}
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

        internal UnityHttpRequest(string requestUriString)
            : base(requestUriString)
        {
            DownloadHandler = new DownloadHandlerBuffer();
        }

        #region AssetBundle

        internal UnityHttpRequest(string requestUriString, uint crc)
            : base(requestUriString)
        {
            DownloadHandler = new DownloadHandlerAssetBundle(requestUriString, crc);
        }

        internal UnityHttpRequest(string requestUriString, uint version, uint crc)
            : base(requestUriString)
        {
            DownloadHandler = new DownloadHandlerAssetBundle(requestUriString, version, crc);
        }

        internal UnityHttpRequest(string requestUriString, Hash128 hash, uint crc)
            : base(requestUriString)
        {
            DownloadHandler = new DownloadHandlerAssetBundle(requestUriString, hash, crc);
        }

        #endregion AssetBundle

        #region AudioClip

        internal UnityHttpRequest(string requestUriString, AudioType audioType)
            : base(requestUriString)
        {
            DownloadHandler = new DownloadHandlerAudioClip(requestUriString, audioType);
        }

        #endregion AudioClip

        #region Texture

        internal UnityHttpRequest(string requestUriString, bool readable)
            : base(requestUriString)
        {
            DownloadHandler = new DownloadHandlerTexture(readable);
        }

        #region Script

        internal UnityHttpRequest(string requestUriString, byte[] preallocatedBuffer)
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
        }

        #endregion Script

        #endregion Texture

        #endregion Constructors

        #region Properties

        public DownloadHandler DownloadHandler
        {
            get;
            private set;
        }

        #endregion Properties
    }
}
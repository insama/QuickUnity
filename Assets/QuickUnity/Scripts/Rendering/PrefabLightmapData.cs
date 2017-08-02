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
using UnityEngine;

namespace QuickUnity.Rendering
{
    /// <summary>
    /// The class <see cref="PrefabLightmapData"/> provides lightmap data of prefab storing and reverting.
    /// </summary>
    /// <seealso cref="MonoBehaviour"/>
    [DisallowMultipleComponent, ExecuteInEditMode]
    public class PrefabLightmapData : MonoBehaviour
    {
        [Serializable]
        public struct RendererInfo
        {
            [SerializeField]
            private Renderer renderer;

            public Renderer Renderer
            {
                get { return renderer; }
                set { renderer = value; }
            }

            [SerializeField]
            private int lightmapIndex;

            public int LightmapIndex
            {
                get { return lightmapIndex; }
                set { lightmapIndex = value; }
            }

            [SerializeField]
            private Vector4 lightmapOffsetScale;

            public Vector4 LightmapOffsetScale
            {
                get { return lightmapOffsetScale; }
                set { lightmapOffsetScale = value; }
            }
        }

        [SerializeField]
        private RendererInfo[] rendererInfos;

        [SerializeField]
        private Texture2D[] lightmaps;

        /// <summary>
        /// Sets the renderer informations.
        /// </summary>
        /// <value>The renderer informations.</value>
        public RendererInfo[] RendererInfos
        {
            set { rendererInfos = value; }
        }

        /// <summary>
        /// Sets the lightmaps.
        /// </summary>
        /// <value>The lightmaps.</value>
        public Texture2D[] Lightmaps
        {
            set { lightmaps = value; }
        }

        private static void ApplyStaticLightmap(RendererInfo[] infos, int lightmapOffsetIndex)
        {
            for (int i = 0, length = infos.Length; i < length; i++)
            {
                RendererInfo info = infos[i];
                info.Renderer.lightmapIndex = info.LightmapIndex + lightmapOffsetIndex;
                info.Renderer.lightmapScaleOffset = info.LightmapOffsetScale;
            }
        }

        #region Messages

        /// <summary>
        /// Called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            if (rendererInfos == null || rendererInfos.Length == 0)
            {
                return;
            }

            LightmapData[] lightmaps = LightmapSettings.lightmaps;
            LightmapData[] combinedLightmaps = new LightmapData[lightmaps.Length + this.lightmaps.Length];
            lightmaps.CopyTo(combinedLightmaps, 0);

            for (int i = 0, length = this.lightmaps.Length; i < length; i++)
            {
                combinedLightmaps[i + lightmaps.Length] = new LightmapData();
                combinedLightmaps[i + lightmaps.Length].lightmapColor = this.lightmaps[i];
            }

            ApplyStaticLightmap(rendererInfos, lightmaps.Length);
            LightmapSettings.lightmaps = combinedLightmaps;
        }

        #endregion Messages

        [ContextMenu("Test")]
        private void Test()
        {
            if (Application.isPlaying)
            {
                Debug.Log("runtime");
            }
            else
            {
                Debug.Log("editor");
            }
        }
    }
}
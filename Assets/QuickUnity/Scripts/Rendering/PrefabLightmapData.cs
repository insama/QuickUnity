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
    /// The <see cref="LightmapRendererInfo"/> store data information about lightmap.
    /// </summary>
    [Serializable]
    public struct LightmapRendererInfo
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
        private Vector4 lightmapScaleOffset;

        public Vector4 LightmapScaleOffset
        {
            get { return lightmapScaleOffset; }
            set { lightmapScaleOffset = value; }
        }
    }

    /// <summary>
    /// The class <see cref="PrefabLightmapData"/> provides lightmap data of prefab storing and reverting.
    /// </summary>
    /// <seealso cref="MonoBehaviour"/>
    [DisallowMultipleComponent, ExecuteInEditMode]
    public class PrefabLightmapData : MonoBehaviour
    {
        [SerializeField]
        private LightmapRendererInfo[] rendererInfos;

        [SerializeField]
        private Texture2D[] lightmapColors;

        [SerializeField]
        private Texture2D[] lightmapDirs;

        [SerializeField]
        private Texture2D[] shadowMasks;

        #region Properties

#if UNITY_EDITOR

        public LightmapRendererInfo[] RendererInfos
        {
            set { rendererInfos = value; }
        }

        public Texture2D[] LightmapColors
        {
            set { lightmapColors = value; }
        }

        public Texture2D[] LightmapDirs
        {
            set { lightmapDirs = value; }
        }

        public Texture2D[] ShadowMasks
        {
            set { shadowMasks = value; }
        }

#endif

        #endregion Properties

        #region Statis Methods

        /// <summary>
        /// Applies the static lightmap.
        /// </summary>
        /// <param name="instance">The instance of <see cref="PrefabLightmapData"/>.</param>
        /// <exception cref="ArgumentNullException"><c>instance</c> is <c>null</c>.</exception>
        public static void ApplyStaticLightmap(PrefabLightmapData instance)
        {
            if (!instance)
            {
                throw new ArgumentNullException("instance");
            }

            LightmapRendererInfo[] rendererInfos = instance.rendererInfos;

            if (rendererInfos == null || rendererInfos.Length == 0)
            {
                return;
            }

            Texture2D[] lightmapColors = instance.lightmapColors;
            Texture2D[] lightmapDirs = instance.lightmapDirs;
            Texture2D[] shadowMasks = instance.shadowMasks;

            LightmapData[] lightmaps = LightmapSettings.lightmaps;
            LightmapData[] combinedLightmaps = new LightmapData[lightmaps.Length + lightmapColors.Length];
            lightmaps.CopyTo(combinedLightmaps, 0);

            LightmapData[] storedLightmaps = new LightmapData[lightmapColors.Length];

            for (int i = 0, length = lightmapColors.Length; i < length; i++)
            {
                LightmapData data = new LightmapData();
                data.lightmapColor = lightmapColors[i];
                data.lightmapDir = lightmapDirs[i];
                data.shadowMask = shadowMasks[i];
                storedLightmaps[i] = data;
            }

            storedLightmaps.CopyTo(combinedLightmaps, lightmaps.Length);

            ApplyStaticLightmap(rendererInfos, lightmaps.Length);
            LightmapSettings.lightmaps = combinedLightmaps;
        }

        /// <summary>
        /// Applies the static lightmap for this prefab.
        /// </summary>
        /// <param name="infos">The <see cref="Array"/> of <see cref="LightmapRendererInfo"/> stored lightmap renderer informations.</param>
        /// <param name="lightmapOffsetIndex">Index of the lightmap offset.</param>
        private static void ApplyStaticLightmap(LightmapRendererInfo[] infos, int lightmapOffsetIndex)
        {
            for (int i = 0, length = infos.Length; i < length; i++)
            {
                LightmapRendererInfo info = infos[i];
                info.Renderer.lightmapIndex = info.LightmapIndex + lightmapOffsetIndex;
                info.Renderer.lightmapScaleOffset = info.LightmapScaleOffset;
            }
        }

        #endregion Statis Methods

        #region Messages

        /// <summary>
        /// Called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            ApplyStaticLightmap(this);
        }

        #endregion Messages
    }
}
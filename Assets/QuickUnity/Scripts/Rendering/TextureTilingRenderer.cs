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

using System.Collections.Generic;
using QuickUnity.Rendering.DataParsers;
using UnityEngine;
using System;

namespace QuickUnity.Rendering
{
    /// <summary>
    /// Specifies the data format for tiling sheet.
    /// </summary>
    public enum TilingSheetDataFormat
    {
        UnityJson
    }

    /// <summary>
    /// The class <see cref="TextureTilingRenderer"/> provides rendering method for texture tiling.
    /// </summary>
    /// <seealso cref="MonoBehaviour"/>
    public class TextureTilingRenderer : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private bool AutoUpdateMeshUV = false;

        [SerializeField]
        private TextAsset dataFileAsset = null;

        [SerializeField]
        private TilingSheetDataFormat dataFormat;

        private Mesh mesh;

        private Vector2[] meshOriginalUV;

        [SerializeField]
        private string textureTilingName;

        private Dictionary<string, Rect> tilingData;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the name of the texture tiling.
        /// </summary>
        /// <value>The name of the texture tiling.</value>
        public string TextureTilingName
        {
            get
            {
                return textureTilingName;
            }
        }

        private Mesh Mesh
        {
            get
            {
                if (!mesh)
                {
                    MeshFilter meshFilter = GetComponent<MeshFilter>();

                    if (meshFilter)
                    {
                        mesh = meshFilter.mesh;
                    }
                }

                return mesh;
            }
        }

        #endregion Properties

        #region Methods

        public void LoadData(string name, string data)
        {
            ITilingSheetDataParser parser = TilingSheetDataParser.CreateParser(dataFormat);
            tilingData = parser.ParseData(name, data);
        }

        public void UpdateMeshUV(string textureTilingName)
        {
            this.textureTilingName = textureTilingName;
            UpdateMeshUV();
        }

        public void UpdateMeshUV()
        {
            if (Mesh)
            {
                if (Mesh.uv != null)
                {
                    meshOriginalUV = new Vector2[Mesh.uv.Length];
                    Array.Copy(Mesh.uv, meshOriginalUV, Mesh.uv.Length);
                }

                if (tilingData != null && !string.IsNullOrEmpty(textureTilingName))
                {
                    if (tilingData.ContainsKey(textureTilingName))
                    {
                        // Change UV
                        Rect rect = tilingData[textureTilingName];
                        Vector2[] uvs = new Vector2[Mesh.uv.Length];

                        for (int i = 0, length = uvs.Length; i < length; i++)
                        {
                            uvs[i].x = rect.x + meshOriginalUV[i].x * rect.width;
                            uvs[i].y = rect.y + meshOriginalUV[i].y * rect.height;
                        }

                        Mesh.uv = uvs;
                    }
                }
            }
        }

        /// <summary>
        /// Called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            if (dataFileAsset)
            {
                LoadData(dataFileAsset.name, dataFileAsset.text);
            }
        }

        /// <summary>
        /// Executes .
        /// </summary>
        [ContextMenu("Execute")]
        private void Execute()
        {
            if (dataFileAsset && !string.IsNullOrEmpty(textureTilingName))
            {
                LoadData(dataFileAsset.name, dataFileAsset.text);
                UpdateMeshUV();
            }
        }

        private void Start()
        {
            if (AutoUpdateMeshUV)
            {
                UpdateMeshUV();
            }
        }

        #endregion Methods
    }
}
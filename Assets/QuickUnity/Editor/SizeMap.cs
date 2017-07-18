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
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace QuickUnityEditor
{
    /// <summary>
    /// The window for showing assets size map.
    /// </summary>
    /// <seealso cref="UnityEditor.EditorWindow"/>
    internal class SizeMap : EditorWindow
    {
        /// <summary>
        /// Dialog messasges collection.
        /// </summary>
        internal sealed class DialogMessages
        {
            /// <summary>
            /// Dialog message of no asset.
            /// </summary>
            public const string NoAssetDialog = "Sorry, found no asset!";
        }

        /// <summary>
        /// The singleton instance of SizeMap.
        /// </summary>
        private static SizeMap s_sizeMapWindow;

        /// <summary>
        /// Shows the window of Size Map.
        /// </summary>
        [MenuItem("Assets/Size Map...", false, 22)]
        public static void ShowWindow()
        {
            if (s_sizeMapWindow == null)
            {
                s_sizeMapWindow = CreateInstance<SizeMap>();
                s_sizeMapWindow.titleContent = new GUIContent("Size Map");
                s_sizeMapWindow.minSize = new Vector2(300, 500);
            }

            s_sizeMapWindow.ShowUtility();
            s_sizeMapWindow.Focus();
            s_sizeMapWindow.ShowSizeMap();
        }

        /// <summary>
        /// The position of scroll view.
        /// </summary>
        private Vector2 m_scrollViewPos;

        /// <summary>
        /// The list of assets.
        /// </summary>
        private List<ReorderableList> m_assetsList;

        /// <summary>
        /// The total runtime memory size.
        /// </summary>
        private long m_totalRuntimeMemorySize = 0;

        /// <summary>
        /// The total storage memory size.
        /// </summary>
        private long m_totalStorageMemorySize = 0;

        /// <summary>
        /// Shows the size map.
        /// </summary>
        public void ShowSizeMap()
        {
            List<string> targetAssets = new List<string>();

            string[] guids = Selection.assetGUIDs;

            for (int i = 0, length = guids.Length; i < length; ++i)
            {
                string guid = guids[i];
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                string[] paths = Utils.EditorUtil.GetObjectAssets(assetPath);

                if (paths != null)
                {
                    targetAssets.AddRange(paths);
                }
            }

            m_assetsList = GenerateAssetsList(targetAssets, out m_totalRuntimeMemorySize, out m_totalStorageMemorySize);

            if (targetAssets.Count == 0)
            {
                QuickUnityEditorApplication.DisplaySimpleDialog("", DialogMessages.NoAssetDialog);
            }

            Repaint();
        }

        #region Messages

        /// <summary>
        /// Draw editor GUI.
        /// </summary>
        private void OnGUI()
        {
            GUILayout.Space(20);

            EditorGUILayout.BeginHorizontal();

            GUILayout.Space(20);

            m_scrollViewPos = EditorGUILayout.BeginScrollView(m_scrollViewPos);

            EditorGUILayout.LabelField(string.Format("Total Assets Count: {0}", (m_assetsList != null) ? m_assetsList.Count : 0));
            EditorGUILayout.LabelField(string.Format("Total Runtime Memory Size: {0}", EditorUtility.FormatBytes(m_totalRuntimeMemorySize)));
            EditorGUILayout.LabelField(string.Format("Total Storage Memory Size: {0}", EditorUtility.FormatBytes(m_totalStorageMemorySize)));

            GUILayout.Space(20);

            if (m_assetsList != null)
            {
                for (int i = 0, length = m_assetsList.Count; i < length; ++i)
                {
                    ReorderableList list = m_assetsList[i];

                    if (list != null)
                    {
                        list.DoLayoutList();
                    }
                }
            }

            EditorGUILayout.EndScrollView();

            GUILayout.Space(20);

            EditorGUILayout.EndHorizontal();

            GUILayout.Space(20);
        }

        #endregion Messages

        /// <summary>
        /// Generates the assets list.
        /// </summary>
        /// <param name="assetPaths">The asset paths.</param>
        /// <param name="totalRuntimeMemorySize">Total size of the runtime memory.</param>
        /// <param name="totalStorageMemorySize">Total size of the storage memory.</param>
        /// <returns>The assets list.</returns>
        private List<ReorderableList> GenerateAssetsList(List<string> assetPaths, out long totalRuntimeMemorySize, out long totalStorageMemorySize)
        {
            List<ReorderableList> lists = new List<ReorderableList>();
            totalRuntimeMemorySize = 0;
            totalStorageMemorySize = 0;

            for (int i = 0, length = assetPaths.Count; i < length; ++i)
            {
                string assetPath = assetPaths[i];
                long runtimeMemorySize = Utils.EditorUtil.GetAssetRuntimeMemorySize(assetPath);
                long storageMemorySize = Utils.EditorUtil.GetAssetStorageMemorySize(assetPath);
                totalRuntimeMemorySize += runtimeMemorySize;
                totalStorageMemorySize += storageMemorySize;

                ReorderableList list = new ReorderableList(new string[] { assetPath }, typeof(string[]), false, true, false, false);
                list.elementHeight = 16;

                // Draw header.
                list.drawHeaderCallback = (Rect rect) =>
                {
                    EditorGUI.LabelField(rect, string.Format("Asset Runtime Memory Size: {0}, Storage Memory Size: {1}",
                        EditorUtility.FormatBytes(runtimeMemorySize), EditorUtility.FormatBytes(storageMemorySize)));
                };

                // Draw list element.
                list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    EditorGUI.ObjectField(rect, "", AssetDatabase.LoadAssetAtPath<Object>(assetPath), typeof(Object), true);
                };

                lists.Add(list);
            }

            return lists;
        }
    }
}
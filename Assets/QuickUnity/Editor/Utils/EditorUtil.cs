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

using CSharpExtensions.Collections.Generic;
using QuickUnity.Core.Miscs;
using QuickUnity.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace QuickUnityEditor.Utils
{
    /// <summary>
    /// Provides constants and static methods to help to do stuffs about editor.
    /// </summary>
    public static class EditorUtil
    {
        /// <summary>
        /// The extension of ScriptableObject asset.
        /// </summary>
        private const string scriptableObjectAssetExtension = ".asset";

        /// <summary>
        /// The extension of Scene asset.
        /// </summary>
        private const string sceneAssetExtension = ".unity";

        /// <summary>
        /// Determines whether the asset by the path is a scene asset.
        /// </summary>
        /// <param name="path">The path of the asset.</param>
        /// <returns><c>true</c> if it is a scene asset; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException"><c>path</c> is <c>null</c>.</exception>
        public static bool IsSceneAsset(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }

            string ext = Path.GetExtension(path);

            if (ext.Equals(sceneAssetExtension))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Loads the asset of ScriptableObject.
        /// </summary>
        /// <typeparam name="T">The Type definition of ScriptableObject.</typeparam>
        /// <param name="path">The path.</param>
        /// <param name="assetName">Name of the asset.</param>
        /// <returns>The ScriptableObject of Type definition.</returns>
        public static T LoadScriptableObjectAsset<T>(string path, string assetName = null) where T : ScriptableObject
        {
            if (string.IsNullOrEmpty(assetName))
            {
                assetName = typeof(T).Name;
            }

            string assetPath = Path.Combine(path, assetName + scriptableObjectAssetExtension);
            return AssetDatabase.LoadAssetAtPath<T>(assetPath);
        }

        /// <summary>
        /// Creates the asset of ScriptableObject.
        /// </summary>
        /// <typeparam name="T">The Type definition of ScriptableObject.</typeparam>
        /// <param name="path">The path.</param>
        /// <param name="assetName">Name of the asset.</param>
        /// <returns>The new instance of ScriptableObject of Type definition.</returns>
        public static T CreateScriptableObjectAsset<T>(string path, string assetName = null) where T : ScriptableObject
        {
            T asset = ScriptableObject.CreateInstance<T>();
            SaveScriptableObjectAsset(path, asset, assetName);
            return asset;
        }

        /// <summary>
        /// Saves the asset of ScriptableObject.
        /// </summary>
        /// <typeparam name="T">The Type definition of ScriptableObject.</typeparam>
        /// <param name="path">The path.</param>
        /// <param name="scriptableObject">The object of ScriptableObject.</param>
        /// <param name="assetName">Name of the asset.</param>
        public static void SaveScriptableObjectAsset<T>(string path, T scriptableObject, string assetName = null) where T : ScriptableObject
        {
            if (!string.IsNullOrEmpty(path))
            {
                int index = path.IndexOf(QuickUnityEditorApplication.AssetsFolderName);

                if (index == 0)
                {
                    if (string.IsNullOrEmpty(assetName))
                    {
                        assetName = typeof(T).Name;
                    }

                    assetName += scriptableObjectAssetExtension;
                    string assetPath = Path.Combine(path, assetName);

                    T targetAsset = AssetDatabase.LoadAssetAtPath<T>(assetPath);

                    if (targetAsset != null)
                    {
                        targetAsset = scriptableObject;
                        EditorUtility.SetDirty(targetAsset);
                    }
                    else
                    {
                        AssetDatabase.CreateAsset(scriptableObject, assetPath);
                    }

                    AssetDatabase.Refresh();
                    AssetDatabase.SaveAssets();
                }
            }
        }

        /// <summary>
        /// Converts absolute path to asset path.
        /// </summary>
        /// <param name="absolutePath">The absolute path.</param>
        /// <returns>The asset path for project.</returns>
        public static string ConvertToAssetPath(string absolutePath)
        {
            string projectPath = Application.dataPath;

            if (!string.IsNullOrEmpty(absolutePath) && absolutePath.IndexOf(projectPath) != -1)
            {
                int index = absolutePath.IndexOf(QuickUnityEditorApplication.AssetsFolderName);

                if (index != -1)
                {
                    return absolutePath.Substring(index);
                }
            }

            return absolutePath;
        }

        /// <summary>
        /// Gets the asset paths.
        /// </summary>
        /// <param name="nameFilter">The name filter.</param>
        /// <param name="typeFilter">The Type filter.</param>
        /// <param name="searchInFolders">The search in folders.</param>
        /// <returns>The paths about this asset name.</returns>
        public static string[] GetAssetPath(string nameFilter, string typeFilter = null, string[] searchInFolders = null)
        {
            if (string.IsNullOrEmpty(nameFilter))
                return null;

            string[] assetPaths = null;
            string fileter = nameFilter;

            if (!string.IsNullOrEmpty(typeFilter))
                fileter = string.Concat(fileter, string.Concat(" ", typeFilter));

            string[] guids = AssetDatabase.FindAssets(fileter, searchInFolders);

            if (guids != null && guids.Length > 0)
            {
                assetPaths = new string[guids.Length];

                for (int i = 0, length = guids.Length; i < length; ++i)
                {
                    string guid = guids[i];
                    string assetPath = AssetDatabase.GUIDToAssetPath(guid);

                    if (!string.IsNullOrEmpty(assetPath))
                    {
                        assetPaths[i] = assetPath;
                    }
                }
            }

            return assetPaths;
        }

        /// <summary>
        /// Gets the object assets.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>The paths of object assets.</returns>
        public static string[] GetObjectAssets(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            List<string> targetAssetPaths = new List<string>();

            if (Directory.Exists(path))
            {
                string[] guids = AssetDatabase.FindAssets("", new string[1] { path });

                for (int i = 0, length = guids.Length; i < length; ++i)
                {
                    string guid = guids[i];
                    string assetPath = AssetDatabase.GUIDToAssetPath(guid);

                    if (!Directory.Exists(assetPath))
                    {
                        targetAssetPaths.AddUnique(assetPath);
                    }
                }
            }
            else
            {
                targetAssetPaths.AddUnique(path);
            }

            return targetAssetPaths.ToArray();
        }

        /// <summary>
        /// Get the references.
        /// </summary>
        /// <param name="targetAssetPathList">The target asset path list.</param>
        /// <returns>Dictionary&lt;System.String, System.String[]&gt; The dictionary of asset references.</returns>
        public static Dictionary<string, List<string>> GetReferences(List<string> targetAssetPathList)
        {
            string[] allAssetGuids = AssetDatabase.FindAssets("");

            Dictionary<string, List<string>> references = new Dictionary<string, List<string>>();

            for (int i = 0, length = allAssetGuids.Length; i < length; ++i)
            {
                string assetGuid = allAssetGuids[i];
                string assetPath = AssetDatabase.GUIDToAssetPath(assetGuid);

                // Show the progress bar.
                EditorUtility.DisplayProgressBar("Processing...", string.Format("Checking the asset {0}...", assetPath), (float)(i + 1) / length);

                List<string> dependencies = new List<string>(AssetDatabase.GetDependencies(assetPath, true));

                for (int j = 0, count = targetAssetPathList.Count; j < count; ++j)
                {
                    string targetAssetPath = targetAssetPathList[j];

                    if (dependencies.Contains(targetAssetPath) && targetAssetPath != assetPath)
                    {
                        if (!references.ContainsKey(targetAssetPath))
                        {
                            references[targetAssetPath] = new List<string>();
                        }

                        references[targetAssetPath].AddUnique(assetPath);
                    }
                }
            }

            EditorUtility.ClearProgressBar();

            return references;
        }

        /// <summary>
        /// Deletes the meta file by asset file path.
        /// </summary>
        /// <param name="assetFilePath">The asset file path.</param>
        public static void DeleteMetaFile(string assetFilePath)
        {
            string metaFilePath = assetFilePath + QuickUnityEditorApplication.MetaFileExtension;

            if (File.Exists(metaFilePath))
            {
                try
                {
                    File.Delete(metaFilePath);
                }
                catch (Exception exception)
                {
                    DebugLogger.LogException(exception);
                }
            }
        }

        /// <summary>
        /// Gets the size of the asset runtime memory.
        /// </summary>
        /// <param name="assetPath">The asset path.</param>
        /// <returns>The runtime memory size of this asset object.</returns>
        public static long GetAssetRuntimeMemorySize(string assetPath)
        {
            UnityEngine.Object asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath);
            return GetAssetRuntimeMemorySize(asset);
        }

        /// <summary>
        /// Gets the size of the asset runtime memory.
        /// </summary>
        /// <param name="asset">The asset object.</param>
        /// <returns>The runtime memory size of this asset object.</returns>
        public static long GetAssetRuntimeMemorySize(UnityEngine.Object asset)
        {
#if UNITY_5_6_OR_NEWER
            return UnityEngine.Profiling.Profiler.GetRuntimeMemorySizeLong(asset);
#else
            return Profiler.GetRuntimeMemorySize(asset);
#endif
        }

        /// <summary>
        /// Gets the size of the asset storage memory.
        /// </summary>
        /// <param name="assetPath">The asset path.</param>
        /// <returns>The storage memory size of this asset object.</returns>
        public static long GetAssetStorageMemorySize(string assetPath)
        {
            UnityEngine.Object asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath);
            return GetAssetStorageMemorySize(asset);
        }

        /// <summary>
        /// Gets the size of the asset storage memory.
        /// </summary>
        /// <param name="asset">The asset object.</param>
        /// <returns>The storage memory size of this asset object.</returns>
        public static long GetAssetStorageMemorySize(UnityEngine.Object asset)
        {
            long size = 0;

            if (asset is Texture)
            {
                size = (int)UnityReflectionUtil.InvokeStaticMethod("UnityEditor.TextureUtil", "GetStorageMemorySize", new object[] { asset });
            }
            else
            {
                string path = AssetDatabase.GetAssetPath(asset);

                if (!string.IsNullOrEmpty(path))
                {
                    FileInfo fileInfo = new FileInfo(path);

                    if (fileInfo != null)
                    {
                        size = fileInfo.Length;
                    }
                }
            }

            return size;
        }

        /// <summary>
        /// Clears messages of console.
        /// </summary>
        public static void ClearConsole()
        {
            UnityReflectionUtil.InvokeStaticMethod("UnityEditorInternal.LogEntries", "Clear");
        }
    }
}
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

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using CSharpExtensions.Collections;
using QuickUnity.Rendering;

namespace QuickUnityEditor
{
    /// <summary>
    /// The class <see cref="GenerateLightmaps"/> provides menu items to bake lightmaps.
    /// </summary>
    internal static class GenerateLightmaps
    {
        /// <summary>
        /// Validates that one of selected items is scene asset.
        /// </summary>
        /// <returns><c>true</c> if one of selected items is scene asset, <c>false</c> otherwise.</returns>
        [MenuItem("Assets/Bake Selected Scenes", true)]
        public static bool ValidateSelectedScenes()
        {
            Object[] objects = Selection.objects;

            if (objects.Length > 0)
            {
                for (int i = 0, length = objects.Length; i < length; i++)
                {
                    Object obj = objects[i];
                    string path = AssetDatabase.GetAssetOrScenePath(obj);

                    if (Utils.EditorUtil.IsSceneAsset(path))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Bakes the selected scenes.
        /// </summary>
        [MenuItem("Assets/Bake Lightmaps for Selected Scenes", false, 500)]
        public static void BakeSelectedScenes()
        {
            List<string> toBakedScenes = new List<string>();
            Object[] objects = Selection.objects;

            if (objects.Length > 0)
            {
                for (int i = 0, length = objects.Length; i < length; i++)
                {
                    Object obj = objects[i];
                    string path = AssetDatabase.GetAssetOrScenePath(obj);

                    if (Utils.EditorUtil.IsSceneAsset(path))
                    {
                        toBakedScenes.AddUnique(path);
                    }
                }
            }

            if (toBakedScenes.Count == 0)
            {
                Debug.LogWarning("No scene to be baked!");
                return;
            }

            Lightmapping.BakeMultipleScenes(toBakedScenes.ToArray());
        }

        /// <summary>
        /// Validates the prefab lightmaps baking.
        /// </summary>
        /// <returns><c>true</c> if got <see cref="PrefabLightmapData"/> component in open scenes, <c>false</c> otherwise.</returns>
        [MenuItem("GameObject/Bake Prefab Lightmaps", true)]
        public static bool ValidatePrefabLightmapsBaking()
        {
            PrefabLightmapData[] prefabs = Object.FindObjectsOfType<PrefabLightmapData>();

            if (prefabs.Length > 0)
            {
                foreach (PrefabLightmapData item in prefabs)
                {
                    GameObject root = PrefabUtility.GetPrefabParent(item.gameObject) as GameObject;

                    if (root != null)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Bakes the prefab lightmaps.
        /// Note: Before building, you need to setup Shader stripping under the menu "Edit -&gt; Project Settings -&gt; Graphics", Set "Lightmap Modes"
        ///       to "Manual' and uncheck "Realtime Non-Directional" and "Realtime Directional".
        /// </summary>
        [MenuItem("GameObject/Bake Prefab Lightmaps", false, 100)]
        public static void BakePrefabLightmaps()
        {
            if (Lightmapping.giWorkflowMode != Lightmapping.GIWorkflowMode.OnDemand)
            {
                EditorUtility.DisplayDialog("Error", "ExtractLightmapData requires that you have baked you lightmaps and Auto mode is disabled.", "Ok");
                return;
            }

            PrefabLightmapData[] prefabs = Object.FindObjectsOfType<PrefabLightmapData>();
            MakeSureRendererGameObjectIsLightmapStatic(prefabs);

            // Bake lightmap for scene.
            Lightmapping.Bake();

            if (prefabs.Length > 0)
            {
                for (int i = 0, length = prefabs.Length; i < length; i++)
                {
                    PrefabLightmapData data = prefabs[i];
                    GameObject gameObject = data.gameObject;
                    List<LightmapRendererInfo> rendererInfos = new List<LightmapRendererInfo>();
                    List<Texture2D> lightmapColors = new List<Texture2D>();
                    List<Texture2D> lightmapDirs = new List<Texture2D>();
                    List<Texture2D> shadowMasks = new List<Texture2D>();

                    GenerateLightmapInfo(gameObject, rendererInfos, lightmapColors, lightmapDirs, shadowMasks);

                    data.RendererInfos = rendererInfos.ToArray();
                    data.LightmapColors = lightmapColors.ToArray();
                    data.LightmapDirs = lightmapDirs.ToArray();
                    data.ShadowMasks = shadowMasks.ToArray();

                    GameObject targetPrefab = PrefabUtility.GetPrefabParent(gameObject) as GameObject;

                    if (targetPrefab != null)
                    {
                        PrefabUtility.ReplacePrefab(gameObject, targetPrefab);
                    }

                    PrefabLightmapData.ApplyStaticLightmap(data);
                }
            }
        }

        /// <summary>
        /// Make sure the <see cref="GameObject"/> of renderer is lightmap static.
        /// </summary>
        /// <param name="prefabs">The <see cref="Array"/> of <see cref="PrefabLightmapData"/>.</param>
        private static void MakeSureRendererGameObjectIsLightmapStatic(PrefabLightmapData[] prefabs)
        {
            if (prefabs.Length > 0)
            {
                foreach (PrefabLightmapData lightmap in prefabs)
                {
                    MeshRenderer[] renderers = lightmap.gameObject.GetComponentsInChildren<MeshRenderer>();

                    foreach (MeshRenderer renderer in renderers)
                    {
                        GameObject gameObject = renderer.gameObject;

                        if (!GameObjectUtility.AreStaticEditorFlagsSet(gameObject, StaticEditorFlags.LightmapStatic))
                        {
                            GameObjectUtility.SetStaticEditorFlags(gameObject, StaticEditorFlags.LightmapStatic);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Generates the lightmap information.
        /// </summary>
        /// <param name="gameObject">The <see cref="GameObject"/>.</param>
        /// <param name="rendererInfos">The <see cref="List{LightmapRendererInfo}"/> to store renderer information.</param>
        /// <param name="lightmapColors">The <see cref="List{Texture2D}"/> to store <see cref="LightmapData.lightmapColor"/>.</param>
        /// <param name="lightmapDirs">The <see cref="List{Texture2D}"/> to store <see cref="LightmapData.lightmapDir"/>.</param>
        /// <param name="shadowMasks">The <see cref="List{Texture2D}"/> to store <see cref="LightmapData.shadowMask"/>.</param>
        private static void GenerateLightmapInfo(GameObject gameObject, List<LightmapRendererInfo> rendererInfos, List<Texture2D> lightmapColors,
            List<Texture2D> lightmapDirs, List<Texture2D> shadowMasks)
        {
            MeshRenderer[] renderers = gameObject.GetComponentsInChildren<MeshRenderer>();

            foreach (MeshRenderer renderer in renderers)
            {
                LightmapRendererInfo info = new LightmapRendererInfo();
                info.Renderer = renderer;
                info.LightmapScaleOffset = renderer.lightmapScaleOffset;

                LightmapData data = LightmapSettings.lightmaps[renderer.lightmapIndex];
                Texture2D lightmapColor = data.lightmapColor;
                Texture2D lightmapDir = data.lightmapDir;
                Texture2D shadowMask = data.shadowMask;

                info.LightmapIndex = lightmapColors.IndexOf(lightmapColor);

                if (info.LightmapIndex == -1)
                {
                    info.LightmapIndex = lightmapColors.Count;
                    lightmapColors.Add(lightmapColor);
                    lightmapDirs.Add(lightmapDir);
                    shadowMasks.Add(shadowMask);
                }

                rendererInfos.Add(info);
            }
        }
    }
}
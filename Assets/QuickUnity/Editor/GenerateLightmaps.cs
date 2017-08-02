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
        /// Validates the prefabs in scene whether has component of <see cref="PrefabLightmapData"/>.
        /// </summary>
        /// <returns><c>true</c> if a prefab in scene has component of <see cref="PrefabLightmapData"/>, <c>false</c> otherwise.</returns>
        [MenuItem("GameObject/Bake Lightmaps for Prefabs in Open Scenes", true)]
        public static bool ValidatePrefabsInScenes()
        {
            PrefabLightmapData[] prefabs = Object.FindObjectsOfType<PrefabLightmapData>();

            if (prefabs.Length > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Bakes lightmaps for the prefabs in scene.
        /// </summary>
        [MenuItem("GameObject/Bake Lightmaps for Prefabs in Open Scenes", false, 500)]
        public static void BakePrefabsInScenes()
        {
            if (Lightmapping.giWorkflowMode != Lightmapping.GIWorkflowMode.OnDemand)
            {
                EditorUtility.DisplayDialog("Error", "ExtractLightmapData requires that you have baked you lightmaps and Auto mode is disabled.", "Ok");
                return;
            }

            // Bake lightmap for scene.
            Lightmapping.Bake();

            PrefabLightmapData[] prefabs = Object.FindObjectsOfType<PrefabLightmapData>();

            foreach (PrefabLightmapData item in prefabs)
            {
                GameObject gameObject = item.gameObject;
                List<PrefabLightmapData.RendererInfo> rendererInfos = new List<PrefabLightmapData.RendererInfo>();
                List<Texture2D> lightmaps = new List<Texture2D>();
                GenerateLightmapInfo(gameObject, rendererInfos, lightmaps);

                item.RendererInfos = rendererInfos.ToArray();
                item.Lightmaps = lightmaps.ToArray();

                GameObject targetPrefab = PrefabUtility.GetPrefabParent(gameObject) as GameObject;

                if (targetPrefab)
                {
                    PrefabUtility.ReplacePrefab(gameObject, targetPrefab);
                }
            }
        }

        /// <summary>
        /// Generates the lightmap information for the GameObject.
        /// </summary>
        /// <param name="root">The root of the <see cref="GameObject"/>.</param>
        /// <param name="rendererInfos">The <see cref="List{PrefabLightmapData.RendererInfo}"/> to store lightmap renderer infomation.</param>
        /// <param name="lightmaps">The <see cref="List{Texture2D}"/> to store lightmap textures.</param>
        private static void GenerateLightmapInfo(GameObject root, List<PrefabLightmapData.RendererInfo> rendererInfos, List<Texture2D> lightmaps)
        {
            MeshRenderer[] renderers = root.GetComponentsInChildren<MeshRenderer>();

            foreach (MeshRenderer renderer in renderers)
            {
                if (renderer.lightmapIndex != -1)
                {
                    PrefabLightmapData.RendererInfo info = new PrefabLightmapData.RendererInfo();
                    info.Renderer = renderer;
                    info.LightmapOffsetScale = renderer.lightmapScaleOffset;

                    Texture2D lightmap = LightmapSettings.lightmaps[renderer.lightmapIndex].lightmapColor;
                    info.LightmapIndex = lightmaps.IndexOf(lightmap);

                    if (info.LightmapIndex == -1)
                    {
                        info.LightmapIndex = lightmaps.Count;
                        lightmaps.Add(lightmap);
                    }

                    rendererInfos.Add(info);
                }
            }
        }
    }
}
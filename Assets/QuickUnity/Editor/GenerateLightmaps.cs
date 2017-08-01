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
        [MenuItem("GameObject/Bake Lightmaps for Prefabs in Current Scene", true)]
        public static bool ValidatePrefabsInScene()
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
        [MenuItem("GameObject/Bake Lightmaps for Prefabs in Current Scene", false, 500)]
        public static void BakePrefabsInScene()
        {
        }
    }
}
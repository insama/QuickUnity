using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using CSharpExtensions.Collections;

namespace QuickUnityEditor
{
    /// <summary>
    /// The class <see cref="GenerateLightmapping"/> provides menu item to bake lightmap for selected scenes.
    /// </summary>
    internal static class GenerateLightmapping
    {
        /// <summary>
        /// Validates that one of selected items is scene asset.
        /// </summary>
        /// <returns><c>true</c> if one of selected items is scene asset, <c>false</c> otherwise.</returns>
        [MenuItem("Assets/Bake Selected Scenes", true)]
        public static bool ValidateScenes()
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
        [MenuItem("Assets/Bake Lightmap for Selected Scenes", false, 500)]
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
    }
}
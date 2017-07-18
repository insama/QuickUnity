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

using UnityEditor;
using UnityEngine;

namespace QuickUnityEditor
{
    /// <summary>
    /// Custom Editor for Transform.
    /// </summary>
    /// <seealso cref="UnityEditor.Editor"/>
    [CustomEditor(typeof(Transform))]
    [CanEditMultipleObjects]
    internal class TransformEditor : Editor
    {
        /// <summary>
        /// The space button texts.
        /// </summary>
        private static readonly string[] s_spaceButtonTexts = new string[] { "Local", "Global" };

        /// <summary>
        /// The selected index of space buttons.
        /// </summary>
        private int m_selectedIndex;

        /// <summary>
        /// This function is called when the ScriptableObject script is started.
        /// </summary>
        private void Awake()
        {
            m_selectedIndex = QuickUnityEditorApplication.GetEditorConfigValue<int>("TransformEditor", "selectedSpaceIndex");
        }

        /// <summary>
        /// This function is called when the scriptable object will be destroyed.
        /// </summary>
        private void OnDestroy()
        {
            QuickUnityEditorApplication.SetEditorConfigValue<int>("TransformEditor", "selectedSpaceIndex", m_selectedIndex);
        }

        /// <summary>
        /// Implement this function to make a custom inspector.
        /// </summary>
        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical();

            // Draw button bar to select local space or global space.
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            m_selectedIndex = GUILayout.Toolbar(m_selectedIndex, s_spaceButtonTexts, GUILayout.Width(250));
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(2.5f);

            // Draw properties.
            Transform targetObject = target as Transform;
            Undo.RecordObject(targetObject, "Transform Change");

            if (targetObject)
            {
                if (m_selectedIndex == 0)
                {
                    // Draw local space properties.
                    targetObject.localPosition = EditorGUILayout.Vector3Field(new GUIContent("Local Position", "Position of the transform relative to the parent transform."),
                        targetObject.localPosition);

                    targetObject.localEulerAngles = EditorGUILayout.Vector3Field(new GUIContent("Local Rotation", "The rotation as Euler angles in degrees relative to the parent transform's rotation."),
                        targetObject.localRotation.eulerAngles);

                    targetObject.localScale = EditorGUILayout.Vector3Field(new GUIContent("Local Scale", "The scale of the transform relative to the parent."),
                        targetObject.localScale);
                }
                else
                {
                    // Draw global space properties.
                    targetObject.position = EditorGUILayout.Vector3Field(new GUIContent("Position", "The position of the transform in world space."),
                        targetObject.position);

                    targetObject.eulerAngles = EditorGUILayout.Vector3Field(new GUIContent("Rotation", "The rotation as Euler angles in degrees."),
                        targetObject.rotation.eulerAngles);

                    GUI.enabled = false;
                    EditorGUILayout.Vector3Field(new GUIContent("Scale", "The global scale of the object (Read Only)."),
                        targetObject.lossyScale);
                    GUI.enabled = true;
                }
            }

            EditorGUILayout.EndVertical();
        }
    }
}
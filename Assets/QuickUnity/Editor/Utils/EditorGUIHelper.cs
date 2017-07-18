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
using UnityEngine;

namespace QuickUnityEditor.Utils
{
    /// <summary>
    /// Miscellaneous helper stuff for EditorGUI.
    /// </summary>
    /// <seealso cref="UnityEngine.GUIUtility"/>
    public sealed class EditorGUIHelper : GUIUtility
    {
        /// <summary>
        /// The collection of GUI styles.
        /// </summary>
        private class Styles
        {
            /// <summary>
            /// The style of path field.
            /// </summary>
            public static readonly GUIStyle pathFieldStyle = new GUIStyle(EditorStyles.textField)
            {
                normal = {
                    background = EditorStyles.textField.normal.background,
                    scaledBackgrounds =  EditorStyles.textField.normal.scaledBackgrounds,
                    textColor = Color.grey
                }
            };

            /// <summary>
            /// The style of browse button.
            /// </summary>
            public static readonly GUIStyle browseButtonStyle = new GUIStyle(EditorStyles.miniButton)
            {
                fixedWidth = 75,
                fixedHeight = EditorStyles.miniButton.fixedHeight + 16
            };
        }

        /// <summary>
        /// The cached GUI contents.
        /// </summary>
        private static Dictionary<string, GUIContent> s_textGUIContents;

        /// <summary>
        /// Initializes static members of the <see cref="EditorGUIUtility"/> class.
        /// </summary>
        static EditorGUIHelper()
        {
            s_textGUIContents = new Dictionary<string, GUIContent>();
        }

        /// <summary>
        /// Gets the text GUI content.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="tooltip">The tooltip.</param>
        /// <returns>The GUI content.</returns>
        public static GUIContent TextContent(string text, string tooltip)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            GUIContent guiContent = null;

            if (!s_textGUIContents.ContainsKey(text))
            {
                guiContent = new GUIContent(text);

                if (!string.IsNullOrEmpty(tooltip))
                {
                    guiContent.tooltip = tooltip;
                }

                s_textGUIContents.Add(text, guiContent);
            }
            else
            {
                guiContent = s_textGUIContents[text];
            }

            return guiContent;
        }

        /// <summary>
        /// Make a file field.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="title">The title.</param>
        /// <param name="directory">The directory.</param>
        /// <param name="filters">The filters.</param>
        /// <returns>The path of the file.</returns>
        public static string FileField(GUIContent label, string filePath, string title, string directory = "", string[] filters = null)
        {
            string text = filePath;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, new GUIContent(text), Styles.pathFieldStyle);
            bool buttonClicked = GUILayout.Button("Browse...", Styles.browseButtonStyle);

            if (buttonClicked)
            {
                string newPath = UnityEditor.EditorUtility.OpenFilePanelWithFilters(title, directory, filters);

                if (!string.IsNullOrEmpty(newPath))
                {
                    text = newPath;
                }
            }

            EditorGUILayout.EndHorizontal();
            return text;
        }

        /// <summary>
        /// Make a folder field.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="title">The title.</param>
        /// <param name="folder">The folder.</param>
        /// <param name="defaultName">The default name.</param>
        /// <returns>The path of the folder.</returns>
        public static string FolderField(GUIContent label, string filePath, string title, string folder = "", string defaultName = "")
        {
            string text = filePath;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, new GUIContent(text), Styles.pathFieldStyle);
            bool buttonClicked = GUILayout.Button("Browse...", Styles.browseButtonStyle);

            if (buttonClicked)
            {
                string newPath = UnityEditor.EditorUtility.OpenFolderPanel(title, folder, defaultName);

                if (!string.IsNullOrEmpty(newPath))
                {
                    text = newPath;
                }
            }

            EditorGUILayout.EndHorizontal();
            return text;
        }

        /// <summary>
        /// Gets the width of the label.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <param name="content">The content.</param>
        /// <returns>The width of label.</returns>
        public static float GetLabelWidth(GUIStyle style, GUIContent content)
        {
            Vector2 size = style.CalcSize(content);
            size = style.CalcScreenSize(size);
            return size.x;
        }
    }
}
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

namespace QuickUnityEditor.Preferences
{
    /// <summary>
    /// This class provide method for adding preference section "Auto Save".
    /// </summary>
    internal sealed class AutoSavePreferenceItem
    {
        /// <summary>
        /// The collections of GUI contents.
        /// </summary>
        private static class Styles
        {
            /// <summary>
            /// The style of property isAutoSaveEnabled.
            /// </summary>
            public static readonly GUIContent IsAutoSaveEnabledStyle = Utils.EditorGUIHelper.TextContent("Enable AutoSave", "Whether to automatically save after a time interval");

            /// <summary>
            /// The style of property isAutoSaveScenesEnabled.
            /// </summary>
            public static readonly GUIContent IsAutoSaveScenesEnabledStyle = Utils.EditorGUIHelper.TextContent("Save Scenes", "Whether to automatically save scenes during an autosave");

            /// <summary>
            /// The style of property isAutoSaveAssetsEnabled.
            /// </summary>
            public static readonly GUIContent IsAutoSaveAssetsEnabledStyle = Utils.EditorGUIHelper.TextContent("Save Assets", "Whether to automatically save assets during an autosave");

            /// <summary>
            /// The style of property frequencyInMinutes.
            /// </summary>
            public static readonly GUIContent FrequencyInMinutesStyle = Utils.EditorGUIHelper.TextContent("Frequency in Minutes", "The time interval after which to auto save");

            /// <summary>
            /// The style of property askWhenSaving.
            /// </summary>
            public static readonly GUIContent AskWhenSavingStyle = Utils.EditorGUIHelper.TextContent("Ask When Saving", "Whether to show confirm dialog when saving");
        }

        /// <summary>
        /// Add preferences section named "Auto Save" to the Preferences Window.
        /// </summary>
        [PreferenceItem("Auto Save")]
        public static void PreferenceGUI()
        {
            // AutoSave toggle button.
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            AutoSave.Instance.IsAutoSaveEnabled = EditorGUILayout.Toggle(Styles.IsAutoSaveEnabledStyle, AutoSave.Instance.IsAutoSaveEnabled);
            GUILayout.EndVertical();

            EditorGUI.BeginDisabledGroup(!AutoSave.Instance.IsAutoSaveEnabled);

            // AutoSave scenes toggle button.
            GUILayout.BeginVertical();
            AutoSave.Instance.IsAutoSaveScenesEnabled = EditorGUILayout.Toggle("Save Scenes", AutoSave.Instance.IsAutoSaveScenesEnabled);
            GUILayout.EndVertical();

            // AutoSave assets toggle button.
            GUILayout.BeginVertical();
            AutoSave.Instance.IsAutoSaveAssetsEnabled = EditorGUILayout.Toggle("Save Assets", AutoSave.Instance.IsAutoSaveAssetsEnabled);
            GUILayout.EndVertical();

            // Draws property AutoSaveTimeMinutes of the instance of class AutoSave.
            GUILayout.BeginVertical();
            int value = EditorGUILayout.IntField(Styles.FrequencyInMinutesStyle, (int)AutoSave.Instance.AutoSaveTimeMinutes);

            if (value >= 1)
            {
                AutoSave.Instance.AutoSaveTimeMinutes = (uint)value;
            }
            else
            {
                AutoSave.Instance.AutoSaveTimeMinutes = 1;
            }

            GUILayout.EndVertical();

            // Draws property AskWhenSaving of the instance of class AutoSave.
            GUILayout.BeginVertical();
            AutoSave.Instance.AskWhenSaving = EditorGUILayout.Toggle(Styles.AskWhenSavingStyle, AutoSave.Instance.AskWhenSaving);
            GUILayout.EndVertical();

            EditorGUI.EndDisabledGroup();
        }
    }
}
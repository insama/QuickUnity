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

using CSharpExtensions;
using CSharpExtensions.IO;
using QuickUnity;
using QuickUnity.Core.Miscs;
using QuickUnity.Data;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace QuickUnityEditor.Data
{
    /// <summary>
    /// The editor window of DataTable Preferences.
    /// </summary>
    /// <seealso cref="UnityEditor.EditorWindow"/>
    internal class DataTablePreferencesWindow : EditorWindow
    {
        /// <summary>
        /// The message collection of dialog.
        /// </summary>
        private static class DialogMessages
        {
            /// <summary>
            /// The message of making sure scripts storage location in project.
            /// </summary>
            public const string MakeSureScriptsStorageLocationInProjectMessage = "The storage location of DataTableRow scripts should be in the project!";
        }

        /// <summary>
        /// The collections of GUI contents.
        /// </summary>
        private static class Styles
        {
            /// <summary>
            /// The style of field dataTablesStorageLocation.
            /// </summary>
            public static readonly GUIContent dataTablesStorageLocationStyle = Utils.EditorGUIHelper.TextContent("DataTables Storage Location", "Where to store data tables.");

            /// <summary>
            /// The style of field dataRowScriptsStorageLocation.
            /// </summary>
            public static readonly GUIContent dataRowScriptsStorageLocationStyle = Utils.EditorGUIHelper.TextContent("DataTableRow Scripts Storage Location", "Where to store DataTableRow scripts.");

            /// <summary>
            /// The style of field autoGenerateScriptsNamespace.
            /// </summary>
            public static readonly GUIContent autoGenerateScriptsNamespaceStyle = Utils.EditorGUIHelper.TextContent("Auto Generate Scripts Namespace", "Whether to generate namespace automatically.");

            /// <summary>
            /// The style of field dataTableRowScriptsNamespace.
            /// </summary>
            public static readonly GUIContent dataTableRowScriptsNamespaceStyle = Utils.EditorGUIHelper.TextContent("DataTableRow Scripts Namespace", "The namespace of DataTableRow scripts.");

            /// <summary>
            /// The style of field dataRowsStartRow.
            /// </summary>
            public static readonly GUIContent dataRowsStartRowStyle = Utils.EditorGUIHelper.TextContent("Data rows Start Row", "The start row of data rows. (Should be > 3)");
        }

        /// <summary>
        /// The resources path.
        /// </summary>
        private static readonly string s_resourcesPath = Path.Combine(QuickUnityEditorApplication.AssetsFolderName,
            QuickUnityApplication.ResourcesFolderName);

        /// <summary>
        /// The editor window instance.
        /// </summary>
        private static DataTablePreferencesWindow s_editorWindow;

        /// <summary>
        /// Shows the editor window.
        /// </summary>
        [MenuItem("Tools/QuickUnity/DataTable/Preferences...", false, 102)]
        public static void ShowEditorWindow()
        {
            if (s_editorWindow == null)
            {
                s_editorWindow = CreateInstance<DataTablePreferencesWindow>();
                s_editorWindow.titleContent = new GUIContent("DataTable Preferences");
                s_editorWindow.minSize = new Vector2(500, 120);
            }

            s_editorWindow.ShowUtility();
            s_editorWindow.Focus();
        }

        /// <summary>
        /// Loads the preferences data.
        /// </summary>
        /// <returns>The loaded preferences data.</returns>
        public static DataTablePreferences LoadPreferencesData()
        {
            if (!AssetDatabase.IsValidFolder(s_resourcesPath))
            {
                AssetDatabase.CreateFolder(QuickUnityEditorApplication.AssetsFolderName,
                    QuickUnityApplication.ResourcesFolderName);
            }

            return Utils.EditorUtil.LoadScriptableObjectAsset<DataTablePreferences>(s_resourcesPath);
        }

        /// <summary>
        /// Creates the default preferences data.
        /// </summary>
        /// <returns>The default preferences data.</returns>
        public static DataTablePreferences CreateDefaultPreferencesData()
        {
            return Utils.EditorUtil.CreateScriptableObjectAsset<DataTablePreferences>(s_resourcesPath);
        }

        /// <summary>
        /// Saves the preference data.
        /// </summary>
        /// <param name="preferencesData">The preferences data.</param>
        public static void SavePreferenceData(DataTablePreferences preferencesData)
        {
            Utils.EditorUtil.SaveScriptableObjectAsset(s_resourcesPath, preferencesData);
        }

        /// <summary>
        /// The preferences data
        /// </summary>
        private DataTablePreferences m_preferencesData;

        /// <summary>
        /// The last data tables storage location.
        /// </summary>
        private DataTableStorageLocation m_lastDataTablesStorageLocation;

        #region Messages

        /// <summary>
        /// Called as the new window is opened.
        /// </summary>
        private void Awake()
        {
            DataTablePreferences preferencesData = LoadPreferencesData();

            if (preferencesData != null)
            {
                m_preferencesData = preferencesData;
            }
            else
            {
                m_preferencesData = CreateDefaultPreferencesData();
            }

            m_lastDataTablesStorageLocation = ObjectUtil.DeepClone(m_preferencesData.dataTablesStorageLocation);
        }

        /// <summary>
        /// Draw editor GUI.
        /// </summary>
        private void OnGUI()
        {
            CalculateLabelWidth();

            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            GUILayout.Space(10);
            m_preferencesData.dataTablesStorageLocation = (DataTableStorageLocation)EditorGUILayout.EnumPopup(Styles.dataTablesStorageLocationStyle, m_preferencesData.dataTablesStorageLocation);
            GUILayout.Space(2.5f);
            m_preferencesData.dataTableRowScriptsStorageLocation = Utils.EditorGUIHelper.FolderField(Styles.dataRowScriptsStorageLocationStyle, m_preferencesData.dataTableRowScriptsStorageLocation,
                "Data Row Scripts Storage Location",
                QuickUnityEditorApplication.AssetsFolderName);
            m_preferencesData.dataTableRowScriptsStorageLocation = Utils.EditorUtil.ConvertToAssetPath(m_preferencesData.dataTableRowScriptsStorageLocation);

            // Check scripts storage location.
            if (!string.IsNullOrEmpty(m_preferencesData.dataTableRowScriptsStorageLocation)
                && m_preferencesData.dataTableRowScriptsStorageLocation.IndexOf(QuickUnityEditorApplication.AssetsFolderName) != 0)
            {
                QuickUnityEditorApplication.DisplaySimpleDialog("", DialogMessages.MakeSureScriptsStorageLocationInProjectMessage, () =>
                {
                    m_preferencesData.dataTableRowScriptsStorageLocation = null;
                });
            }

            GUILayout.Space(2.5f);
            m_preferencesData.autoGenerateScriptsNamespace = EditorGUILayout.Toggle(Styles.autoGenerateScriptsNamespaceStyle, m_preferencesData.autoGenerateScriptsNamespace);
            GUILayout.Space(2.5f);
            EditorGUI.BeginDisabledGroup(m_preferencesData.autoGenerateScriptsNamespace);
            m_preferencesData.dataTableRowScriptsNamespace = EditorGUILayout.TextField(Styles.dataTableRowScriptsNamespaceStyle, m_preferencesData.dataTableRowScriptsNamespace);

            // Handle empty namespace.
            if (m_preferencesData.autoGenerateScriptsNamespace)
            {
                m_preferencesData.dataTableRowScriptsNamespace = null;
            }
            else if (string.IsNullOrEmpty(m_preferencesData.dataTableRowScriptsNamespace))
            {
                m_preferencesData.dataTableRowScriptsNamespace = DataTablePreferences.DefaultNamespace;
            }

            EditorGUI.EndDisabledGroup();
            GUILayout.Space(2.5f);
            m_preferencesData.dataRowsStartRow = EditorGUILayout.IntField(Styles.dataRowsStartRowStyle, m_preferencesData.dataRowsStartRow);
            m_preferencesData.dataRowsStartRow = Math.Max(m_preferencesData.dataRowsStartRow, DataTablePreferences.MinDataRowsStartRow);
            GUILayout.Space(10);
            GUILayout.EndVertical();
            GUILayout.Space(10);
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// OnDestroy is called when the EditorWindow is closed.
        /// </summary>
        private void OnDestroy()
        {
            if (m_lastDataTablesStorageLocation != m_preferencesData.dataTablesStorageLocation)
            {
                MoveDbFiles(m_lastDataTablesStorageLocation, m_preferencesData.dataTablesStorageLocation);
            }

            SavePreferenceData(m_preferencesData);
        }

        #endregion Messages

        /// <summary>
        /// Calculates the width of the label.
        /// </summary>
        private void CalculateLabelWidth()
        {
            SetEditorLabelWidth(EditorStyles.popup, Styles.dataTablesStorageLocationStyle);
            SetEditorLabelWidth(EditorStyles.label, Styles.dataRowScriptsStorageLocationStyle);
            SetEditorLabelWidth(EditorStyles.toggle, Styles.autoGenerateScriptsNamespaceStyle);
            SetEditorLabelWidth(EditorStyles.textField, Styles.dataTableRowScriptsNamespaceStyle);
        }

        /// <summary>
        /// Sets the width of the editor label.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <param name="content">The content.</param>
        private void SetEditorLabelWidth(GUIStyle style, GUIContent content)
        {
            float labelWidth = Utils.EditorGUIHelper.GetLabelWidth(style, content);

            if (labelWidth > EditorGUIUtility.labelWidth)
            {
                EditorGUIUtility.labelWidth = labelWidth;
            }
        }

        /// <summary>
        /// Moves the database files.
        /// </summary>
        /// <param name="oldLocation">The old location.</param>
        /// <param name="newLocation">The new location.</param>
        private void MoveDbFiles(DataTableStorageLocation oldLocation, DataTableStorageLocation newLocation)
        {
            string oldPath = DataImport.dataTablesLocationMap[oldLocation];

            if (!Directory.Exists(oldPath))
            {
                Directory.CreateDirectory(oldPath);
            }

            string[] filePaths = Directory.GetFiles(oldPath);

            if (filePaths != null && filePaths.Length > 0)
            {
                // Check new path.
                string newPath = DataImport.dataTablesLocationMap[newLocation];

                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }

                // Move files.
                for (int i = 0, length = filePaths.Length; i < length; ++i)
                {
                    try
                    {
                        string filePath = filePaths[i];
                        FileInfo fileInfo = new FileInfo(filePath);

                        if (newLocation == DataTableStorageLocation.ResourcesPath)
                        {
                            // Files move to Resources folder need to be renamed.
                            if (fileInfo.Extension == DataImport.BoxDbFileExtension)
                            {
                                string destPath = Path.Combine(newPath, fileInfo.GetFileNameWithoutExtension() + QuickUnityEditorApplication.BytesAssetFileExtension);
                                File.Move(filePath, destPath);
                            }
                        }
                        else if (oldLocation == DataTableStorageLocation.ResourcesPath)
                        {
                            // Files move from Resources folder also need to be renamed.
                            if (fileInfo.Extension == QuickUnityEditorApplication.BytesAssetFileExtension)
                            {
                                string destPath = Path.Combine(newPath, fileInfo.GetFileNameWithoutExtension() + DataImport.BoxDbFileExtension);
                                File.Move(filePath, destPath);
                            }
                        }
                        else
                        {
                            if (fileInfo.Extension != QuickUnityEditorApplication.MetaFileExtension)
                            {
                                string destPath = Path.Combine(newPath, fileInfo.Name);
                                File.Move(filePath, destPath);
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        DebugLogger.LogException(exception);
                    }
                }
            }

            // Delete the DataTables folder.
            try
            {
                Directory.Delete(oldPath, true);
                Utils.EditorUtil.DeleteMetaFile(oldPath);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
        }
    }
}
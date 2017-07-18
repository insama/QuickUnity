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

using CSharpExtensions.IO;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

namespace QuickUnityEditor
{
    /// <summary>
    /// Main Application class for QuickUnity.
    /// </summary>
    public sealed class QuickUnityEditorApplication
    {
        /// <summary>
        /// The label of Ok button.
        /// </summary>
        public const string OkButtonLabel = "Ok";

        /// <summary>
        /// The folder name of Assets.
        /// </summary>
        public const string AssetsFolderName = "Assets";

        /// <summary>
        /// The folder name of Resources.
        /// </summary>
        public const string ResourcesFolderName = "Resources";

        /// <summary>
        /// The folder name of Scripts.
        /// </summary>
        public const string ScriptsFolderName = "Scripts";

        /// <summary>
        /// The extension of bytes asset file.
        /// </summary>
        public const string BytesAssetFileExtension = ".bytes";

        /// <summary>
        /// The extension of meta file.
        /// </summary>
        public const string MetaFileExtension = ".meta";

        /// <summary>
        /// The enum of config file domain.
        /// </summary>
        public enum ConfigFileDomain
        {
            Editor,
            Project
        }

        /// <summary>
        /// The editor configuration file path.
        /// </summary>
        public static readonly string editorConfigFilePath = Path.Combine(new FileInfo(EditorApplication.applicationPath).DirectoryName, "Config/Editor.ini");

        /// <summary>
        /// The editor configuration file path of the project.
        /// </summary>
        public static readonly string projectEditorConfigFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Config/Editor.ini");

        /// <summary>
        /// Gets the editor configuration value.
        /// </summary>
        /// <typeparam name="T">The type definition.</typeparam>
        /// <param name="sectionName">Name of the section.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="configFileDomain">The configuration file domain.</param>
        /// <returns>The value to get.</returns>
        public static T GetEditorConfigValue<T>(string sectionName, string key, T defaultValue = default(T), ConfigFileDomain configFileDomain = ConfigFileDomain.Editor)
        {
            string configFilePath = editorConfigFilePath;

            if (configFileDomain == ConfigFileDomain.Project)
            {
                configFilePath = projectEditorConfigFilePath;
            }

            IniFileInfo fileInfo = new IniFileInfo(configFilePath);

            if (fileInfo != null)
            {
                IniSectionInfo sectionInfo = fileInfo.GetSection(sectionName);

                if (sectionInfo != null)
                {
                    return sectionInfo.GetValue<T>(key);
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// Sets the editor configuration value.
        /// </summary>
        /// <typeparam name="T">The type definition.</typeparam>
        /// <param name="sectionName">Name of the section.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="configFileDomain">The configuration file domain.</param>
        public static void SetEditorConfigValue<T>(string sectionName, string key, T value, ConfigFileDomain configFileDomain = ConfigFileDomain.Editor)
        {
            string configFilePath = editorConfigFilePath;

            if (configFileDomain == ConfigFileDomain.Project)
            {
                configFilePath = projectEditorConfigFilePath;
            }

            if (!File.Exists(configFilePath))
            {
                using (FileStream fs = File.Create(configFilePath))
                {
                }
            }

            IniFileInfo fileInfo = new IniFileInfo(configFilePath);

            if (fileInfo != null)
            {
                IniSectionInfo sectionInfo = fileInfo.GetSection(sectionName);

                if (sectionInfo == null)
                {
                    sectionInfo = new IniSectionInfo(sectionName);
                    fileInfo.AddSection(sectionName, sectionInfo);
                }

                sectionInfo.AddOrSetValue(key, value.ToString());
                fileInfo.Save();
            }
        }

        /// <summary>
        /// Displays the simple dialog.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="message">The message.</param>
        /// <param name="okButtonClickedDelegate">The ok button clicked delegate.</param>
        public static void DisplaySimpleDialog(string title, string message, Action okButtonClickedDelegate = null)
        {
            if (EditorUtility.DisplayDialog(title, message, OkButtonLabel))
            {
                if (okButtonClickedDelegate != null)
                {
                    okButtonClickedDelegate.Invoke();
                }
            }
        }
    }
}
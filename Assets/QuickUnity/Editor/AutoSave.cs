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

using CSharpExtensions.Patterns.Singleton;
using QuickUnity.Timers;
using QuickUnityEditor.Attributes;
using QuickUnityEditor.Timers;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace QuickUnityEditor
{
    /// <summary>
    /// Class to implement the feature of AutoSave.
    /// </summary>
    /// <seealso cref="QuickUnity.Patterns.Singleton{QuickUnityEditor.AutoSave}"/>
    [InitializeOnEditorStartup]
    internal class AutoSave : SingletonBase<AutoSave>
    {
        /// <summary>
        /// Class contains all config keys.
        /// </summary>
        private class ConfigKeys
        {
            /// <summary>
            /// The key of config parameter "isAutoSaveEnabled".
            /// </summary>
            public const string IsAutoSaveEnabledKey = "isAutoSaveEnabled";

            /// <summary>
            /// The key of config parameter "isAutoSaveScenesEnabled".
            /// </summary>
            public const string IsAutoSaveScenesEnabledKey = "isAutoSaveScenesEnabled";

            /// <summary>
            /// The key of config parameter "isAutoSaveAssetsEnabled".
            /// </summary>
            public const string IsAutoSaveAssetsEnabledKey = "isAutoSaveAssetsEnabled";

            /// <summary>
            /// The key of config parameter "autoSaveTimeMinutes".
            /// </summary>
            public const string AutoSaveTimeMinutesKey = "autoSaveTimeMinutes";

            /// <summary>
            /// The key of config parameter "askWhenSaving".
            /// </summary>
            public const string AskWhenSavingKey = "askWhenSaving";
        }

        /// <summary>
        /// The configuration section.
        /// </summary>
        private const string configSection = "AutoSave";

        /// <summary>
        /// Whether initialize is complete.
        /// </summary>
        private bool initialized = false;

        /// <summary>
        /// The timer of autosave.
        /// </summary>
        private ITimer autosaveTimer;

        /// <summary>
        /// Whether the data is dirty.
        /// </summary>
        private bool isDirty = false;

        /// <summary>
        /// Whether to enable AutoSave feature.
        /// </summary>
        private bool isAutoSaveEnabled = false;

        /// <summary>
        /// Sets a value indicating whether to enable AutoSave feature.
        /// </summary>
        /// <value><c>true</c> if the feature of AutoSave is enabled; otherwise, <c>false</c>.</value>
        public bool IsAutoSaveEnabled
        {
            get
            {
                if (!initialized)
                {
                    isAutoSaveEnabled = QuickUnityEditorApplication.GetEditorConfigValue<bool>(configSection, ConfigKeys.IsAutoSaveEnabledKey, false);
                }

                return isAutoSaveEnabled;
            }

            set
            {
                if (isAutoSaveEnabled != value)
                {
                    isAutoSaveEnabled = value;
                    QuickUnityEditorApplication.SetEditorConfigValue(configSection, ConfigKeys.IsAutoSaveEnabledKey, value);
                    isDirty = true;
                }
            }
        }

        /// <summary>
        /// Whether to auto save scenes.
        /// </summary>
        private bool isAutoSaveScenesEnabled = true;

        /// <summary>
        /// Gets or sets a value indicating whether it is automatic save scenes enabled.
        /// </summary>
        /// <value><c>true</c> if it is automatic save scenes enabled; otherwise, <c>false</c>.</value>
        public bool IsAutoSaveScenesEnabled
        {
            get
            {
                if (!initialized)
                {
                    isAutoSaveScenesEnabled = QuickUnityEditorApplication.GetEditorConfigValue<bool>(configSection, ConfigKeys.IsAutoSaveScenesEnabledKey, true);
                }

                return isAutoSaveScenesEnabled;
            }

            set
            {
                if (isAutoSaveScenesEnabled != value)
                {
                    isAutoSaveScenesEnabled = value;
                    QuickUnityEditorApplication.SetEditorConfigValue(configSection, ConfigKeys.IsAutoSaveScenesEnabledKey, value);
                    isDirty = true;
                }
            }
        }

        /// <summary>
        /// Whether to save assets automatically.
        /// </summary>
        private bool isAutoSaveAssetsEnabled = true;

        /// <summary>
        /// Gets or sets a value indicating whether it is automatic save assets enabled.
        /// </summary>
        /// <value><c>true</c> if it is automatic save assets enabled; otherwise, <c>false</c>.</value>
        public bool IsAutoSaveAssetsEnabled
        {
            get
            {
                if (!initialized)
                {
                    isAutoSaveAssetsEnabled = QuickUnityEditorApplication.GetEditorConfigValue<bool>(configSection, ConfigKeys.IsAutoSaveAssetsEnabledKey, true);
                }

                return isAutoSaveAssetsEnabled;
            }

            set
            {
                if (isAutoSaveAssetsEnabled != value)
                {
                    isAutoSaveAssetsEnabled = value;
                    QuickUnityEditorApplication.SetEditorConfigValue(configSection, ConfigKeys.IsAutoSaveAssetsEnabledKey, value);
                    isDirty = true;
                }
            }
        }

        /// <summary>
        /// The time interval after which to autosave.
        /// </summary>
        private uint autoSaveTimeMinutes = 10;

        /// <summary>
        /// Gets or sets the automatic save time minutes.
        /// </summary>
        /// <value>The automatic save time minutes.</value>
        public uint AutoSaveTimeMinutes
        {
            get
            {
                if (!initialized)
                {
                    autoSaveTimeMinutes = QuickUnityEditorApplication.GetEditorConfigValue<uint>(configSection, ConfigKeys.AutoSaveTimeMinutesKey, 10);
                }

                return autoSaveTimeMinutes;
            }

            set
            {
                if (autoSaveTimeMinutes != value)
                {
                    autoSaveTimeMinutes = value;
                    QuickUnityEditorApplication.SetEditorConfigValue(configSection, ConfigKeys.AutoSaveTimeMinutesKey, value);
                    isDirty = true;
                }
            }
        }

        /// <summary>
        /// Whether to show confirm dialog when autosave.
        /// </summary>
        private bool askWhenSaving = true;

        /// <summary>
        /// Gets or sets a value indicating whether [show confirm dialog].
        /// </summary>
        /// <value><c>true</c> if [show confirm dialog]; otherwise, <c>false</c>.</value>
        public bool AskWhenSaving
        {
            get
            {
                if (!initialized)
                {
                    askWhenSaving = QuickUnityEditorApplication.GetEditorConfigValue<bool>(configSection, ConfigKeys.AskWhenSavingKey, true);
                }

                return askWhenSaving;
            }

            set
            {
                if (askWhenSaving != value)
                {
                    askWhenSaving = value;
                    QuickUnityEditorApplication.SetEditorConfigValue(configSection, ConfigKeys.AskWhenSavingKey, value);
                    isDirty = true;
                }
            }
        }

        /// <summary>
        /// Initializes static members of the <see cref="AutoSave"/> class.
        /// </summary>
        static AutoSave()
        {
            Instance.Initialize();
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            if (!EditorApplication.isPlaying && !initialized)
            {
                initialized = false;

                isAutoSaveEnabled = IsAutoSaveEnabled;
                isAutoSaveScenesEnabled = IsAutoSaveScenesEnabled;
                isAutoSaveAssetsEnabled = IsAutoSaveAssetsEnabled;
                autoSaveTimeMinutes = AutoSaveTimeMinutes;
                askWhenSaving = AskWhenSaving;

                initialized = true;

                // Initialize autosave timer.
                if (autosaveTimer == null)
                {
                    autosaveTimer = new EditorTimer(1, AutoSaveTimeMinutes * 60);
                    autosaveTimer.AddEventListener<TimerEvent>(TimerEvent.Timer, OnAutosaveTimer);
                    autosaveTimer.AddEventListener<TimerEvent>(TimerEvent.TimerComplete, OnAutosaveTimerComplete);
                }
            }
        }

        /// <summary>
        /// Called when [autosave timer].
        /// </summary>
        /// <param name="timerEvent">The timer event.</param>
        private void OnAutosaveTimer(TimerEvent timerEvent)
        {
            if (isDirty)
            {
                autosaveTimer.Reset();
                autosaveTimer.RepeatCount = AutoSaveTimeMinutes * 60;
                isDirty = false;
                autosaveTimer.Start();
            }
        }

        /// <summary>
        /// Called when [autosave timer complete].
        /// </summary>
        /// <param name="timerEvent">The timer event.</param>
        private void OnAutosaveTimerComplete(TimerEvent timerEvent)
        {
            if (isAutoSaveEnabled && !EditorApplication.isPlaying)
            {
                if (AskWhenSaving)
                {
                    if (EditorUtility.DisplayDialog("Auto Save", "Do you want to save project?", "Yes", "No"))
                    {
                        AutoSaveProject();
                    }
                    else
                    {
                        autosaveTimer.Start();
                    }
                }
                else
                {
                    AutoSaveProject();
                }
            }
        }

        /// <summary>
        /// Save project automatically.
        /// </summary>
        private void AutoSaveProject()
        {
            if (IsAutoSaveScenesEnabled)
            {
                // Save scenes.
                EditorSceneManager.SaveOpenScenes();
            }

            if (isAutoSaveAssetsEnabled)
            {
                // Save assets.
                EditorApplication.SaveAssets();
            }

            autosaveTimer.Start();
        }
    }
}
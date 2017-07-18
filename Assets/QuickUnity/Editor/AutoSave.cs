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
            public const string isAutoSaveEnabledKey = "isAutoSaveEnabled";

            /// <summary>
            /// The key of config parameter "isAutoSaveScenesEnabled".
            /// </summary>
            public const string isAutoSaveScenesEnabledKey = "isAutoSaveScenesEnabled";

            /// <summary>
            /// The key of config parameter "isAutoSaveAssetsEnabled".
            /// </summary>
            public const string isAutoSaveAssetsEnabledKey = "isAutoSaveAssetsEnabled";

            /// <summary>
            /// The key of config parameter "autoSaveTimeMinutes".
            /// </summary>
            public const string autoSaveTimeMinutesKey = "autoSaveTimeMinutes";

            /// <summary>
            /// The key of config parameter "askWhenSaving".
            /// </summary>
            public const string askWhenSavingKey = "askWhenSaving";
        }

        /// <summary>
        /// The configuration section.
        /// </summary>
        private const string ConfigSection = "AutoSave";

        /// <summary>
        /// Whether initialize is complete.
        /// </summary>
        public bool m_initialized = false;

        /// <summary>
        /// The timer of autosave.
        /// </summary>
        private ITimer m_autosaveTimer;

        /// <summary>
        /// Whether the data is dirty.
        /// </summary>
        private bool m_isDirty = false;

        /// <summary>
        /// Whether to enable AutoSave feature.
        /// </summary>
        private bool m_isAutoSaveEnabled = false;

        /// <summary>
        /// Sets a value indicating whether to enable AutoSave feature.
        /// </summary>
        /// <value><c>true</c> if the feature of AutoSave is enabled; otherwise, <c>false</c>.</value>
        public bool isAutoSaveEnabled
        {
            get
            {
                if (!m_initialized)
                {
                    m_isAutoSaveEnabled = QuickUnityEditorApplication.GetEditorConfigValue<bool>(ConfigSection, ConfigKeys.isAutoSaveEnabledKey, false);
                }

                return m_isAutoSaveEnabled;
            }

            set
            {
                if (m_isAutoSaveEnabled != value)
                {
                    m_isAutoSaveEnabled = value;
                    QuickUnityEditorApplication.SetEditorConfigValue(ConfigSection, ConfigKeys.isAutoSaveEnabledKey, value);
                    m_isDirty = true;
                }
            }
        }

        /// <summary>
        /// Whether to auto save scenes.
        /// </summary>
        private bool m_isAutoSaveScenesEnabled = true;

        /// <summary>
        /// Gets or sets a value indicating whether it is automatic save scenes enabled.
        /// </summary>
        /// <value><c>true</c> if it is automatic save scenes enabled; otherwise, <c>false</c>.</value>
        public bool isAutoSaveScenesEnabled
        {
            get
            {
                if (!m_initialized)
                {
                    m_isAutoSaveScenesEnabled = QuickUnityEditorApplication.GetEditorConfigValue<bool>(ConfigSection, ConfigKeys.isAutoSaveScenesEnabledKey, true);
                }

                return m_isAutoSaveScenesEnabled;
            }

            set
            {
                if (m_isAutoSaveScenesEnabled != value)
                {
                    m_isAutoSaveScenesEnabled = value;
                    QuickUnityEditorApplication.SetEditorConfigValue(ConfigSection, ConfigKeys.isAutoSaveScenesEnabledKey, value);
                    m_isDirty = true;
                }
            }
        }

        /// <summary>
        /// Whether to auto save assets.
        /// </summary>
        private bool m_isAutoSaveAssetsEnabled = true;

        /// <summary>
        /// Gets or sets a value indicating whether it is automatic save assets enabled.
        /// </summary>
        /// <value><c>true</c> if it is automatic save assets enabled; otherwise, <c>false</c>.</value>
        public bool isAutoSaveAssetsEnabled
        {
            get
            {
                if (!m_initialized)
                {
                    m_isAutoSaveAssetsEnabled = QuickUnityEditorApplication.GetEditorConfigValue<bool>(ConfigSection, ConfigKeys.isAutoSaveAssetsEnabledKey, true);
                }

                return m_isAutoSaveAssetsEnabled;
            }

            set
            {
                if (m_isAutoSaveAssetsEnabled != value)
                {
                    m_isAutoSaveAssetsEnabled = value;
                    QuickUnityEditorApplication.SetEditorConfigValue(ConfigSection, ConfigKeys.isAutoSaveAssetsEnabledKey, value);
                    m_isDirty = true;
                }
            }
        }

        /// <summary>
        /// The time interval after which to autosave.
        /// </summary>
        private uint m_autoSaveTimeMinutes = 10;

        /// <summary>
        /// Gets or sets the automatic save time minutes.
        /// </summary>
        /// <value>The automatic save time minutes.</value>
        public uint autoSaveTimeMinutes
        {
            get
            {
                if (!m_initialized)
                {
                    m_autoSaveTimeMinutes = QuickUnityEditorApplication.GetEditorConfigValue<uint>(ConfigSection, ConfigKeys.autoSaveTimeMinutesKey, 10);
                }

                return m_autoSaveTimeMinutes;
            }

            set
            {
                if (m_autoSaveTimeMinutes != value)
                {
                    m_autoSaveTimeMinutes = value;
                    QuickUnityEditorApplication.SetEditorConfigValue(ConfigSection, ConfigKeys.autoSaveTimeMinutesKey, value);
                    m_isDirty = true;
                }
            }
        }

        /// <summary>
        /// Whether to show confirm dialog when autosave.
        /// </summary>
        private bool m_askWhenSaving = true;

        /// <summary>
        /// Gets or sets a value indicating whether [show confirm dialog].
        /// </summary>
        /// <value><c>true</c> if [show confirm dialog]; otherwise, <c>false</c>.</value>
        public bool askWhenSaving
        {
            get
            {
                if (!m_initialized)
                {
                    m_askWhenSaving = QuickUnityEditorApplication.GetEditorConfigValue<bool>(ConfigSection, ConfigKeys.askWhenSavingKey, true);
                }

                return m_askWhenSaving;
            }

            set
            {
                if (m_askWhenSaving != value)
                {
                    m_askWhenSaving = value;
                    QuickUnityEditorApplication.SetEditorConfigValue(ConfigSection, ConfigKeys.askWhenSavingKey, value);
                    m_isDirty = true;
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
            if (!EditorApplication.isPlaying && !m_initialized)
            {
                m_initialized = false;

                m_isAutoSaveEnabled = isAutoSaveEnabled;
                m_isAutoSaveScenesEnabled = isAutoSaveScenesEnabled;
                m_isAutoSaveAssetsEnabled = isAutoSaveAssetsEnabled;
                m_autoSaveTimeMinutes = autoSaveTimeMinutes;
                m_askWhenSaving = askWhenSaving;

                m_initialized = true;

                // Initialize autosave timer.
                if (m_autosaveTimer == null)
                {
                    m_autosaveTimer = new EditorTimer(1, autoSaveTimeMinutes * 60);
                    m_autosaveTimer.AddEventListener<TimerEvent>(TimerEvent.Timer, OnAutosaveTimer);
                    m_autosaveTimer.AddEventListener<TimerEvent>(TimerEvent.TimerComplete, OnAutosaveTimerComplete);
                }
            }
        }

        /// <summary>
        /// Called when [autosave timer].
        /// </summary>
        /// <param name="timerEvent">The timer event.</param>
        private void OnAutosaveTimer(TimerEvent timerEvent)
        {
            if (m_isDirty)
            {
                m_autosaveTimer.Reset();
                m_autosaveTimer.repeatCount = autoSaveTimeMinutes * 60;
                m_isDirty = false;
                m_autosaveTimer.Start();
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
                if (askWhenSaving)
                {
                    if (EditorUtility.DisplayDialog("Auto Save", "Do you want to save project?", "Yes", "No"))
                    {
                        AutoSaveProject();
                    }
                    else
                    {
                        m_autosaveTimer.Start();
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
            if (isAutoSaveScenesEnabled)
            {
                // Save scenes.
                EditorSceneManager.SaveOpenScenes();
            }

            if (m_isAutoSaveAssetsEnabled)
            {
                // Save assets.
                EditorApplication.SaveAssets();
            }

            m_autosaveTimer.Start();
        }
    }
}
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

using QuickUnity.Timers;
using QuickUnityEditor.Attributes;
using System;
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CSharpExtensions.Patterns.Singleton;

namespace QuickUnityEditor
{
    /// <summary>
    /// The EditorTimerManager is a convenience class for managing editor timer systems. This class
    /// cannot be inherited.
    /// </summary>
    /// <seealso cref="SingletonBase{EditorTimerManager}"/>
    /// <seealso cref="ITimerCollection"/>
    [InitializeOnEditorStartup(int.MaxValue)]
    internal sealed class EditorTimerManager : SingletonBase<EditorTimerManager>, ITimerCollection
    {
        /// <summary>
        /// The last time.
        /// </summary>
        private double lastTime = 0.0d;

        /// <summary>
        /// The timer list.
        /// </summary>
        private ITimerList timerList;

        /// <summary>
        /// Initializes static members of the <see cref="EditorTimerManager"/> class.
        /// </summary>
        static EditorTimerManager()
        {
            Instance.Initialize();
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            lastTime = EditorApplication.timeSinceStartup;
            timerList = new TimerList();
            EditorApplication.update += OnEditorUpdate;
        }

        #region ITimerCollection Interface

        /// <summary>
        /// Gets the number of <see cref="ITimer"/> elements contained in the <see cref="EditorTimerManager"/>.
        /// </summary>
        /// <value>The number of <see cref="ITimer"/> elements contained in the <see cref="EditorTimerManager"/>.</value>
        public int Count
        {
            get
            {
                if (timerList != null)
                {
                    return timerList.Count;
                }

                return 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="EditorTimerManager"/> is read-only.
        /// </summary>
        /// <value>
        /// <c>true</c> if the <see cref="EditorTimerManager"/> is read-only; otherwise, <c>false</c>.
        /// </value>
        bool ICollection<ITimer>.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Copies the elements of the <see cref="EditorTimerManager"/> to an <see
        /// cref="System.Array"/>, starting at a particular <see cref="System.Array"/> index.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional <see cref="System.Array"/> that is the destination of the elements
        /// copied from <see cref="EditorTimerManager"/>. The <see cref="System.Array"/> must have
        /// zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in <c>array</c> at which copying begins.</param>
        void ICollection<ITimer>.CopyTo(ITimer[] array, int arrayIndex)
        {
            if (timerList != null)
            {
                timerList.CopyTo(array, arrayIndex);
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        IEnumerator<ITimer> IEnumerable<ITimer>.GetEnumerator()
        {
            if (timerList != null)
            {
                return timerList.GetEnumerator();
            }

            return null;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            if (timerList != null)
            {
                return timerList.GetEnumerator();
            }

            return null;
        }

        /// <summary>
        /// Adds an <see cref="QuickUnity.Timers.ITimer"/> item to the <see cref="QuickUnity.Timers.ITimerCollection"/>.
        /// </summary>
        /// <param name="item">
        /// The <see cref="QuickUnity.Timers.ITimer"/> object to add to the <see cref="QuickUnity.Timers.ITimerCollection"/>.
        /// </param>
        public void Add(ITimer item)
        {
            if (timerList != null)
            {
                timerList.Add(item);
            }
        }

        /// <summary>
        /// Removes all <see cref="QuickUnity.Timers.ITimer"/> items from the <see cref="QuickUnity.Timers.ITimerCollection"/>.
        /// </summary>
        public void Clear()
        {
            if (timerList != null)
            {
                timerList.Clear();
            }
        }

        /// <summary>
        /// Determines whether the <see cref="QuickUnity.Timers.ITimerCollection"/>. contains a
        /// specific <see cref="QuickUnity.Timers.ITimer"/> object.
        /// </summary>
        /// <param name="item">
        /// The <see cref="QuickUnity.Timers.ITimer"/> object to locate in the <see cref="QuickUnity.Timers.ITimerCollection"/>.
        /// </param>
        /// <returns>
        /// <c>true</c> if <see cref="QuickUnity.Timers.ITimer"/> item is found in the <see
        /// cref="QuickUnity.Timers.ITimerCollection"/>; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(ITimer item)
        {
            if (timerList != null)
            {
                return timerList.Contains(item);
            }

            return false;
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="QuickUnity.Timers.ITimerCollection"/>.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="QuickUnity.Timers.ITimerCollection"/>.</param>
        /// <returns>
        /// <c>true</c> if item was successfully removed from the <see
        /// cref="QuickUnity.Timers.ITimerCollection"/>; otherwise, <c>false</c>. This method also
        /// returns <c>false</c> if item is not found in the original <see cref="QuickUnity.Timers.ITimerCollection"/>.
        /// </returns>
        public bool Remove(ITimer item)
        {
            if (timerList != null)
            {
                return timerList.Remove(item);
            }

            return false;
        }

        /// <summary>
        /// Pauses all timers in the <see cref="QuickUnity.Timers.ITimerCollection"/>.
        /// </summary>
        public void PauseAll()
        {
        }

        /// <summary>
        /// Resets all timers in the <see cref="QuickUnity.Timers.ITimerCollection"/>.
        /// </summary>
        public void ResetAll()
        {
            if (timerList != null)
            {
                timerList.ResetAll();
            }
        }

        /// <summary>
        /// Resumes all timers in <see cref="QuickUnity.Timers.ITimerCollection"/>.
        /// </summary>
        public void ResumeAll()
        {
            if (timerList != null)
            {
                timerList.ResumeAll();
            }
        }

        /// <summary>
        /// Sets all timers in the <see cref="QuickUnity.Timers.ITimerCollection"/> to be enabled or not.
        /// </summary>
        /// <param name="value">
        /// Set to <c>true</c> to enable all timers in the <see
        /// cref="QuickUnity.Timers.ITimerCollection"/> control to trigger their timer event;
        /// otherwise, set to <c>false</c>.
        /// </param>
        public void SetAllEnabled(bool value = true)
        {
            if (timerList != null)
            {
                timerList.SetAllEnabled(value);
            }
        }

        /// <summary>
        /// Starts all timers in the <see cref="QuickUnity.Timers.ITimerCollection"/>.
        /// </summary>
        public void StartAll()
        {
            if (timerList != null)
            {
                timerList.StartAll();
            }
        }

        /// <summary>
        /// Stops all timers in the <see cref="QuickUnity.Timers.ITimerCollection"/>.
        /// </summary>
        public void StopAll()
        {
            if (timerList != null)
            {
                timerList.StopAll();
            }
        }

        #endregion ITimerCollection Interface

        #region Private Functions

        /// <summary>
        /// Called when [editor update].
        /// </summary>
        private void OnEditorUpdate()
        {
            float deltaTime = (float)(EditorApplication.timeSinceStartup - lastTime);
            lastTime = EditorApplication.timeSinceStartup;

            if (timerList != null)
            {
                timerList.ForEach((timer) =>
                {
                    try
                    {
                        if (!timer.IgnoreTimeScale)
                        {
                            deltaTime = deltaTime * Time.timeScale;
                        }

                        timer.Tick(deltaTime);
                    }
                    catch (Exception exception)
                    {
                        Debug.LogException(exception);
                    }
                });
            }
        }

        #endregion Private Functions
    }
}
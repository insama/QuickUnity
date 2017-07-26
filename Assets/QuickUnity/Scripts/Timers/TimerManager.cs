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

using QuickUnity.Patterns.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuickUnity.Timers
{
    /// <summary>
    /// The TimerManager is a convenience class for managing timer systems. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="SingletonMonoBehaviour{TimerManager}"/>
    /// <seealso cref="ITimerCollection"/>
    public sealed class TimerManager : SingletonMonoBehaviour<TimerManager>, ITimerCollection
    {
        /// <summary>
        /// The timer list.
        /// </summary>
        private ITimerList timerList;

        #region Messages

        /// <summary>
        /// Called when script receive message Awake.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            timerList = new TimerList();
        }

        /// <summary>
        /// This function is called when the behaviour becomes enabled and active.
        /// </summary>
        private void OnEnable()
        {
            SetAllEnabled(true);
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled () or inactive.
        /// </summary>
        private void OnDisable()
        {
            SetAllEnabled(false);
        }

        /// <summary>
        /// This function is called when the application pauses.
        /// </summary>
        /// <param name="pauseStatus"><c>true</c> if the application is paused, else <c>false</c>.</param>
        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                // Pause all timers.
                PauseAll();
            }
            else
            {
                // Resume all timers.
                ResumeAll();
            }
        }

        /// <summary>
        /// Update is called every frame.
        /// </summary>
        private void Update()
        {
            if (timerList != null)
            {
                timerList.ForEach((timer) =>
                {
                    float deltaTime = Time.deltaTime;

                    try
                    {
                        if (timer.IgnoreTimeScale)
                        {
                            deltaTime = Time.unscaledDeltaTime;
                        }

                        timer.Tick(deltaTime);
                    }
                    catch (Exception exception)
                    {
                        Debug.LogException(exception, this);
                    }
                });
            }
        }

        /// <summary>
        /// This function is called when the MonoBehaviour will be destroyed.
        /// </summary>
        private void OnDestroy()
        {
            Clear();
            timerList = null;
        }

        #endregion Messages

        #region ITimerCollection Interface

        /// <summary>
        /// Gets the number of <see cref="ITimer"/> elements contained in the <see cref="TimerManager"/>.
        /// </summary>
        /// <value>The number of <see cref="ITimer"/> elements contained in the <see cref="TimerManager"/>.</value>
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
        /// Gets a value indicating whether the <see cref="TimerManager"/> is read-only.
        /// </summary>
        /// <value><c>true</c> if the <see cref="TimerManager"/> is read-only; otherwise, <c>false</c>.</value>
        bool ICollection<ITimer>.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Copies the elements of the <see cref="TimerManager"/> to an <see cref="System.Array"/>,
        /// starting at a particular <see cref="System.Array"/> index.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional <see cref="System.Array"/> that is the destination of the elements
        /// copied from <see cref="TimerManager"/>. The <see cref="System.Array"/> must have
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
        /// Adds an <see cref="ITimer"/> item to the <see cref="ITimerCollection"/>.
        /// </summary>
        /// <param name="item">The <see cref="ITimer"/> object to add to the <see cref="ITimerCollection"/>.</param>
        public void Add(ITimer item)
        {
            if (timerList != null)
            {
                timerList.Add(item);
            }
        }

        /// <summary>
        /// Removes all <see cref="ITimer"/> items from the <see cref="ITimerCollection"/>.
        /// </summary>
        public void Clear()
        {
            if (timerList != null)
            {
                timerList.Clear();
            }
        }

        /// <summary>
        /// Determines whether the <see cref="ITimerCollection"/>. contains a specific <see
        /// cref="ITimer"/> object.
        /// </summary>
        /// <param name="item">The <see cref="ITimer"/> object to locate in the <see cref="ITimerCollection"/>.</param>
        /// <returns>
        /// <c>true</c> if <see cref="ITimer"/> item is found in the <see cref="ITimerCollection"/>;
        /// otherwise, <c>false</c>.
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
        /// Removes the first occurrence of a specific object from the <see cref="ITimerCollection"/>.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="ITimerCollection"/>.</param>
        /// <returns>
        /// <c>true</c> if item was successfully removed from the <see cref="ITimerCollection"/>;
        /// otherwise, <c>false</c>. This method also returns <c>false</c> if item is not found in
        /// the original <see cref="ITimerCollection"/>.
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
        /// Pauses all timers in the <see cref="ITimerCollection"/>.
        /// </summary>
        public void PauseAll()
        {
            if (timerList != null)
            {
                timerList.PauseAll();
            }
        }

        /// <summary>
        /// Resets all timers in the <see cref="ITimerCollection"/>.
        /// </summary>
        public void ResetAll()
        {
            if (timerList != null)
            {
                timerList.ResetAll();
            }
        }

        /// <summary>
        /// Resumes all timers in <see cref="ITimerCollection"/>.
        /// </summary>
        public void ResumeAll()
        {
            if (timerList != null)
            {
                timerList.ResumeAll();
            }
        }

        /// <summary>
        /// Sets all timers in the <see cref="ITimerCollection"/> to be enabled or not.
        /// </summary>
        /// <param name="value">
        /// Set to <c>true</c> to enable all timers in the <see cref="ITimerCollection"/> control to
        /// trigger their timer event; otherwise, set to <c>false</c>.
        /// </param>
        public void SetAllEnabled(bool value = true)
        {
            if (timerList != null)
            {
                timerList.SetAllEnabled(value);
            }
        }

        /// <summary>
        /// Starts all timers in the <see cref="ITimerCollection"/>.
        /// </summary>
        public void StartAll()
        {
            if (timerList != null)
            {
                timerList.StartAll();
            }
        }

        /// <summary>
        /// Stops all timers in the <see cref="ITimerCollection"/>.
        /// </summary>
        public void StopAll()
        {
            if (timerList != null)
            {
                timerList.StopAll();
            }
        }

        #endregion ITimerCollection Interface
    }
}
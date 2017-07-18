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

using System;
using System.Collections.ObjectModel;
using System.Collections;
using System.Collections.Generic;
using CSharpExtensions.Collections.Generic;
using CSharpExtensions.Events;

namespace QuickUnity.Timers
{
    /// <summary>
    /// Class used internally to store the list of <see cref="ITimer"/> objects.
    /// </summary>
    /// <seealso cref=".ITimerList"/>
    /// <seealso cref="EventDispatcher"/>
    public class TimerList : EventDispatcher, ITimerList
    {
        /// <summary>
        /// The timer collection.
        /// </summary>
        private Collection<ITimer> m_timerCollection;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerList"/> class.
        /// </summary>
        public TimerList()
            : base()
        {
            m_timerCollection = new Collection<ITimer>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerList"/> class.
        /// </summary>
        /// <param name="timers">The timers array.</param>
        public TimerList(params ITimer[] timers)
            : base()
        {
            m_timerCollection = new Collection<ITimer>(timers);
        }

        #endregion Constructors

        #region ITimerList

        /// <summary>
        /// Gets the number of <see cref="ITimer"/> elements contained in the <see cref="TimerList"/>.
        /// </summary>
        /// <value>The number of <see cref="ITimer"/> elements contained in the <see cref="TimerList"/>.</value>
        public int Count
        {
            get
            {
                if (m_timerCollection != null)
                {
                    return m_timerCollection.Count;
                }

                return 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="TimerList"/> is read-only.
        /// </summary>
        /// <value><c>true</c> if the <see cref="TimerList"/> is read-only; otherwise, <c>false</c>.</value>
        bool ICollection<ITimer>.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Copies the elements of the <see cref="TimerList"/> to an <see cref="System.Array"/>, starting at a particular <see cref="System.Array"/> index.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional <see cref="System.Array"/> that is the destination of the elements copied from <see cref="TimerList"/>. The <see
        /// cref="System.Array"/> must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in <c>array</c> at which copying begins.</param>
        void ICollection<ITimer>.CopyTo(ITimer[] array, int arrayIndex)
        {
            if (m_timerCollection != null)
            {
                m_timerCollection.CopyTo(array, arrayIndex);
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        IEnumerator<ITimer> IEnumerable<ITimer>.GetEnumerator()
        {
            if (m_timerCollection != null)
            {
                return m_timerCollection.GetEnumerator();
            }

            return null;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            if (m_timerCollection != null)
            {
                return m_timerCollection.GetEnumerator();
            }

            return null;
        }

        /// <summary>
        /// Adds an <see cref="ITimer"/> item to the <see cref="TimerList"/>.
        /// </summary>
        /// <param name="item">The <see cref="ITimer"/> object to add to the <see cref="TimerList"/>.</param>
        public void Add(ITimer item)
        {
            if (m_timerCollection != null && item != null)
            {
                m_timerCollection.AddUnique(item);
            }
        }

        /// <summary>
        /// Removes all <see cref="ITimer"/> items from the <see cref="TimerList"/>.
        /// </summary>
        public void Clear()
        {
            if (m_timerCollection != null)
            {
                m_timerCollection.Clear();
            }
        }

        /// <summary>
        /// Determines whether the <see cref="TimerList"/>. contains a specific <see cref="ITimer"/> object.
        /// </summary>
        /// <param name="item">The <see cref="ITimer"/> object to locate in the <see cref="TimerList"/>.</param>
        /// <returns><c>true</c> if <see cref="ITimer"/> item is found in the <see cref="TimerList"/>; otherwise, <c>false</c>.</returns>
        public bool Contains(ITimer item)
        {
            if (m_timerCollection != null && item != null)
            {
                return m_timerCollection.Contains(item);
            }

            return false;
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="TimerList"/>.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="TimerList"/>.</param>
        /// <returns>
        /// <c>true</c> if item was successfully removed from the <see cref="TimerList"/>; otherwise, <c>false</c>. This method also returns
        /// <c>false</c> if item is not found in the original <see cref="TimerList"/>.
        /// </returns>
        public bool Remove(ITimer item)
        {
            if (m_timerCollection != null && item != null)
            {
                return m_timerCollection.Remove(item);
            }

            return false;
        }

        /// <summary>
        /// Performs the specified action on each <see cref="ITimer"/> element of the <see cref="TimerList"/>.
        /// </summary>
        /// <param name="action">The <see cref="Action{ITimer}"/> delegate to perform on each <see cref="ITimer"/> element of the <see cref="TimerList"/>.</param>
        public void ForEach(Action<ITimer> action)
        {
            for (int i = 0, length = m_timerCollection.Count; i < length; ++i)
            {
                ITimer timer = m_timerCollection[i];

                if (action != null && timer != null)
                {
                    action.Invoke(timer);
                }
            }
        }

        /// <summary>
        /// Sets all timers in the <see cref="TimerList"/> to be enabled or not.
        /// </summary>
        /// <param name="value">
        /// Set to <c>true</c> to enable all timers in the <see cref="TimerList"/> control to trigger their timer event; otherwise, set to <c>false</c>.
        /// </param>
        public void SetAllEnabled(bool value = true)
        {
            ForEach((timer) =>
            {
                timer.enabled = value;
            });
        }

        /// <summary>
        /// Starts all timers in the <see cref="TimerList"/>.
        /// </summary>
        public void StartAll()
        {
            ForEach((timer) =>
            {
                timer.Start();
            });

            if (m_timerCollection != null)
            {
                DispatchEvent(new TimerListEvent(TimerListEvent.AllStart, this));
            }
        }

        /// <summary>
        /// Pauses all timers in the <see cref="TimerList"/>.
        /// </summary>
        public void PauseAll()
        {
            ForEach((timer) =>
            {
                timer.Pause();
            });

            if (m_timerCollection != null)
            {
                DispatchEvent(new TimerListEvent(TimerListEvent.AllPause, this));
            }
        }

        /// <summary>
        /// Resumes all timers in <see cref="TimerList"/>.
        /// </summary>
        public void ResumeAll()
        {
            ForEach((timer) =>
            {
                timer.Resume();
            });

            if (m_timerCollection != null)
            {
                DispatchEvent(new TimerListEvent(TimerListEvent.AllResume, this));
            }
        }

        /// <summary>
        /// Stops all timers in the <see cref="TimerList"/>.
        /// </summary>
        public void StopAll()
        {
            ForEach((timer) =>
            {
                timer.Stop();
            });

            if (m_timerCollection != null)
            {
                DispatchEvent(new TimerListEvent(TimerListEvent.AllStop, this));
            }
        }

        /// <summary>
        /// Resets all timers in the <see cref="TimerList"/>.
        /// </summary>
        public void ResetAll()
        {
            ForEach((timer) =>
            {
                timer.Reset();
            });

            if (m_timerCollection != null)
            {
                DispatchEvent(new TimerListEvent(TimerListEvent.AllReset, this));
            }
        }

        #endregion ITimerList
    }
}
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

using CSharpExtensions.Collections.Generic;
using CSharpExtensions.Events;
using System;
using System.Collections.Generic;

namespace QuickUnity.Events
{
    /// <summary>
    /// The ThreadEventDispatcher class is the class for all classes that are working in child thread and dispatch events to Unity main thread.
    /// </summary>
    /// <seealso cref="QuickUnity.Events.IThreadEventDispatcher"/>
    public class ThreadEventDispatcher : IThreadEventDispatcher
    {
        private Dictionary<string, List<Action<Event>>> m_listeners = null;
        private Dictionary<string, List<Action<Event>>> m_pendingListeners = null;
        private Dictionary<string, List<Action<Event>>> m_pendingRemovedListeners = null;

        private List<Event> m_events = null;
        private List<Event> m_pendingEvents = null;

        private bool m_pendingFlag = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadEventDispatcher"/> class.
        /// </summary>
        public ThreadEventDispatcher()
        {
            m_listeners = new Dictionary<string, List<Action<Event>>>();
            m_pendingListeners = new Dictionary<string, List<Action<Event>>>();
            m_pendingRemovedListeners = new Dictionary<string, List<Action<Event>>>();

            m_events = new List<Event>();
            m_pendingEvents = new List<Event>();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="ThreadEventDispatcher"/> class.
        /// </summary>
        ~ThreadEventDispatcher()
        {
            m_listeners.Clear();
            m_listeners = null;

            m_pendingListeners.Clear();
            m_pendingListeners = null;

            m_pendingRemovedListeners.Clear();
            m_pendingRemovedListeners = null;

            m_events.Clear();
            m_events = null;

            m_pendingEvents.Clear();
            m_pendingEvents = null;
        }

        #region IThreadEventDispatcher Interface

        /// <summary>
        /// Update is called every frame.
        /// </summary>
        public virtual void Update()
        {
            lock (this)
            {
                bool addedFlag = AddPendingListeners();
                RemovePendingListeners();

                if (!addedFlag)
                {
                    m_pendingFlag = true;

                    // Dispatch events.
                    if (m_events != null && m_events.Count != 0)
                    {
                        m_events.ForEach(eventObject =>
                        {
                            if (eventObject != null && m_listeners.ContainsKey(eventObject.EventType))
                            {
                                List<Action<Event>> listeners = m_listeners[eventObject.EventType];

                                listeners.ForEach(listener =>
                                {
                                    if (listener != null)
                                    {
                                        listener.Invoke(eventObject);
                                    }
                                });
                            }
                        });

                        m_events.Clear();
                    }
                }
            }

            m_pendingFlag = false;
        }

        /// <summary>
        /// Registers an event listener object with an EventDispatcher object so that the listener receives notification of an event.
        /// </summary>
        /// <param name="eventType">The type of event.</param>
        /// <param name="listener">The listener function that processes the event.</param>
        public void AddEventListener(string eventType, Action<Event> listener)
        {
            lock (this)
            {
                // Add to pending listeners dictionary.
                if (m_pendingFlag)
                {
                    if (!m_pendingListeners.ContainsKey(eventType))
                        m_pendingListeners.Add(eventType, new List<Action<Event>>());

                    if (!m_pendingListeners[eventType].Contains(listener))
                        m_pendingListeners[eventType].Add(listener);

                    return;
                }

                // Add to listeners dictionary.
                if (!m_listeners.ContainsKey(eventType))
                    m_listeners.Add(eventType, new List<Action<Event>>());

                if (!m_listeners[eventType].Contains(listener))
                    m_listeners[eventType].Add(listener);
            }
        }

        /// <summary>
        /// Dispatches the event.
        /// </summary>
        /// <param name="eventObject">The event object.</param>
        public void DispatchEvent(Event eventObject)
        {
            lock (this)
            {
                if (!m_listeners.ContainsKey(eventObject.EventType))
                    return;

                // Add to pending events list.
                if (m_pendingFlag)
                {
                    if (!m_pendingEvents.Contains(eventObject))
                        m_pendingEvents.Add(eventObject);

                    return;
                }

                m_events = new List<Event>(m_pendingEvents.ToArray());
                m_events.Add(eventObject);
                m_pendingEvents.Clear();
            }
        }

        /// <summary>
        /// Checks whether the EventDispatcher object has any listeners registered for a specific type of event.
        /// </summary>
        /// <param name="eventType">The type of event.</param>
        /// <param name="listener">The listener function that processes the event.</param>
        /// <returns>A value of <c>true</c> if a listener of the specified type is registered; <c>false</c> otherwise.</returns>
        public bool HasEventListener(string eventType, Action<Event> listener)
        {
            return m_listeners.ContainsKey(eventType) && m_listeners[eventType].Contains(listener);
        }

        /// <summary>
        /// Removes a listener from the EventDispatcher object.
        /// </summary>
        /// <param name="eventType">The type of event.</param>
        /// <param name="listener">The listener object to remove.</param>
        public void RemoveEventListener(string eventType, Action<Event> listener)
        {
            lock (this)
            {
                // Can not remove event listener when this is pending.
                if (m_pendingFlag)
                {
                    if (!m_pendingRemovedListeners.ContainsKey(eventType))
                        m_pendingRemovedListeners.Add(eventType, new List<Action<Event>>());

                    if (!m_pendingRemovedListeners[eventType].Contains(listener))
                        m_pendingRemovedListeners[eventType].Add(listener);

                    return;
                }

                // Remove listener from listeners dictionary.
                if (!m_listeners.ContainsKey(eventType))
                    m_listeners.Add(eventType, new List<Action<Event>>());

                if (m_listeners[eventType].Contains(listener))
                    m_listeners[eventType].Remove(listener);
            }
        }

        #endregion IThreadEventDispatcher Interface

        /// <summary>
        /// Adds the pending listeners.
        /// </summary>
        /// <returns><c>true</c> if add pending listeners successfully, <c>false</c> otherwise.</returns>
        private bool AddPendingListeners()
        {
            if (m_events != null && m_events.Count == 0)
            {
                foreach (string eventType in m_pendingListeners.Keys)
                {
                    List<Action<Event>> pendingListeners = m_pendingListeners[eventType];

                    if (pendingListeners != null)
                    {
                        pendingListeners.ForEach(listener =>
                        {
                            AddEventListener(eventType, listener);
                        });
                    }
                }

                m_pendingListeners.Clear();

                m_events.AddRangeUnique(m_pendingEvents);
                m_pendingEvents.Clear();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes the pending listeners.
        /// </summary>
        private void RemovePendingListeners()
        {
            foreach (string eventType in m_pendingRemovedListeners.Keys)
            {
                List<Action<Event>> pendingRemovedListeners = m_pendingRemovedListeners[eventType];

                if (pendingRemovedListeners != null)
                {
                    pendingRemovedListeners.ForEach(listener =>
                    {
                        RemoveEventListener(eventType, listener);
                    });
                }
            }

            m_pendingRemovedListeners.Clear();
        }
    }
}
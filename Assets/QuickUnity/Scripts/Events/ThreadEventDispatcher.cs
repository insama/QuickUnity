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
using System.Collections.Generic;

namespace QuickUnity.Events
{
    /// <summary>
    /// The ThreadEventDispatcher class is the class for all classes that are working in child thread
    /// and dispatch events to Unity main thread.
    /// </summary>
    /// <seealso cref="QuickUnity.Events.IThreadEventDispatcher"/>
    public class ThreadEventDispatcher : IThreadEventDispatcher
    {
        private Dictionary<string, List<Action<Event>>> listeners = null;
        private Dictionary<string, List<Action<Event>>> pendingListeners = null;
        private Dictionary<string, List<Action<Event>>> pendingRemovedListeners = null;

        private List<Event> events = null;
        private List<Event> pendingEvents = null;

        private bool pendingFlag = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadEventDispatcher"/> class.
        /// </summary>
        public ThreadEventDispatcher()
        {
            listeners = new Dictionary<string, List<Action<Event>>>();
            pendingListeners = new Dictionary<string, List<Action<Event>>>();
            pendingRemovedListeners = new Dictionary<string, List<Action<Event>>>();

            events = new List<Event>();
            pendingEvents = new List<Event>();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="ThreadEventDispatcher"/> class.
        /// </summary>
        ~ThreadEventDispatcher()
        {
            listeners.Clear();
            listeners = null;

            pendingListeners.Clear();
            pendingListeners = null;

            pendingRemovedListeners.Clear();
            pendingRemovedListeners = null;

            events.Clear();
            events = null;

            pendingEvents.Clear();
            pendingEvents = null;
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
                    pendingFlag = true;

                    // Dispatch events.
                    if (events != null && events.Count != 0)
                    {
                        events.ForEach(eventObject =>
                        {
                            if (eventObject != null && listeners.ContainsKey(eventObject.EventType))
                            {
                                List<Action<Event>> eventListeners = listeners[eventObject.EventType];

                                eventListeners.ForEach(listener =>
                                {
                                    if (listener != null)
                                    {
                                        listener.Invoke(eventObject);
                                    }
                                });
                            }
                        });

                        events.Clear();
                    }
                }
            }

            pendingFlag = false;
        }

        /// <summary>
        /// Registers an event listener object with an EventDispatcher object so that the listener
        /// receives notification of an event.
        /// </summary>
        /// <param name="eventType">The type of event.</param>
        /// <param name="listener">The listener function that processes the event.</param>
        public void AddEventListener(string eventType, Action<Event> listener)
        {
            lock (this)
            {
                // Add to pending listeners dictionary.
                if (pendingFlag)
                {
                    pendingListeners.AddUnique(eventType, new List<Action<Event>>());
                    pendingListeners[eventType].AddUnique(listener);
                    return;
                }

                // Add to listeners dictionary.
                listeners.AddUnique(eventType, new List<Action<Event>>());
                listeners[eventType].AddUnique(listener);
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
                if (!listeners.ContainsKey(eventObject.EventType))
                    return;

                // Add to pending events list.
                if (pendingFlag)
                {
                    pendingEvents.Add(eventObject);
                    return;
                }

                events.AddRange(pendingEvents.ToArray());
                events.Add(eventObject);
                pendingEvents.Clear();
            }
        }

        /// <summary>
        /// Checks whether the EventDispatcher object has any listeners registered for a specific
        /// type of event.
        /// </summary>
        /// <param name="eventType">The type of event.</param>
        /// <param name="listener">The listener function that processes the event.</param>
        /// <returns>
        /// A value of <c>true</c> if a listener of the specified type is registered; <c>false</c> otherwise.
        /// </returns>
        public bool HasEventListener(string eventType, Action<Event> listener)
        {
            return listeners.ContainsKey(eventType) && listeners[eventType].Contains(listener);
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
                if (pendingFlag)
                {
                    pendingRemovedListeners.AddUnique(eventType, new List<Action<Event>>());
                    pendingRemovedListeners[eventType].AddUnique(listener);
                    return;
                }

                // Remove listener from listeners dictionary.
                listeners.AddUnique(eventType, new List<Action<Event>>());
                listeners[eventType].Remove(listener);
            }
        }

        #endregion IThreadEventDispatcher Interface

        /// <summary>
        /// Adds the pending listeners.
        /// </summary>
        /// <returns><c>true</c> if add pending listeners successfully, <c>false</c> otherwise.</returns>
        private bool AddPendingListeners()
        {
            if (events != null && events.Count == 0)
            {
                foreach (string eventType in pendingListeners.Keys)
                {
                    List<Action<Event>> pendingEventListeners = pendingListeners[eventType];

                    if (pendingEventListeners != null)
                    {
                        pendingEventListeners.ForEach(listener =>
                        {
                            AddEventListener(eventType, listener);
                        });
                    }
                }

                pendingListeners.Clear();

                events.AddRange(pendingEvents);
                pendingEvents.Clear();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes the pending listeners.
        /// </summary>
        private void RemovePendingListeners()
        {
            foreach (string eventType in pendingRemovedListeners.Keys)
            {
                List<Action<Event>> pendingRemovedEventListeners = pendingRemovedListeners[eventType];

                if (pendingRemovedEventListeners != null)
                {
                    pendingRemovedEventListeners.ForEach(listener =>
                    {
                        RemoveEventListener(eventType, listener);
                    });
                }
            }

            pendingRemovedListeners.Clear();
        }
    }
}
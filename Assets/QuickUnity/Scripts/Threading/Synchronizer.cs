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
using System.Collections;
using System.Collections.Generic;

namespace QuickUnity.Threading
{
    /// <summary>
    /// A <see cref="Synchronizer"/> representing a <see cref="UnityEngine.MonoBehaviour"/> to
    /// synchronize data between child threads and main threads.
    /// </summary>
    /// <seealso cref="SingletonMonoBehaviour{Synchronizer}"/>
    public class Synchronizer : SingletonMonoBehaviour<Synchronizer>, ICollection<ISynchronizedObject>
    {
        #region Fields

        private LinkedList<ISynchronizedObject> synchronizedObjects;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets the number of objects contained in the <see cref="Synchronizer"/>.
        /// </summary>
        /// <value>The number of objects contained in the <see cref="Synchronizer"/>.</value>
        public int Count
        {
            get
            {
                if (synchronizedObjects != null)
                {
                    return synchronizedObjects.Count;
                }

                return 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="Synchronizer"/> is read-only.
        /// </summary>
        /// <value><c>true</c> if the <see cref="Synchronizer"/> is read-only; otherwise, <c>false</c>.</value>
        bool ICollection<ISynchronizedObject>.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Adds an object of <see cref="ISynchronizedObject"/> to the <see cref="Synchronizer"/>.
        /// </summary>
        /// <param name="item">
        /// The object of <see cref="ISynchronizedObject"/> to add to the <see cref="Synchronizer"/>.
        /// </param>
        public void Add(ISynchronizedObject item)
        {
            if (synchronizedObjects != null)
            {
                synchronizedObjects.AddLast(item);
            }
        }

        /// <summary>
        /// Removes all items from the <see cref="Synchronizer"/>.
        /// </summary>
        public void Clear()
        {
            if (synchronizedObjects != null)
            {
                synchronizedObjects.Clear();
            }
        }

        /// <summary>
        /// Determines whether the <see cref="Synchronizer"/> contains a specific object of <see cref="ISynchronizedObject"/>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="Synchronizer"/>.</param>
        /// <returns>
        /// <c>true</c> if <c>item</c> is found in the <see cref="Synchronizer"/>; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(ISynchronizedObject item)
        {
            if (synchronizedObjects != null)
            {
                return synchronizedObjects.Contains(item);
            }

            return false;
        }

        /// <summary>
        /// Copies the objects of <see cref="ISynchronizedObject"/> to sychronize in the <see
        /// cref="Synchronizer"/> to an <see cref="System.Array"/>, starting at a particular <see
        /// cref="System.Array"/> index.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional <see cref="System.Array"/> that is the destination of the elements
        /// copied from <see cref="Synchronizer"/>. The <see cref="System.Array"/> must have
        /// zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">
        /// The zero-based index in <paramref name="array"/> at which copying begins.
        /// </param>
        public void CopyTo(ISynchronizedObject[] array, int arrayIndex)
        {
            if (synchronizedObjects != null)
            {
                synchronizedObjects.CopyTo(array, arrayIndex);
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<ISynchronizedObject> GetEnumerator()
        {
            if (synchronizedObjects != null)
            {
                return synchronizedObjects.GetEnumerator();
            }

            return null;
        }

        /// <summary>
        /// Removes the first occurrence of a specific object of <see cref="ISynchronizedObject"/>
        /// from the <see cref="Synchronizer"/>.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="Synchronizer"/>.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="item"/> was successfully removed from the <see
        /// cref="Synchronizer"/>; otherwise, <c>false</c>. This method also returns <c>false</c> if
        /// <paramref name="item"/> is not found in the original <see cref="Synchronizer"/>.
        /// </returns>
        public bool Remove(ISynchronizedObject item)
        {
            if (synchronizedObjects != null)
            {
                return synchronizedObjects.Remove(item);
            }

            return false;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Called when script receive message Awake.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            synchronizedObjects = new LinkedList<ISynchronizedObject>();
        }

        /// <summary>
        /// Called when script receive message Destroy.
        /// </summary>
        protected override void OnDestroy()
        {
            base.OnDestroy();

            synchronizedObjects = null;
        }

        /// <summary>
        /// Update is called every frame.
        /// </summary>
        private void Update()
        {
            foreach (ISynchronizedObject item in synchronizedObjects)
            {
                item.Synchronize();
            }
        }

        #endregion Methods
    }
}
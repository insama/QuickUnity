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

using UnityEngine;

namespace QuickUnity.Patterns.Singleton
{
    /// <summary>
    /// Abstract class for implementing singleton pattern for which is inherited from <see cref="MonoBehaviour"/>.
    /// </summary>
    /// <typeparam name="T">The type of the class.</typeparam>
    /// <seealso cref="MonoBehaviour"/>
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance = null;

        private static bool instantiated = false;
        private static bool destroyed = false;

        /// <summary>
        /// Gets or sets the singleton instance.
        /// </summary>
        /// <value>The singleton instance.</value>
        public static T Instance
        {
            get
            {
                if (destroyed)
                {
                    Debug.LogWarningFormat("SingletonMonoBehaviour<{0}> already destroyed. Return null.", typeof(T));
                    return null;
                }

                if (!instantiated || instance == null)
                {
                    T[] foundObjects = FindObjectsOfType<T>();

                    if (foundObjects.Length > 0)
                    {
                        // Found instances already have.
                        Instance = foundObjects[0];

                        if (foundObjects.Length > 1)
                        {
                            Debug.LogWarningFormat("There are more than one instance of MonoBehaviourSingleton of type \"{0}\". Keeping the first. Destroying the others.", typeof(T).ToString());

                            for (int i = 1, length = foundObjects.Length; i < length; ++i)
                            {
                                T behaviour = foundObjects[i];
                                Destroy(behaviour.gameObject);
                            }
                        }
                    }
                    else
                    {
                        // Make new one.
                        GameObject go = new GameObject();
                        go.name = typeof(T).Name;
                        Instance = go.AddComponent<T>();
                    }
                }

                return instance;
            }

            set
            {
                instance = value;
                instantiated = value != null;

                if (Application.isPlaying && value != null)
                {
                    DontDestroyOnLoad(instance.gameObject);
                }
            }
        }

        #region Messages

        /// <summary>
        /// Called when script receive message Awake.
        /// </summary>
        protected virtual void Awake()
        {
            // If singleton instance got null, find the instance already have.
            if (instance == null)
            {
                Instance = FindObjectOfType<T>();
            }
        }

        /// <summary>
        /// Called when script receive message Destroy.
        /// </summary>
        protected virtual void OnDestroy()
        {
            destroyed = true;
        }

        #endregion Messages
    }
}
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
using System;

namespace QuickUnity.Extensions
{
    /// <summary>
    /// Extension methods to the <see cref="UnityEngine.GameObject"/>.
    /// </summary>
    public static class GameObjectExtensions
    {
        /// <summary>
        /// Gets or adds the <see cref="UnityEngine.Component"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="UnityEngine.Component"/>.</typeparam>
        /// <param name="gameObject">The <see cref="UnityEngine.GameObject"/> need to get or add component.</param>
        /// <returns>The <see cref="UnityEngine.Component"/> get or added.</returns>
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            T component = default(T);

            component = gameObject.GetComponent<T>();

            if (component == null)
            {
                component = gameObject.AddComponent<T>();
            }

            return component;
        }

        /// <summary>
        /// Gets or adds the <see cref="UnityEngine.Component"/>.
        /// </summary>
        /// <param name="gameObject">The <see cref="UnityEngine.GameObject"/> need to get or add component.</param>
        /// <param name="type">The <see cref="System.Type"/> of the <see cref="UnityEngine.GameObject"/>.</param>
        /// <returns>The <see cref="UnityEngine.Component"/> get or added.</returns>
        public static Component GetOrAddComponent(this GameObject gameObject, Type type)
        {
            Component component = gameObject.GetComponent(type);

            if (component == null)
            {
                component = gameObject.AddComponent(type);
            }

            return component;
        }

        /// <summary>
        /// Removes the <see cref="UnityEngine.Component"/> from the <see cref="UnityEngine.GameObject"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="UnityEngine.Component"/>.</typeparam>
        /// <param name="gameObject">The <see cref="UnityEngine.GameObject"/> need to remove component.</param>
        /// <param name="immediate">if set to <c>true</c> remove the <see cref="UnityEngine.Component"/> immediately.</param>
        public static void RemoveComponent<T>(this GameObject gameObject, bool immediate = false) where T : Component
        {
            T component = gameObject.GetComponent<T>();

            if (component != null)
            {
                if (immediate)
                {
                    UnityEngine.Object.DestroyImmediate(component);
                }
                else
                {
                    UnityEngine.Object.Destroy(component);
                }
            }
        }

        /// <summary>
        /// Get all components.
        /// </summary>
        /// <param name="gameObject">The game object.</param>
        /// <returns>The array of all components.</returns>
        public static Component[] GetAllComponents(this GameObject gameObject)
        {
            return gameObject.GetComponents(typeof(Component));
        }

        /// <summary>
        /// Copies all components values.
        /// </summary>
        /// <param name="gameObject">The game object.</param>
        /// <param name="targetGameObject">The target game object.</param>
        /// <param name="exceptTransform">if set to <c>true</c> [except transform component].</param>
        public static void CopyAllComponentsValues(this GameObject gameObject, GameObject targetGameObject, bool exceptTransform = true)
        {
            if (targetGameObject)
            {
                Component[] components = gameObject.GetAllComponents();

                for (int i = 0, length = components.Length; i < length; ++i)
                {
                    Component component = components[i];

                    if (exceptTransform && component.GetType().FullName == "UnityEngine.Transform")
                    {
                        continue;
                    }

                    component.CopyComponentValues(targetGameObject);
                }
            }
        }

        /// <summary>
        /// Finds the game object in children.
        /// </summary>
        /// <param name="gameObject">The game object.</param>
        /// <param name="targetObjectName">Name of the target object.</param>
        /// <param name="includeInactive">if set to <c>true</c> [include inactive game object].</param>
        /// <returns>GameObject The game object you want to find.</returns>
        public static GameObject FindInChildren(this GameObject gameObject, string targetObjectName, bool includeInactive = true)
        {
            if (!string.IsNullOrEmpty(targetObjectName))
            {
                Transform transform = gameObject.transform;

                foreach (Transform childTransform in transform)
                {
                    if (!includeInactive && !childTransform.gameObject.activeSelf)
                    {
                        continue;
                    }

                    if (childTransform.name == targetObjectName)
                    {
                        return childTransform.gameObject;
                    }

                    GameObject targetGameObject = childTransform.gameObject.FindInChildren(targetObjectName, includeInactive);

                    if (targetGameObject != null)
                    {
                        return targetGameObject;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Sets the children layer.
        /// </summary>
        /// <param name="gameObject">The game object.</param>
        /// <param name="layer">The layer.</param>
        /// <param name="includeParent">if set to <c>true</c> [include parent game object].</param>
        /// <param name="includeInactive">if set to <c>true</c> [include inactive game object].</param>
        public static void SetChildrenLayer(this GameObject gameObject, int layer = -1, bool includeParent = true, bool includeInactive = true)
        {
            if (layer >= 0)
            {
                if (includeParent)
                {
                    if (!(!gameObject.activeSelf && !includeInactive))
                    {
                        gameObject.layer = layer;
                    }
                }

                Transform transform = gameObject.transform;

                foreach (Transform childTransform in transform)
                {
                    if (!includeInactive && !childTransform.gameObject.activeSelf)
                    {
                        continue;
                    }

                    childTransform.gameObject.layer = layer;
                    childTransform.gameObject.SetChildrenLayer(layer, includeParent, includeInactive);
                }
            }
        }

        /// <summary>
        /// Sets the children layer.
        /// </summary>
        /// <param name="gameObject">The game object.</param>
        /// <param name="layerName">Name of the layer.</param>
        /// <param name="includeParent">if set to <c>true</c> [include parent game object].</param>
        /// <param name="includeInactive">if set to <c>true</c> [include inactive game object].</param>
        public static void SetChildrenLayer(this GameObject gameObject, string layerName, bool includeParent = true, bool includeInactive = true)
        {
            if (!string.IsNullOrEmpty(layerName))
            {
                int layer = LayerMask.NameToLayer(layerName);
                gameObject.SetChildrenLayer(layer, includeParent, includeInactive);
            }
        }
    }
}
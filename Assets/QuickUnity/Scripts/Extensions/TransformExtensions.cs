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

namespace QuickUnity.Extensions
{
    /// <summary>
    /// Extension methods to the <see cref="Transform"/>.
    /// </summary>
    public static class TransformExtensions
    {
        /// <summary>
        /// Removes the component.
        /// </summary>
        /// <typeparam name="T">The type definition of the component.</typeparam>
        /// <param name="transform">The transform of GameObject.</param>
        /// <param name="immediate">if set to <c>true</c> [remove component immediately].</param>
        public static void RemoveComponent<T>(this Transform transform, bool immediate = false) where T : Component
        {
            T component = transform.GetComponent<T>();

            if (component != null)
            {
                if (immediate)
                {
                    Object.DestroyImmediate(component);
                }
                else
                {
                    Object.Destroy(component);
                }
            }
        }

        /// <summary>
        /// Get all components.
        /// </summary>
        /// <param name="transform">The transform of GameObject.</param>
        /// <returns>The array of all components.</returns>
        public static Component[] GetAllComponents(this Transform transform)
        {
            return transform.GetComponents(typeof(Component));
        }

        /// <summary>
        /// Find the transform in children.
        /// </summary>
        /// <param name="transform">The transform of game object.</param>
        /// <param name="targetTransformName">Name of the target transform.</param>
        /// <param name="includeInactive">if set to <c>true</c> [include inactive game object].</param>
        /// <returns>The transform of game object you want to find.</returns>
        public static Transform FindInChildren(this Transform transform, string targetTransformName, bool includeInactive = true)
        {
            if (!string.IsNullOrEmpty(targetTransformName))
            {
                foreach (Transform childTransform in transform)
                {
                    if (!includeInactive && !childTransform.gameObject.activeSelf)
                    {
                        continue;
                    }

                    if (childTransform.name == targetTransformName)
                    {
                        return childTransform;
                    }

                    Transform targetTransform = childTransform.FindInChildren(targetTransformName, includeInactive);

                    if (targetTransform != null)
                    {
                        return targetTransform;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Sets the children layer.
        /// </summary>
        /// <param name="transform">The transform of game object.</param>
        /// <param name="layer">The layer.</param>
        /// <param name="includeParent">if set to <c>true</c> [include parent game object].</param>
        /// <param name="includeInactive">if set to <c>true</c> [include inactive game object].</param>
        public static void SetChildrenLayer(this Transform transform, int layer = -1, bool includeParent = true, bool includeInactive = true)
        {
            if (layer >= 0)
            {
                if (includeParent)
                {
                    if (!(!transform.gameObject.activeSelf && !includeInactive))
                    {
                        transform.gameObject.layer = layer;
                    }
                }

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
        /// <param name="transform">The transform.</param>
        /// <param name="layerName">Name of the layer.</param>
        /// <param name="includeParent">if set to <c>true</c> [include parent game object].</param>
        /// <param name="includeInactive">if set to <c>true</c> [include inactive game object].</param>
        public static void SetChildrenLayer(this Transform transform, string layerName, bool includeParent = true, bool includeInactive = true)
        {
            if (!string.IsNullOrEmpty(layerName))
            {
                int layer = LayerMask.NameToLayer(layerName);
                transform.SetChildrenLayer(layer, includeParent, includeInactive);
            }
        }

        /// <summary>
        /// The reverse direction of blue axis of the transform in world space.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <returns>The backward direction.</returns>
        public static Vector3 GetBackwardDirection(this Transform transform)
        {
            return -transform.forward;
        }

        /// <summary>
        /// The reverse direction of red axis of the transform in world space.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <returns>The left direction.</returns>
        public static Vector3 GetLeftDirection(this Transform transform)
        {
            return -transform.right;
        }

        /// <summary>
        /// The reverse direction of green axis of the transform in world space.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <returns>The down direction.</returns>
        public static Vector3 GetDownDirection(this Transform transform)
        {
            return -transform.up;
        }

        #region Axis X

        /// <summary>
        /// Gets the position x.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <returns>The value of axis x in position.</returns>
        public static float GetPositionX(this Transform transform)
        {
            return transform.position.x;
        }

        /// <summary>
        /// Sets the position x.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="value">The value of axis x in position.</param>
        public static void SetPositionX(this Transform transform, float value)
        {
            if (transform.position.x != value)
            {
                transform.position = new Vector3(value, transform.position.y, transform.position.z);
            }
        }

        /// <summary>
        /// Gets the local position x.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <returns>The value of axis x in local position.</returns>
        public static float GetLocalPositionX(this Transform transform)
        {
            return transform.localPosition.x;
        }

        /// <summary>
        /// Sets the local position x.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="value">The value of axis x in local position.</param>
        public static void SetLocalPositionX(this Transform transform, float value)
        {
            if (transform.localPosition.x != value)
            {
                transform.localPosition = new Vector3(value, transform.localPosition.y, transform.localPosition.z);
            }
        }

        #endregion Axis X

        #region Axis Y

        /// <summary>
        /// Gets the position y.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <returns>The value of axis y in position.</returns>
        public static float GetPositionY(this Transform transform)
        {
            return transform.position.y;
        }

        /// <summary>
        /// Sets the position y.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="value">The value of axis y in position.</param>
        public static void SetPositionY(this Transform transform, float value)
        {
            if (transform.position.y != value)
            {
                transform.position = new Vector3(transform.position.x, value, transform.position.z);
            }
        }

        /// <summary>
        /// Gets the local position y.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <returns>The value of axis y in local position.</returns>
        public static float GetLocalPositionY(this Transform transform)
        {
            return transform.localPosition.y;
        }

        /// <summary>
        /// Sets the local position y.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="value">The value of axis y in local position.</param>
        public static void SetLocalPositionY(this Transform transform, float value)
        {
            if (transform.localPosition.y != value)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, value, transform.localPosition.z);
            }
        }

        #endregion Axis Y

        #region Axis Z

        /// <summary>
        /// Gets the position z.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <returns>The value of axis z in position.</returns>
        public static float GetPositionZ(this Transform transform)
        {
            return transform.position.z;
        }

        /// <summary>
        /// Sets the position z.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="value">The value of axis z in position.</param>
        public static void SetPositionZ(this Transform transform, float value)
        {
            if (transform.position.z != value)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, value);
            }
        }

        /// <summary>
        /// Gets the local position z.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <returns>The value of axis z in local position.</returns>
        public static float GetLocalPositionZ(this Transform transform)
        {
            return transform.localPosition.z;
        }

        /// <summary>
        /// Sets the local position z.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="value">The value of axis z in local position.</param>
        public static void SetLocalPositionZ(this Transform transform, float value)
        {
            if (transform.localPosition.z != value)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, value);
            }
        }

        #endregion Axis Z
    }
}
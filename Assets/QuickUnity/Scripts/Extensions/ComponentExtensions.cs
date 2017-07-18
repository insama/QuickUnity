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
using System.Reflection;
using UnityEngine;

namespace QuickUnity.Extensions
{
    /// <summary>
    /// Extension methods to the <see cref="UnityEngine.Component"/>.
    /// </summary>
    public static class ComponentExtensions
    {
        /// <summary>
        /// Copies values from the <see cref="UnityEngine.Component"/> to another <see cref="UnityEngine.Component"/> of the <see cref="UnityEngine.GameObject"/>.
        /// </summary>
        /// <param name="component">The <see cref="UnityEngine.Component"/>.</param>
        /// <param name="targetGameObject">The target <see cref="UnityEngine.GameObject"/>.</param>
        public static void CopyComponentValues(this Component component, GameObject targetGameObject)
        {
            if (targetGameObject)
            {
                Type type = component.GetType();
                Component copyComponent = targetGameObject.GetOrAddComponent(type);
                FieldInfo[] fields = type.GetFields();
                PropertyInfo[] properties = type.GetProperties();

                foreach (FieldInfo fieldInfo in fields)
                {
                    if (!fieldInfo.IsLiteral)
                        fieldInfo.SetValue(copyComponent, fieldInfo.GetValue(component));
                }

                foreach (PropertyInfo propertyInfo in properties)
                {
                    propertyInfo.SetValue(copyComponent, propertyInfo.GetValue(component, null), null);
                }
            }
        }
    }
}
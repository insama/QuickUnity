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

using CSharpExtensions.Reflection;
using QuickUnity.Attributes;
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace QuickUnityEditor.PropertyDrawers
{
    /// <summary>
    /// Property drawer for EnumFlagsAttribute.
    /// </summary>
    /// <seealso cref="UnityEditor.PropertyDrawer"/>
    [CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
    internal class EnumFlagsDrawer : PropertyDrawer
    {
        /// <summary>
        /// Override this method to make your own GUI for the property.
        /// </summary>
        /// <param name="position">Rectangle on the screen to use for the property GUI.</param>
        /// <param name="property">The SerializedProperty to make the custom GUI for.</param>
        /// <param name="label">The label of this property.</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EnumFlagsAttribute flagsAttribute = (EnumFlagsAttribute)attribute;
            Enum targetEnum = GetBaseProperty<Enum>(property);

            string propertyDisplayName = flagsAttribute.Label;

            if (string.IsNullOrEmpty(propertyDisplayName))
            {
                propertyDisplayName = property.name;
            }

            if (targetEnum != null)
            {
                EditorGUI.BeginProperty(position, label, property);
                Enum enumMask = EditorGUI.EnumMaskField(position, propertyDisplayName, targetEnum);
                property.intValue = (int)Convert.ChangeType(enumMask, targetEnum.GetType());
                EditorGUI.EndProperty();
            }
        }

        /// <summary>
        /// Gets the base property.
        /// </summary>
        /// <typeparam name="T">The Type definition of property.</typeparam>
        /// <param name="property">The property.</param>
        /// <returns>The property with Type definition.</returns>
        private static T GetBaseProperty<T>(SerializedProperty property)
        {
            // Separate the steps it takes to get to this property.
            string[] separatedPaths = property.propertyPath.Split('.');

            // Go down to the root of this serialized property.
            object reflectionTarget = property.serializedObject.targetObject as object;

            foreach (string path in separatedPaths)
            {
                reflectionTarget = ReflectionUtil.GetObjectFieldValue(reflectionTarget, path, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            }

            return (T)reflectionTarget;
        }
    }
}
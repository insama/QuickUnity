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

namespace QuickUnity
{
    /// <summary>
    /// ProjectAssemblies is class to get all assemblies of this project. This class cannot be inherited.
    /// </summary>
    public sealed class ProjectAssemblies
    {
        /// <summary>
        /// The assembly names.
        /// </summary>
        public static readonly string[] assemblyNames = new string[]
        {
            "Assembly-CSharp",
            "UnityEngine",

#if UNITY_EDITOR
            "Assembly-CSharp-Editor",
            "UnityEditor"
#endif
        };

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <returns>The Type object for the type name.</returns>
        public static Type GetType(string typeName)
        {
            Type result = null;

            if (!string.IsNullOrEmpty(typeName))
            {
                result = Type.GetType(typeName);

                if (result == null)
                {
                    for (int i = 0, length = assemblyNames.Length; i < length; ++i)
                    {
                        string assemblyName = assemblyNames[i];

                        if (!string.IsNullOrEmpty(assemblyName))
                        {
                            try
                            {
                                result = Assembly.Load(assemblyName).GetType(typeName);
                            }
                            catch (Exception exception)
                            {
                                Debug.LogException(exception);
                            }

                            if (result != null)
                            {
                                return result;
                            }
                        }
                    }
                }
            }

            return result;
        }
    }
}
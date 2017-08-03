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
using System;

namespace QuickUnity.Utils
{
    /// <summary>
    /// The utility class is about reflection functions. This class cannot be inherited.
    /// </summary>
    public sealed class UnityReflectionUtil
    {
        /// <summary>
        /// Creates the instance of class.
        /// </summary>
        /// <param name="typeFullName">Full name of the type.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>The instance of type.</returns>
        public static object CreateClassInstance(string typeFullName, object[] args = null)
        {
            if (!string.IsNullOrEmpty(typeFullName))
            {
                Type type = ProjectAssemblies.GetType(typeFullName);
                return ReflectionUtil.CreateClassInstance(type, args);
            }

            return null;
        }

        /// <summary>
        /// Creates the class instance.
        /// </summary>
        /// <typeparam name="T">The type definition of the instance returned.</typeparam>
        /// <param name="typeFullName">Full name of the type.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>The instance of type.</returns>
        public static T CreateClassInstance<T>(string typeFullName, object[] args = null)
        {
            if (!string.IsNullOrEmpty(typeFullName))
            {
                Type type = ProjectAssemblies.GetType(typeFullName);
                return ReflectionUtil.CreateClassInstance<T>(type, args);
            }

            return default(T);
        }

        /// <summary>
        /// Invokes the static method.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The object of method return.</returns>
        public static object InvokeStaticMethod(string typeName,
            string methodName,
            object[] parameters = null)
        {
            Type type = ProjectAssemblies.GetType(typeName);

            if (type != null)
            {
                return ReflectionUtil.InvokeStaticMethod(type, methodName, parameters);
            }

            return null;
        }

        /// <summary>
        /// Invokes the static method.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="types">The types.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The object of static method return.</returns>
        public static object InvokeStaticMethod(string typeName,
            string methodName,
            Type[] types,
            ref object[] parameters)
        {
            Type type = ProjectAssemblies.GetType(typeName);

            if (type != null)
            {
                return ReflectionUtil.InvokeStaticMethod(type, methodName, types, ref parameters);
            }

            return null;
        }

        /// <summary>
        /// Invokes the static generic method.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="genericType">Type of the generic.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The object of method return.</returns>
        public static object InvokeStaticGenericMethod(string typeName,
            string methodName,
            Type genericType,
            object[] parameters = null)
        {
            Type type = ProjectAssemblies.GetType(typeName);

            if (type != null)
            {
                return ReflectionUtil.InvokeStaticGenericMethod(type, methodName, genericType, parameters);
            }

            return null;
        }
    }
}
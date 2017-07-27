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
using QuickUnity.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace QuickUnityEditor.Data.Parsers
{
    /// <summary>
    /// The parser of Type System.Boolean. 
    /// </summary>
    /// <seealso cref="QuickUnityEditor.Data.Parsers.TypeParser{System.Boolean}"/>
    internal class BoolTypeParser : TypeParser<bool>
    {
        /// <summary>
        /// The Type keyword. 
        /// </summary>
        public const string TypeKeyword = "bool";
    }

    /// <summary>
    /// The parser of Type System.Byte. 
    /// </summary>
    /// <seealso cref="QuickUnityEditor.Data.Parsers.TypeParser{System.Byte}"/>
    internal class ByteTypeParser : TypeParser<byte>
    {
        /// <summary>
        /// The Type keyword. 
        /// </summary>
        public const string TypeKeyword = "byte";
    }

    /// <summary>
    /// The parser of Type System.SByte. 
    /// </summary>
    /// <seealso cref="QuickUnityEditor.Data.Parsers.TypeParser{System.SByte}"/>
    internal class SByteTypeParser : TypeParser<sbyte>
    {
        /// <summary>
        /// The Type keyword. 
        /// </summary>
        public const string TypeKeyword = "sbyte";
    }

    /// <summary>
    /// The parser of Type System.Decimal. 
    /// </summary>
    /// <seealso cref="QuickUnityEditor.Data.Parsers.TypeParser{System.Decimal}"/>
    internal class DecimalTypeParser : TypeParser<decimal>
    {
        /// <summary>
        /// The Type keyword. 
        /// </summary>
        public const string TypeKeyword = "decimal";
    }

    /// <summary>
    /// The parser of Type System.Double. 
    /// </summary>
    /// <seealso cref="QuickUnityEditor.Data.Parsers.TypeParser{System.Byte}"/>
    internal class DoubleTypeParser : TypeParser<double>
    {
        /// <summary>
        /// The Type keyword. 
        /// </summary>
        public const string TypeKeyword = "double";
    }

    /// <summary>
    /// The parser of Type System.Single. 
    /// </summary>
    /// <seealso cref="QuickUnityEditor.Data.Parsers.TypeParser{System.Single}"/>
    internal class FloatTypeParser : TypeParser<float>
    {
        /// <summary>
        /// The Type keyword. 
        /// </summary>
        public const string TypeKeyword = "float";
    }

    /// <summary>
    /// The parser of Type System.Int32. 
    /// </summary>
    /// <seealso cref="QuickUnityEditor.Data.Parsers.TypeParser{System.Int32}"/>
    internal class IntTypeParser : TypeParser<int>
    {
        /// <summary>
        /// The Type keyword. 
        /// </summary>
        public const string TypeKeyword = "int";
    }

    /// <summary>
    /// The parser of Type System.UInt32. 
    /// </summary>
    /// <seealso cref="QuickUnityEditor.Data.Parsers.TypeParser{System.UInt32}"/>
    internal class UIntTypeParser : TypeParser<uint>
    {
        /// <summary>
        /// The Type keyword. 
        /// </summary>
        public const string TypeKeyword = "uint";
    }

    /// <summary>
    /// The parser of Type System.Int64. 
    /// </summary>
    /// <seealso cref="QuickUnityEditor.Data.Parsers.TypeParser{System.Int64}"/>
    internal class LongTypeParser : TypeParser<long>
    {
        /// <summary>
        /// The Type keyword. 
        /// </summary>
        public const string TypeKeyword = "long";
    }

    /// <summary>
    /// The parser of Type System.UInt64. 
    /// </summary>
    /// <seealso cref="QuickUnityEditor.Data.Parsers.TypeParser{System.UInt64}"/>
    internal class ULongTypeParser : TypeParser<ulong>
    {
        /// <summary>
        /// The Type keyword. 
        /// </summary>
        public const string TypeKeyword = "ulong";
    }

    /// <summary>
    /// The parser of Type System.Int16. 
    /// </summary>
    /// <seealso cref="QuickUnityEditor.Data.Parsers.TypeParser{System.Int16}"/>
    internal class ShortTypeParser : TypeParser<short>
    {
        /// <summary>
        /// The Type keyword. 
        /// </summary>
        public const string TypeKeyword = "short";
    }

    /// <summary>
    /// The parser of Type System.UInt16. 
    /// </summary>
    /// <seealso cref="QuickUnityEditor.Data.Parsers.TypeParser{System.UInt16}"/>
    internal class UShortTypeParser : TypeParser<ushort>
    {
        /// <summary>
        /// The Type keyword. 
        /// </summary>
        public const string TypeKeyword = "ushort";
    }

    /// <summary>
    /// The parser of Type System.String. 
    /// </summary>
    /// <seealso cref="QuickUnityEditor.Data.Parsers.TypeParser{System.String}"/>
    internal class StringTypeParser : TypeParser<string>
    {
        /// <summary>
        /// The Type keyword. 
        /// </summary>
        public const string TypeKeyword = "string";
    }

    /// <summary>
    /// The data Type parser abstract class. 
    /// </summary>
    /// <typeparam name="T"> The Type definition. </typeparam>
    /// <seealso cref="QuickUnityEditor.Data.Parsers.ITypeParser{T}"/>
    internal abstract class TypeParser<T> : ITypeParser
    {
        /// <summary>
        /// The separator for array element. 
        /// </summary>
        public const string ArrayElementSeparator = "|";

        #region ITypeParser Interface

        /// <summary>
        /// Parses the specified value. 
        /// </summary>
        /// <param name="value"> The value. </param>
        /// <returns> The parsed data. </returns>
        public virtual object Parse(string value)
        {
            return Parse<T>(value);
        }

        #endregion ITypeParser Interface

        #region Protected Methods

        /// <summary>
        /// Parses the specified value. 
        /// </summary>
        /// <typeparam name="V"> The Type definition of returned object. </typeparam>
        /// <param name="value"> The value. </param>
        /// <returns> The parsed data. </returns>
        protected V Parse<V>(string value)
        {
            Type type = typeof(V);
            object result = null;

            if (type == typeof(bool))
            {
                result = ParseBoolString(value);
            }
            else if (type == typeof(string))
            {
                result = ParseString(value);
            }
            else
            {
                result = ParseNumber<V>(value);
            }

            return (V)Convert.ChangeType(result, type);
        }

        /// <summary>
        /// Parses the string object. 
        /// </summary>
        /// <param name="value"> The value. </param>
        /// <returns> The string value. </returns>
        protected string ParseString(string value)
        {
            value = value.Trim();

            if (string.IsNullOrEmpty(value))
                return null;

            return value;
        }

        /// <summary>
        /// Parses the bool string. 
        /// </summary>
        /// <param name="value"> The value. </param>
        /// <returns> The parsed bool data. </returns>
        protected bool ParseBoolString(string value)
        {
            bool result = false;

            if (!string.IsNullOrEmpty(value))
            {
                TextInfo textInfo = Thread.CurrentThread.CurrentCulture.TextInfo;
                value = textInfo.ToTitleCase(value.Trim());
                bool.TryParse(value, out result);
            }

            return result;
        }

        /// <summary>
        /// Parses the number value. 
        /// </summary>
        /// <typeparam name="T"> The Type definition of the number. </typeparam>
        /// <param name="value"> The value. </param>
        /// <returns> The parsed number data. </returns>
        protected U ParseNumber<U>(string value)
        {
            U result = default(U);
            Type targetType = typeof(U);
            value = value.Trim();
            object[] args = new object[4] { value, NumberStyles.Any, CultureInfo.InvariantCulture, result };

            if (targetType != null && !string.IsNullOrEmpty(value))
            {
                ReflectionUtil.InvokeStaticMethod(targetType, "TryParse", new Type[4] {
                    typeof(string), typeof(NumberStyles), typeof(CultureInfo), targetType.MakeByRefType()
                }, ref args);
            }

            return (U)args[3];
        }

        /// <summary>
        /// Parses the array string. 
        /// </summary>
        /// <typeparam name="W"> The Type definition of array element. </typeparam>
        /// <param name="value"> The string value. </param>
        /// <returns> The parsed array data. </returns>
        protected W[] ParseArrayString<W>(string value)
        {
            List<W> list = new List<W>();
            string[] valueStrArr = value.Split(ArrayElementSeparator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            if (valueStrArr != null && valueStrArr.Length > 0)
            {
                foreach (string valueStr in valueStrArr)
                {
                    W obj = Parse<W>(valueStr);
                    list.Add(obj);
                }
            }

            return list.ToArray();
        }

        #endregion Protected Methods
    }
}
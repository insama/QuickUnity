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
using System.Collections.Generic;

namespace QuickUnityEditor.Data.Parsers
{
    /// <summary>
    /// The factory class of Type parser.
    /// </summary>
    internal static class TypeParserFactory
    {
        /// <summary>
        /// The map of Type parsers.
        /// </summary>
        private static readonly Dictionary<string, Type> typeParsersMap = new Dictionary<string, Type>()
        {
            { BoolTypeParser.TypeKeyword, typeof(BoolTypeParser) },
            { ByteTypeParser.TypeKeyword, typeof(ByteTypeParser) },
            { SByteTypeParser.TypeKeyword, typeof(SByteTypeParser) },
            { DecimalTypeParser.TypeKeyword, typeof(DecimalTypeParser) },
            { DoubleTypeParser.TypeKeyword, typeof(DoubleTypeParser) },
            { FloatTypeParser.TypeKeyword, typeof(FloatTypeParser) },
            { IntTypeParser.TypeKeyword, typeof(IntTypeParser) },
            { UIntTypeParser.TypeKeyword, typeof(UIntTypeParser) },
            { LongTypeParser.TypeKeyword, typeof(LongTypeParser) },
            { ULongTypeParser.TypeKeyword, typeof(ULongTypeParser) },
            { ShortTypeParser.TypeKeyword, typeof(ShortTypeParser) },
            { UShortTypeParser.TypeKeyword, typeof(UShortTypeParser) },
            { StringTypeParser.TypeKeyword, typeof(StringTypeParser) },
            { BoolArrayTypeParser.TypeKeyword, typeof(BoolArrayTypeParser) },
            { ByteArrayTypeParser.TypeKeyword, typeof(ByteArrayTypeParser) },
            { SByteArrayTypeParser.TypeKeyword, typeof(SByteArrayTypeParser) },
            { DecimalArrayTypeParser.TypeKeyword, typeof(DecimalArrayTypeParser) },
            { DoubleArrayTypeParser.TypeKeyword, typeof(DoubleArrayTypeParser) },
            { FloatArrayTypeParser.TypeKeyword, typeof(FloatArrayTypeParser) },
            { IntArrayTypeParser.TypeKeyword, typeof(IntArrayTypeParser) },
            { UIntArrayTypeParser.TypeKeyword, typeof(UIntArrayTypeParser) },
            { LongArrayTypeParser.TypeKeyword, typeof(LongArrayTypeParser) },
            { ULongArrayTypeParser.TypeKeyword, typeof(ULongArrayTypeParser) },
            { ShortArrayTypeParser.TypeKeyword, typeof(ShortArrayTypeParser) },
            { UShortArrayTypeParser.TypeKeyword, typeof(UShortArrayTypeParser) },
            { StringArrayTypeParser.TypeKeyword, typeof(StringArrayTypeParser) }
        };

        /// <summary>
        /// Gets the Type of the Type parser.
        /// </summary>
        /// <param name="typeKeyword">The Type keyword.</param>
        /// <returns>Type The Type of Type parser.</returns>
        public static Type GetTypeParserType(string typeKeyword)
        {
            if (!string.IsNullOrEmpty(typeKeyword) && typeParsersMap.ContainsKey(typeKeyword))
            {
                return typeParsersMap[typeKeyword];
            }

            return null;
        }

        /// <summary>
        /// Creates the Type parser.
        /// </summary>
        /// <param name="typeKeyword">The Type keyword.</param>
        /// <returns>The Type parser.</returns>
        public static ITypeParser CreateTypeParser(string typeKeyword)
        {
            Type type = GetTypeParserType(typeKeyword);

            if (type != null)
            {
                return ReflectionUtil.CreateClassInstance<ITypeParser>(type);
            }

            return null;
        }
    }
}
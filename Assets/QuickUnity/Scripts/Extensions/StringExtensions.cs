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

namespace System
{
    /// <summary>
    /// Extension methods to the <see cref="string"/>.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Returns Vector2 for this formatted string (format like this: (0.0, 1.0)).
        /// </summary>
        /// <param name="source">The source formatted string.</param>
        /// <returns>The Vector2 value.</returns>
        public static Vector2 ToVector2(this string source)
        {
            if (!string.IsNullOrEmpty(source))
            {
                string[] valuesArr = GetValuesArray(source);

                if (valuesArr != null && valuesArr.Length == 2)
                {
                    float x = 0;
                    float y = 0;
                    float.TryParse(valuesArr[0], out x);
                    float.TryParse(valuesArr[1], out y);
                    return new Vector2(x, y);
                }
            }

            return Vector2.zero;
        }

        /// <summary>
        /// Returns Vector3 for this formatted string (format like this: (0.0, 1.0, 2.0)).
        /// </summary>
        /// <param name="source">The source formatted string.</param>
        /// <returns>The Vector3 value.</returns>
        public static Vector3 ToVector3(this string source)
        {
            if (!string.IsNullOrEmpty(source))
            {
                string[] valuesArr = GetValuesArray(source);

                if (valuesArr != null && valuesArr.Length == 3)
                {
                    float x = 0;
                    float y = 0;
                    float z = 0;
                    float.TryParse(valuesArr[0], out x);
                    float.TryParse(valuesArr[1], out y);
                    float.TryParse(valuesArr[2], out z);
                    return new Vector3(x, y, z);
                }
            }

            return Vector3.zero;
        }

        /// <summary>
        /// Returns Quaternion for this formatted string (format like this: (0.0, 1.0, 2.0. 0.0)).
        /// </summary>
        /// <param name="source">The source formatted string.</param>
        /// <returns>The Quaternion value.</returns>
        public static Quaternion ToQuaternion(this string source)
        {
            if (!string.IsNullOrEmpty(source))
            {
                string[] valuesArr = GetValuesArray(source);

                if (valuesArr != null && valuesArr.Length == 4)
                {
                    float x = 0;
                    float y = 0;
                    float z = 0;
                    float w = 0;
                    float.TryParse(valuesArr[0], out x);
                    float.TryParse(valuesArr[1], out y);
                    float.TryParse(valuesArr[2], out z);
                    float.TryParse(valuesArr[3], out w);
                    return new Quaternion(x, y, z, w);
                }
            }

            return Quaternion.identity;
        }

        /// <summary>
        /// Gets the values array.
        /// </summary>
        /// <param name="strValue">The string value.</param>
        /// <returns>The array of values.</returns>
        private static string[] GetValuesArray(string strValue)
        {
            if (strValue.IndexOf('(') == 0 && strValue.LastIndexOf(')') == strValue.Length - 1)
            {
                string realValuesStr = strValue.Trim('(', ')');
                return realValuesStr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            }

            return null;
        }
    }
}
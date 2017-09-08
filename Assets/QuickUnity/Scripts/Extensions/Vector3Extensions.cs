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

namespace UnityEngine
{
    /// <summary>
    /// Extension methods to the <see cref="Vector3"/>.
    /// </summary>
    public static class Vector3Extensions
    {
        /// <summary>
        /// If this Vector3 object strictly equals other Vector3 object.
        /// </summary>
        /// <param name="vector">The self Vector3 object.</param>
        /// <param name="other">The other Vector3 object.</param>
        /// <returns><c>true</c> if self equals other strictly, <c>false</c> otherwise.</returns>
        public static bool StrictlyEquals(this Vector3 vector, Vector3 other)
        {
            if (vector.StrictlyToString() == other.StrictlyToString())
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Return the strict string of this Vector3 object.
        /// </summary>
        /// <param name="vector">The self Vector3 object.</param>
        /// <param name="decimalDigits">The decimal digits.</param>
        /// <returns>The strict string of the Vector3 object.</returns>
        public static string StrictlyToString(this Vector3 vector, int decimalDigits = 4)
        {
            string format = string.Format("f{0}", decimalDigits.ToString());
            return string.Format("({0}, {1}, {2})", vector.x.ToString(format), vector.y.ToString(format), vector.z.ToString(format));
        }
    }
}
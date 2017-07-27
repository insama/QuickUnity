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
using System.Collections.Generic;
using System.Text;

namespace QuickUnity.Data
{
    /// <summary>
    /// Class DataTable.
    /// </summary>
    public abstract class DataTableRow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataTableRow"/> class.
        /// </summary>
        public DataTableRow()
        {
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder propsSb = new StringBuilder();
            Dictionary<string, object> map = ReflectionUtil.GetObjectProperties(this);

            foreach (KeyValuePair<string, object> kvp in map)
            {
                string output = string.Format("{0} = {1}, ", kvp.Key, kvp.Value);
                propsSb.Append(output);
            }

            sb.AppendFormat("{0}({1})", base.ToString(), propsSb.ToString(0, propsSb.Length - 2));
            return sb.ToString();
        }
    }
}
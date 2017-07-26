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
            string output = string.Empty;
            Dictionary<string, object> map = ReflectionUtil.GetObjectFields(this);

            foreach (KeyValuePair<string, object> kvp in map)
            {
                output += string.Format("{0}: {1}, ", kvp.Key, kvp.Value);
            }

            return base.ToString() + string.Format("({0})", output.Substring(0, output.Length - 2));
        }
    }

    /// <summary>
    /// The address map of data table.
    /// </summary>
    /// <seealso cref="QuickUnity.Data.DataTableRow"/>
    public class DataTableAddressMap : DataTableRow
    {
        /// <summary>
        /// The primary key.
        /// </summary>
        public const string PrimaryKey = "Type";

        /// <summary>
        /// The type.
        /// </summary>
        public string Type;

        /// <summary>
        /// The local address.
        /// </summary>
        public long LocalAddress;

        /// <summary>
        /// The field name of primary key.
        /// </summary>
        public string PrimaryFieldName;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataTableAddressMap"/> class.
        /// </summary>
        public DataTableAddressMap()
            : base()
        {
        }
    }
}
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

namespace QuickUnity.Data
{
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
        /// Gets or sets the string of <see cref="Type"/>.
        /// </summary>
        /// <value>The string of <see cref="Type"/>.</value>
        public string Type
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the local address of the <see cref="Type"/>.
        /// </summary>
        /// <value>The local address of the <see cref="Type"/>.</value>
        public long LocalAddress
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the primary property.
        /// </summary>
        /// <value>The name of the primary property.</value>
        public string PrimaryPropertyName
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataTableAddressMap"/> class.
        /// </summary>
        public DataTableAddressMap()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataTableAddressMap"/> class with the string of the <see cref="Type"/>, the local address and
        /// the name of the primary property.
        /// </summary>
        /// <param name="type">The string of the <see cref="Type"/>.</param>
        /// <param name="localAddress">The local address.</param>
        /// <param name="primaryPropertyName">Name of the primary property.</param>
        public DataTableAddressMap(string type, long localAddress, string primaryPropertyName)
            : base()
        {
        }
    }
}
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

using QuickUnity.Data;
using UnityEngine;

namespace QuickUnity.Data
{
    /// <summary>
    /// ScriptableObject class to save preferences of DataTable.
    /// </summary>
    /// <seealso cref="UnityEngine.ScriptableObject"/>
    public class DataTablePreferences : ScriptableObject
    {
        /// <summary>
        /// The default namespace string.
        /// </summary>
        public const string DefaultNamespace = "DefaultNamespace";

        /// <summary>
        /// The minimum row number of data rows start.
        /// </summary>
        public const int MinDataRowsStartRow = 4;

        /// <summary>
        /// The data tables storage location.
        /// </summary>
        public DataTableStorageLocation dataTablesStorageLocation = DataTableStorageLocation.PersistentDataPath;

        /// <summary>
        /// The data table row scripts storage location.
        /// </summary>
        public string dataTableRowScriptsStorageLocation;

        /// <summary>
        /// Whether to generate namespace automatically.
        /// </summary>
        public bool autoGenerateScriptsNamespace = true;

        /// <summary>
        /// The namespace of DataTableRow scripts.
        /// </summary>
        public string dataTableRowScriptsNamespace = "";

        /// <summary>
        /// The start row of data rows.
        /// </summary>
        public int dataRowsStartRow = MinDataRowsStartRow;
    }
}
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

namespace QuickUnity.Data
{
    /// <summary>
    /// The enumeration of data table storage location.
    /// </summary>
    public enum DataTableStorageLocation
    {
        ResourcesPath,
        StreamingAssetsPath,
        PersistentDataPath
    }

    /// <summary>
    /// ScriptableObject class to save preferences of DataTable.
    /// </summary>
    /// <seealso cref="UnityEngine.ScriptableObject"/>
    public class DataTablePreferences : ScriptableObject
    {
        #region Constants

        /// <summary>
        /// The default namespace string.
        /// </summary>
        public const string DefaultNamespace = "DefaultNamespace";

        /// <summary>
        /// The minimum row number of data rows start.
        /// </summary>
        public const int MinDataRowsStartRow = 4;

        #endregion Constants

        #region Fields

        private DataTableStorageLocation dataTablesStorageLocation = DataTableStorageLocation.PersistentDataPath;

        private bool autoGenerateScriptsNamespace = true;

        private string dataTableRowScriptsNamespace = string.Empty;

        private int dataRowsStartRow = MinDataRowsStartRow;

        #endregion Fields

        #region Properties

        /// <summary>
        /// The data table row scripts storage location.
        /// </summary>
        public string DataTableRowScriptsStorageLocation
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the data tables storage location.
        /// </summary>
        /// <value>The data tables storage location.</value>
        public DataTableStorageLocation DataTablesStorageLocation
        {
            get { return dataTablesStorageLocation; }
            set { dataTablesStorageLocation = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether automatic generate scripts namespace.
        /// </summary>
        /// <value><c>true</c> if automatic generate scripts namespace; otherwise, <c>false</c>.</value>
        public bool AutoGenerateScriptsNamespace
        {
            get { return autoGenerateScriptsNamespace; }
            set { autoGenerateScriptsNamespace = value; }
        }

        /// <summary>
        /// Gets or sets the namespace of the script <see cref="DataTableRow"/>.
        /// </summary>
        /// <value>The namespace of the script <see cref="DataTableRow"/>.</value>
        public string DataTableRowScriptsNamespace
        {
            get { return dataTableRowScriptsNamespace; }
            set { dataTableRowScriptsNamespace = value; }
        }

        /// <summary>
        /// Gets or sets the start row of data rows.
        /// </summary>
        /// <value>The start row of data rows.</value>
        public int DataRowsStartRow
        {
            get { return dataRowsStartRow; }
            set { dataRowsStartRow = value; }
        }

        #endregion Properties
    }
}
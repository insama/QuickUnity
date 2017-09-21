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

using System.Collections.Generic;
using UnityEngine;

namespace QuickUnity.Rendering.DataParsers
{
    internal abstract class TilingSheetDataParser : ITilingSheetDataParser
    {
        private static readonly Dictionary<TilingSheetDataFormat, ITilingSheetDataParser> parsers = new Dictionary<TilingSheetDataFormat, ITilingSheetDataParser>()
        {
            { TilingSheetDataFormat.UnityJson, new UnityJsonDataParser() }
        };

        private static Dictionary<string, Dictionary<string, Rect>> dataDict = new Dictionary<string, Dictionary<string, Rect>>();

        protected static Dictionary<string, Dictionary<string, Rect>> DataDict
        {
            get
            {
                return dataDict;
            }
        }

        public static ITilingSheetDataParser CreateParser(TilingSheetDataFormat dataFormat)
        {
            if (parsers.ContainsKey(dataFormat))
            {
                return parsers[dataFormat];
            }

            return null;
        }

        public abstract Dictionary<string, Rect> ParseData(string name, string data);
    }
}
/*
 *  The MIT License (MIT)
 *
 *  Copyright (c) 2017 Jerry Lee
 *
 *  Permission is hereby granted, free of charge, to any person obtaining a copy
 *  of this software and associated documentation files (the "Software"), to deal
 *  in the Software without restriction, including without limitation the rights
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *  copies of the Software, and to permit persons to whom the Software is
 *  furnished to do so, subject to the following conditions:
 *
 *  The above copyright notice and this permission notice shall be included in all
 *  copies or substantial portions of the Software.
 *
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 *  SOFTWARE.
 */

using CSharpExtensions.Net.Http;
using QuickUnity.Events;
using System;

namespace QuickUnity.Net.Http
{
    public class MonoHttpEvent : Event
    {
        #region Event Constants

        public const string DownloadInProgress = "DownloadInProgress";

        public const string DownloadCompleted = "DownloadCompleted";

        public const string ExceptionCaught = "ExceptionCaught";

        #endregion Event Constants

        #region Constructors

        public MonoHttpEvent(string eventType, MonoHttpClient httpClient, long bytesRead, long totalLength)
            : base(eventType, httpClient)
        {
            BytesRead = bytesRead;
            TotalLength = totalLength;
        }

        public MonoHttpEvent(string eventType, MonoHttpClient httpClient, HttpResponse response)
            : base(eventType, httpClient)
        {
            Response = response;
        }

        public MonoHttpEvent(string eventType, MonoHttpClient httpClient, Exception exceptionCaught)
            : base(eventType, httpClient)
        {
            Exception = exceptionCaught;
        }

        #endregion Constructors

        #region Properties

        public MonoHttpClient HttpClient
        {
            get { return (MonoHttpClient)Context; }
        }

        public long BytesRead
        {
            get;
            private set;
        }

        public long TotalLength
        {
            get;
            private set;
        }

        public HttpResponse Response
        {
            get;
            private set;
        }

        public Exception Exception
        {
            get;
            private set;
        }

        #endregion Properties
    }
}
using QuickUnity.Events;
using System;
using System.IO;
using UnityEngine;

namespace QuickUnity.Tests.IntegrationTests
{
    /// <summary>
    /// Text reader by thread.
    /// </summary>
    /// <seealso cref="QuickUnity.Events.ThreadEventDispatcher"/>
    public class ThreadTextReader : ThreadEventDispatcher
    {
        /// <summary>
        /// The image path.
        /// </summary>
        private static readonly string imagePath = Application.streamingAssetsPath + "/text.txt";

        /// <summary>
        /// The FileStream object.
        /// </summary>
        private FileStream fileStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadTextReader"/> class.
        /// </summary>
        public ThreadTextReader()
            : base()
        {
        }

        /// <summary>
        /// Begins to read image.
        /// </summary>
        public void BeginRead()
        {
            byte[] buffer = new byte[204800];
            fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
            fileStream.BeginRead(buffer, 0, buffer.Length, new System.AsyncCallback(AsyncReadCallback), this);
        }

        /// <summary>
        /// Read callback.
        /// </summary>
        /// <param name="asyncResult">The asynchronous result.</param>
        private void AsyncReadCallback(IAsyncResult asyncResult)
        {
            if (fileStream != null)
            {
                fileStream.EndRead(asyncResult);
                fileStream.Close();
                fileStream = null;
                DispatchEvent(new TestEvent(TestEvent.Complete));
            }
        }
    }
}
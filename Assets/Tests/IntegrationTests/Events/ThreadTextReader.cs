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
        private static readonly string k_imagePath = Application.streamingAssetsPath + "/text.txt";

        /// <summary>
        /// The FileStream object.
        /// </summary>
        private FileStream m_fileStream;

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
            m_fileStream = new FileStream(k_imagePath, FileMode.Open, FileAccess.Read);
            m_fileStream.BeginRead(buffer, 0, buffer.Length, new System.AsyncCallback(AsyncReadCallback), this);
        }

        /// <summary>
        /// Read callback.
        /// </summary>
        /// <param name="asyncResult">The asynchronous result.</param>
        private void AsyncReadCallback(IAsyncResult asyncResult)
        {
            if (m_fileStream != null)
            {
                m_fileStream.EndRead(asyncResult);
                m_fileStream.Close();
                m_fileStream = null;
                DispatchEvent(new TestEvent(TestEvent.Complete));
            }
        }
    }
}
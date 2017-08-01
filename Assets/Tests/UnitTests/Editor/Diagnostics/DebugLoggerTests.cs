using NUnit.Framework;
using System;

namespace QuickUnity.Diagnostics
{
    /// <summary>
    /// Unit test cases for class DebugLogger.
    /// </summary>
    [TestFixture]
    internal class DebugLoggerTests
    {
        /// <summary>
        /// Test case for method DebugLogger.Log.
        /// </summary>
        [Test]
        public void LogTest()
        {
            DebugLogger.Log("Test Log method!", this);
            Assert.Pass();
        }

        /// <summary>
        /// Test case for method DebugLogger.LogFormat.
        /// </summary>
        [Test]
        public void LogFormatTest()
        {
            DebugLogger.LogFormat(this, "Test LogFormat method: {0}", DateTime.Now);
            Assert.Pass();
        }

        /// <summary>
        /// Test case for method DebugLogger.LogWarning.
        /// </summary>
        [Test]
        public void LogWarningTest()
        {
            DebugLogger.LogWarning("Test LogWarning method!", this);
            Assert.Pass();
        }

        /// <summary>
        /// Test case for method DebugLogger.LogWarningFormat.
        /// </summary>
        [Test]
        public void LogWarningFormatTest()
        {
            DebugLogger.LogWarningFormat(this, "Test LogWarningFormat method: {0}", DateTime.Now);
            Assert.Pass();
        }

        /// <summary>
        /// Test case for method DebugLogger.LogError.
        /// </summary>
        [Test]
        public void LogErrorTest()
        {
            DebugLogger.LogError("Test LogError method!", this);
            Assert.Pass();
        }

        /// <summary>
        /// Test case for method DebugLogger.LogErrorFormat.
        /// </summary>
        [Test]
        public void LogErrorFormatTest()
        {
            DebugLogger.LogErrorFormat(this, "Test LogErrorFormat method: {0}", DateTime.Now);
            Assert.Pass();
        }

        /// <summary>
        /// Test case for method DebugLogger.LogException.
        /// </summary>
        [Test]
        public void LogExceptionTest()
        {
            DebugLogger.LogException(new Exception(), this);
            Assert.Pass();
        }
    }
}
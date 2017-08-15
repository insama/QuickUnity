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

using CSharpExtensions.IO;
using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace QuickUnity.Diagnostics
{
    /// <summary>
    /// Class containing methods to output log message while developing a game.
    /// </summary>
    public static class DebugLogger
    {
        /// <summary>
        /// The log files folder name.
        /// </summary>
        private const string logFilesFolderName = "Logs";

        /// <summary>
        /// The log file extension.
        /// </summary>
        private const string logFileExtension = ".log";

#if (UNITY_ANDROID || UNITY_IPHONE) && !UNITY_EDITOR

        /// <summary>
        /// The log files root path
        /// </summary>
        private static readonly string rootPath = Application.persistentDataPath;

#elif UNITY_EDITOR

        /// <summary>
        /// The log files root path
        /// </summary>
        private static readonly string rootPath = Directory.GetCurrentDirectory();

#else

        /// <summary>
        /// The log files root path
        /// </summary>
        private static readonly string rootPath = DirectoryUtil.GetRealCurrentDirectory();

#endif

        /// <summary>
        /// The log files path.
        /// </summary>
        private static readonly string logFilesPath = Path.Combine(rootPath, logFilesFolderName);

        /// <summary>
        /// The flag whether allow to write log messages into file.
        /// </summary>
        private static bool logFileEnabled = true;

        /// <summary>
        /// Whether allow to show log messages in Console window of Unity.
        /// </summary>
        private static bool showInConsole = true;

        /// <summary>
        /// Gets or sets a value indicating whether allow to write log messages into file.
        /// </summary>
        /// <value><c>true</c> if allow to write log messages into file; otherwise, <c>false</c>.</value>
        public static bool LogFileEnabled
        {
            get { return logFileEnabled; }
            set { logFileEnabled = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating Whether allow to show log messages in Console window of Unity.
        /// </summary>
        /// <value><c>true</c> if allow to show log messages in Console window of Unity; otherwise, <c>false</c>.</value>
        public static bool ShowInConsole
        {
            get { return showInConsole; }
            set { showInConsole = value; }
        }

        #region Public Static Functions

        /// <summary>
        /// Logs information message to the log system.
        /// </summary>
        /// <param name="message">String or object to be converted to string representation for display.</param>
        /// <param name="context">Object to which the message applies.</param>
        public static void Log(object message, object context = null)
        {
            if (message != null)
            {
                LogMessage(message, LogType.Log, context);
            }
        }

        /// <summary>
        /// Logs a formatted information message to the log system.
        /// </summary>
        /// <param name="context">Object to which the message applies.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">Format arguments.</param>
        public static void LogFormat(object context, string format, params object[] args)
        {
            if (!string.IsNullOrEmpty(format) && args != null)
            {
                string message = string.Format(format, args);
                Log(message, context);
            }
        }

        /// <summary>
        /// Logs warning message to the log system.
        /// </summary>
        /// <param name="message">String or object to be converted to string representation for display.</param>
        /// <param name="context">Object to which the message applies.</param>
        public static void LogWarning(object message, object context = null)
        {
            if (message != null)
            {
                LogMessage(message, LogType.Warning, context);
            }
        }

        /// <summary>
        /// Logs a formatted warning message to the log system.
        /// </summary>
        /// <param name="context">Object to which the message applies.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">Format arguments.</param>
        public static void LogWarningFormat(object context, string format, params object[] args)
        {
            if (!string.IsNullOrEmpty(format) && args != null)
            {
                string message = string.Format(format, args);
                LogWarning(message, context);
            }
        }

        /// <summary>
        /// Logs error message to the log system.
        /// </summary>
        /// <param name="message">String or object to be converted to string representation for display.</param>
        /// <param name="context">Object to which the message applies.</param>
        public static void LogError(object message, object context = null)
        {
            if (message != null)
            {
                LogMessage(message, LogType.Error, context);
            }
        }

        /// <summary>
        /// Logs a formatted error message to the log system.
        /// </summary>
        /// <param name="context">Object to which the message applies.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">Format arguments.</param>
        public static void LogErrorFormat(object context, string format, params object[] args)
        {
            if (!string.IsNullOrEmpty(format) && args != null)
            {
                string message = string.Format(format, args);
                LogError(message, context);
            }
        }

        /// <summary>
        /// Assert a condition and logs an error message to the log system.
        /// </summary>
        /// <param name="condition">Condition you expect to be true.</param>
        /// <param name="message">String or object to be converted to string representation for display.</param>
        /// <param name="context">Object to which the message applies.</param>
        public static void LogAssert(bool condition, object message, object context = null)
        {
            if (!condition && message != null)
            {
                LogMessage(message, LogType.Assert, context);
            }
        }

        /// <summary>
        /// Assert a condition and logs a formatted error message to the log system.
        /// </summary>
        /// <param name="condition">Condition you expect to be true.</param>
        /// <param name="context">A composite format string.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">Format arguments.</param>
        public static void LogAssertFormat(bool condition, object context, string format, params object[] args)
        {
            if (!string.IsNullOrEmpty(format) && args != null)
            {
                string message = string.Format(format, args);
                LogAssert(condition, message, context);
            }
        }

        /// <summary>
        /// A variant of DebugLogger.Log that logs an error message to log system.
        /// </summary>
        /// <param name="exception">Runtime Exception.</param>
        /// <param name="context">Object to which the message applies.</param>
        public static void LogException(Exception exception, object context = null)
        {
            if (exception != null)
            {
                string message = exception.ToString();
                LogMessage(message, LogType.Exception, context);
            }
        }

        #endregion Public Static Functions

        /// <summary>
        /// Gets the timestamp string.
        /// </summary>
        /// <returns>The timestamp string.</returns>
        private static string GetTimestampString()
        {
            return "[" + DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss,fff") + "]";
        }

        /// <summary>
        /// Gets the log type string.
        /// </summary>
        /// <param name="logType">Type of the log.</param>
        /// <returns>The log type string.</returns>
        private static string GetLogTypeString(LogType logType)
        {
            return "[" + logType + "]";
        }

        /// <summary>
        /// Gets the name of the context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The name of the context.</returns>
        private static string GetContextName(object context)
        {
            return context.GetType().Name + ": ";
        }

        /// <summary>
        /// Logs the message to log system.
        /// </summary>
        /// <param name="message">String or object to be converted to string representation for display.</param>
        /// <param name="logType">The type of the log message.</param>
        /// <param name="context">Object to which the message applies.</param>
        private static void LogMessage(object message, LogType logType, object context = null)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(GetTimestampString());
            builder.Append(GetLogTypeString(logType));

            if (context != null)
            {
                builder.Append(GetContextName(context));
            }

            if (!string.IsNullOrEmpty(message.ToString()))
            {
                builder.AppendLine(message.ToString());
            }
            else
            {
                builder.AppendLine();
            }

            string messageToShow = builder.ToString();

            if (showInConsole)
            {
                Debug.logger.Log(logType, messageToShow, context);
            }

            if (logFileEnabled)
            {
                WriteIntoLogFile(messageToShow);
            }
        }

        /// <summary>
        /// Writes the log message into file.
        /// </summary>
        /// <param name="message">The message content.</param>
        private static void WriteIntoLogFile(string message)
        {
            try
            {
                string dirPath = CheckPaths();
                string timestamp = DateTime.Now.ToString("yyyyMMddHH");
                string filePath = Path.Combine(dirPath, timestamp + logFileExtension);
                File.AppendAllText(filePath, message);
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
        }

        /// <summary>
        /// Checks the paths.
        /// </summary>
        /// <returns>The directory path for log file.</returns>
        private static string CheckPaths()
        {
            // Create log files path.
            if (!Directory.Exists(logFilesPath))
            {
                Directory.CreateDirectory(logFilesPath);
            }

            string dateTime = DateTime.Now.ToString("yyyy-MM-dd");
            string dirPath = Path.Combine(logFilesPath, dateTime);

            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            return dirPath;
        }
    }
}
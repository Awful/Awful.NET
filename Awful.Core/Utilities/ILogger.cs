// <copyright file="ILogger.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Awful.Core
{
    /// <summary>
    /// Log Level.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Used for filtering only; All messages are logged.
        /// </summary>
        All, // must be first

        /// <summary>
        /// Informational messages used for debugging or to trace code execution.
        /// </summary>
        Debug,

        /// <summary>
        /// Informational messages containing performance metrics.
        /// </summary>
        Perf,

        /// <summary>
        /// Informational messages that might be of interest to the user.
        /// </summary>
        Info,

        /// <summary>
        /// Warnings.
        /// </summary>
        Warn,

        /// <summary>
        /// Errors that are handled gracefully.
        /// </summary>
        Error,

        /// <summary>
        /// Errors that are not handled gracefully.
        /// </summary>
        Fail,

        /// <summary>
        /// Used for filtering only; No messages are logged.
        /// </summary>
        None, // must be last
    }

    /// <summary>
    /// Log Message.
    /// </summary>
    [Serializable]
    public sealed class LogMessage
    {
        /// <summary>
        /// Gets the time stamp format.
        /// </summary>
        public const string TimestampFormat = "yyyy-MM-dd HH:mm:ss.f";

        /// <summary>
        /// Initializes a new instance of the <see cref="LogMessage"/> class.
        /// </summary>
        /// <param name="timestamp">Timestamp.</param>
        /// <param name="level">Level.</param>
        /// <param name="message">Message.</param>
        public LogMessage(DateTime timestamp, LogLevel level, string message)
        {
            if (level <= LogLevel.All || level >= LogLevel.None)
            {
                throw new ArgumentException("Invalid log level", nameof(level));
            }

            if (message is null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            this.Timestamp = timestamp;
            this.Level = level;
            this.Message = message;
        }

        /// <summary>
        /// Gets the timestamp.
        /// </summary>
        public DateTime Timestamp { get; }

        /// <summary>
        /// Gets the log level.
        /// </summary>
        public LogLevel Level { get; }

        /// <summary>
        /// Gets the message.
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Create a log message.
        /// </summary>
        /// <param name="message">Message to add.</param>
        /// <returns>LogMessage.</returns>
        public LogMessage WithMessage(string message)
        {
            var result = (LogMessage)MemberwiseClone();
            result.Message = message;
            return result;
        }

        /// <summary>
        /// Log To String.
        /// </summary>
        /// <returns>String.</returns>
        public override string ToString()
            => $"[Awful.Core] ({this.Timestamp.ToString(TimestampFormat)}): {this.Level.ToString().ToUpperInvariant()}: {this.Message}";

        /// <summary>
        /// Return message appropriate for the output pad/pane, which is more visible to the end user (but still technical).
        /// Here, we want to have somewhat concise output, minimzing horizontal scrolling.
        /// </summary>
        /// <returns>String.</returns>
        public string ToOutputPaneString()
        {
            // In the output pane, only show time, not date, to make it more concise.
            // Use the locale specific long time format (e.g. "1:45:30 PM" for en-US)
            string timestamp = this.Timestamp.ToString("T");

            return $"[{timestamp}]  {this.Message}";
        }
    }

    /// <summary>
    /// ILogger.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Log.
        /// </summary>
        /// <param name="message">Write log message.</param>
        void Log(LogMessage message);
    }

    /// <summary>
    /// Logger.
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// Write Log.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="level">Level.</param>
        /// <param name="message">Message.</param>
        public static void Log(this ILogger logger, LogLevel level, string? message)
            => logger.Log(new LogMessage(DateTime.Now, level, message ?? string.Empty));

        /// <summary>
        /// Write Log.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="ex">Exception.</param>
        /// <param name="level">Level.</param>
        /// <param name="memberName">Member Name.</param>
        /// <param name="sourceLineNumber">Source Line Number.</param>
        public static void Log(
            this ILogger logger,
            Exception ex,
            LogLevel level = LogLevel.Error,
            [CallerMemberName] string memberName = "(unknown)",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            logger.Log(level, $"Caught exception in {memberName} at {sourceLineNumber}: {ex}\n{ex.StackTrace}");
        }

        /// <summary>
        /// Log method if faulted.
        /// </summary>
        /// <param name="task">Task.</param>
        /// <param name="logger">Logger.</param>
        /// <param name="level">Level.</param>
        /// <param name="memberName">Member Name.</param>
        /// <param name="sourceLineNumber">Source Line Number.</param>
        public static void LogIfFaulted(
            this Task task,
            ILogger logger,
            LogLevel level = LogLevel.Error,
            [CallerMemberName] string memberName = "(unknown)",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            task.ContinueWith(
                t => logger.Log(t.Exception, level, memberName, sourceLineNumber),
                TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously);
        }

        /// <summary>
        /// Log.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="sw">Stopwatch.</param>
        /// <param name="level">Level.</param>
        /// <param name="memberName">Member Name.</param>
        /// <param name="sourceLineNumber">Source Line Number.</param>
        public static void Log(
            this ILogger logger,
            Stopwatch sw,
            LogLevel level = LogLevel.Perf,
            [CallerMemberName] string memberName = "(unknown)",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            logger.Log(level, $"Elapsed time in {memberName} at {sourceLineNumber}: {sw.ElapsedMilliseconds}ms");
        }

        /// <summary>
        /// Returns a new <see cref="ILogger"/> that prefixes every message with
        /// parenthesis and the given tag.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="tag">Tag.</param>
        /// <param name="includeTagInUserVisibleMessages">Include Taag In User Visible Message.</param>
        /// <returns>Tagged Logger.</returns>
        public static ILogger WithTag(this ILogger logger, string tag, bool includeTagInUserVisibleMessages = false)
            => new TaggedLogger(logger, tag, includeTagInUserVisibleMessages);
    }

    /// <summary>
    /// Console Logger.
    /// </summary>
    public class ConsoleLogger : ILogger
    {
        /// <summary>
        /// Gets or Sets the minimum <see cref="LogLevel"/> for this logger.
        /// </summary>
        public LogLevel LogLevel { get; set; } = LogLevel.All;

        /// <summary>
        /// Logs.
        /// </summary>
        /// <param name="message">Message.</param>
        public virtual void Log(LogMessage message)
        {
            if (message.Level < this.LogLevel)
            {
                return;
            }

            Console.WriteLine(message.Message);
        }
    }

    /// <summary>
    /// Console Logger.
    /// </summary>
    public class DebuggerLogger : ILogger
    {
        /// <summary>
        /// Gets or Sets the minimum <see cref="LogLevel"/> for this logger.
        /// </summary>
        public LogLevel LogLevel { get; set; } = LogLevel.All;

        /// <summary>
        /// Logs.
        /// </summary>
        /// <param name="message">Message.</param>
        public virtual void Log(LogMessage message)
        {
            if (message.Level < this.LogLevel)
            {
                return;
            }

            System.Diagnostics.Debug.WriteLine(message.Message);
        }
    }

    /// <summary>
    /// Tagged Logger.
    /// </summary>
    class TaggedLogger : ILogger
    {
        private ILogger logger;
        private string tag;
        private bool includeTagInUserVisibileMessages;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaggedLogger"/> class.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="tag">Tag.</param>
        /// <param name="includeTagInUserVisibileMessages">Include Tag In User Visible Messages.</param>
        public TaggedLogger(ILogger logger, string tag, bool includeTagInUserVisibileMessages)
        {
            this.logger = logger;
            this.tag = tag;
            this.includeTagInUserVisibileMessages = includeTagInUserVisibileMessages;
        }

        /// <summary>
        /// Log.
        /// </summary>
        /// <param name="log">Log message.</param>
        public void Log(LogMessage log)
        {
            LogLevel level = log.Level;
            bool isUserVisible = level == LogLevel.Info || level == LogLevel.Warn || level == LogLevel.Error;
            bool includeTag = !isUserVisible || this.includeTagInUserVisibileMessages;

            if (includeTag)
            {
                this.logger.Log(log.WithMessage($"({this.tag}) {log.Message}"));
            }
            else
            {
                this.logger.Log(log);
            }
        }
    }
}
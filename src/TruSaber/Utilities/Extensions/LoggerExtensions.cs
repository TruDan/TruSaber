using System;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.Logging
{

	/// <summary>
	/// Do you also have an extremely strong dislike towards Microsoft's naming style
	/// for their ILogger implementation? Yes? Then this is the perfect extension
	/// class for you! :D
	/// </summary>
	public static class LoggerExtensions
	{
		#region Trace

		[StringFormatMethod("message")]
		public static void Trace(this ILogger logger, string message, params object[] args)
		{
			logger.LogTrace(message, args);
		}

		[StringFormatMethod("message")]
		public static void Trace(this ILogger logger, EventId eventId, string message, params object[] args)
		{
			logger.LogTrace(eventId, message, args);
		}

		[StringFormatMethod("message")]
		public static void Trace(this ILogger logger, Exception exception, string message, params object[] args)
		{
			logger.LogTrace(exception, message, args);
		}

		[StringFormatMethod("message")]
		public static void Trace(this ILogger logger, string message, Exception exception, params object[] args)
		{
			logger.LogTrace(exception, message, args);
		}

		[StringFormatMethod("message")]
		public static void Trace(this ILogger logger, EventId eventId, Exception exception, string message, params object[] args)
		{
			logger.LogTrace(eventId, exception, message, args);
		}

		#endregion

		#region Debug

		[StringFormatMethod("message")]
		public static void Debug(this ILogger logger, string message, params object[] args)
		{
			logger.LogDebug(message, args);
		}

		[StringFormatMethod("message")]
		public static void Debug(this ILogger logger, EventId eventId, string message, params object[] args)
		{
			logger.LogDebug(eventId, message, args);
		}

		[StringFormatMethod("message")]
		public static void Debug(this ILogger logger, Exception exception, string message, params object[] args)
		{
			logger.LogDebug(exception, message, args);
		}

		[StringFormatMethod("message")]
		public static void Debug(this ILogger logger, string message, Exception exception, params object[] args)
		{
			logger.LogDebug(exception, message, args);
		}

		[StringFormatMethod("message")]
		public static void Debug(this ILogger logger, EventId eventId, Exception exception, string message, params object[] args)
		{
			logger.LogDebug(eventId, exception, message, args);
		}

		#endregion

		#region Information

		[StringFormatMethod("message")]
		public static void Info(this ILogger logger, string message, params object[] args)
		{
			logger.LogInformation(message, args);
		}

		[StringFormatMethod("message")]
		public static void Info(this ILogger logger, EventId eventId, string message, params object[] args)
		{
			logger.LogInformation(eventId, message, args);
		}

		[StringFormatMethod("message")]
		public static void Info(this ILogger logger, Exception exception, string message, params object[] args)
		{
			logger.LogInformation(exception, message, args);
		}

		[StringFormatMethod("message")]
		public static void Info(this ILogger logger, string message, Exception exception, params object[] args)
		{
			logger.LogInformation(exception, message, args);
		}

		[StringFormatMethod("message")]
		public static void Info(this ILogger logger, EventId eventId, Exception exception, string message, params object[] args)
		{
			logger.LogInformation(eventId, exception, message, args);
		}

		#endregion

		#region Warning

		[StringFormatMethod("message")]
		public static void Warn(this ILogger logger, string message, params object[] args)
		{
			logger.LogWarning(message, args);
		}

		[StringFormatMethod("message")]
		public static void Warn(this ILogger logger, EventId eventId, string message, params object[] args)
		{
			logger.LogWarning(eventId, message, args);
		}

		[StringFormatMethod("message")]
		public static void Warn(this ILogger logger, Exception exception, string message, params object[] args)
		{
			logger.LogWarning(exception, message, args);
		}

		[StringFormatMethod("message")]
		public static void Warn(this ILogger logger, string message, Exception exception, params object[] args)
		{
			logger.LogWarning(exception, message, args);
		}

		[StringFormatMethod("message")]
		public static void Warn(this ILogger logger, EventId eventId, Exception exception, string message, params object[] args)
		{
			logger.LogWarning(eventId, exception, message, args);
		}

		#endregion

		#region Error

		[StringFormatMethod("message")]
		public static void Error(this ILogger logger, string message, params object[] args)
		{
			logger.LogError(message, args);
		}

		[StringFormatMethod("message")]
		public static void Error(this ILogger logger, EventId eventId, string message, params object[] args)
		{
			logger.LogError(eventId, message, args);
		}

		[StringFormatMethod("message")]
		public static void Error(this ILogger logger, Exception exception, string message, params object[] args)
		{
			logger.LogError(exception, message, args);
		}

		[StringFormatMethod("message")]
		public static void Error(this ILogger logger, string message, Exception exception, params object[] args)
		{
			logger.LogError(exception, message, args);
		}

		[StringFormatMethod("message")]
		public static void Error(this ILogger logger, EventId eventId, Exception exception, string message, params object[] args)
		{
			logger.LogError(eventId, exception, message, args);
		}

		#endregion

		#region Critical

		[StringFormatMethod("message")]
		public static void Critical(this ILogger logger, string message, params object[] args)
		{
			logger.LogCritical(message, args);
		}

		[StringFormatMethod("message")]
		public static void Critical(this ILogger logger, EventId eventId, string message, params object[] args)
		{
			logger.LogCritical(eventId, message, args);
		}

		[StringFormatMethod("message")]
		public static void Critical(this ILogger logger, Exception exception, string message, params object[] args)
		{
			logger.LogCritical(exception, message, args);
		}

		[StringFormatMethod("message")]
		public static void Critical(this ILogger logger, string message, Exception exception, params object[] args)
		{
			logger.LogCritical(exception, message, args);
		}

		[StringFormatMethod("message")]
		public static void Critical(this ILogger logger, EventId eventId, Exception exception, string message, params object[] args)
		{
			logger.LogCritical(eventId, exception, message, args);
		}

		#endregion
	}
}
using System;
using log4net;
using Rebus.Logging;
using ILog = Rebus.Logging.ILog;

namespace Rebus
{
    /// <summary>
    /// Logger factory that creates Log4net-based loggers
    /// </summary>
    public class Log4NetLoggerFactory : AbstractRebusLoggerFactory
    {
        /// <inheritdoc />
        protected override ILog GetLogger(Type type)
        {
            return new Log4NetLogger(LogManager.GetLogger(type), this);
        }

        class Log4NetLogger : ILog
        {
            readonly log4net.ILog _logger;
            readonly Log4NetLoggerFactory _loggerFactory;

            public Log4NetLogger(log4net.ILog logger, Log4NetLoggerFactory loggerFactory)
            {
                _logger = logger;
                _loggerFactory = loggerFactory;
            }

            public void Debug(string message, params object[] objs)
            {
                _logger.Debug(SafeFormat(message, objs));
            }

            public void Info(string message, params object[] objs)
            {
                _logger.Info(SafeFormat(message, objs));
            }

            public void Warn(string message, params object[] objs)
            {
                _logger.Warn(SafeFormat(message, objs));
            }

            public void Error(Exception exception, string message, params object[] objs)
            {
                _logger.Error(SafeFormat(message, objs), exception);
            }

            public void Error(string message, params object[] objs)
            {
                _logger.Error(SafeFormat(message, objs));
            }

            string SafeFormat(string message, object[] objs) => _loggerFactory.RenderString(message, objs);
        }

    }
}

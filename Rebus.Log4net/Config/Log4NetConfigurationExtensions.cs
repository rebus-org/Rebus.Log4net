using System;
using log4net;
using Rebus.Log4net;
using Rebus.Pipeline;
// ReSharper disable EmptyGeneralCatchClause

namespace Rebus.Config;

/// <summary>
/// Configuration extensions for setting up logging with Log4net
/// </summary>
public static class Log4NetConfigurationExtensions
{
    /// <summary>
    /// Configures Rebus to use Log4Net for all of its internal logging, getting its loggers by calling logger <see cref="LogManager.GetLogger(Type)"/>
    /// </summary>
    public static void Log4Net(this RebusLoggingConfigurer configurer)
    {
        if (configurer == null) throw new ArgumentNullException(nameof(configurer));

        configurer.Use(new Log4NetLoggerFactory());

        configurer.Decorate<IPipeline>(c =>
        {
            var pipeline = c.Get<IPipeline>();

            return new PipelineStepConcatenator(pipeline)
                .OnReceive(new Log4NetContextStep(), PipelineAbsolutePosition.Front);
        });
    }
}
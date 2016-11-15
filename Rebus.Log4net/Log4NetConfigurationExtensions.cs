using System;
using System.Reflection;
using log4net;
using Rebus.Config;
using Rebus.Injection;
using Rebus.Pipeline;

namespace Rebus.Log4net
{
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

            try
            {
                TryConfigurePipelineStep(configurer);
            }
            catch { }
        }

        static void TryConfigurePipelineStep(RebusLoggingConfigurer configurer)
        {
            var injectionistField = configurer.GetType()
                .GetField("_injectionist", BindingFlags.Instance | BindingFlags.NonPublic);

            var injectionist = injectionistField?.GetValue(configurer) as Injectionist;

            injectionist?.Decorate<IPipeline>(c =>
            {
                var pipeline = c.Get<IPipeline>();

                return new PipelineStepConcatenator(pipeline)
                    .OnReceive(new Log4NetContextStep(), PipelineAbsolutePosition.Front);
            });
        }
    }
}
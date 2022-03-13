using System;
using System.Threading.Tasks;
using System.Timers;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;
using Rebus.Activation;
using Rebus.Config;
using Rebus.Handlers;
using Rebus.Transport.InMem;
// ReSharper disable AccessToDisposedClosure
#pragma warning disable 1998

namespace TestApplication
{
    static class Program
    {
        static void Main()
        {
            BasicConfigurator.Configure(new ConsoleAppender
            {
                Layout = new PatternLayout("%timestamp [%thread] %level %logger %property{CorrelationId} - %message%newline")
            });

            using var activator = new BuiltinHandlerActivator();

            activator.Register(() => new RealisticHandler());

            Configure.With(activator)
                .Logging(l => l.Log4Net())
                .Transport(t => t.UseInMemoryTransport(new InMemNetwork(), "doesn't matter"))
                .Start();

            using var timer = new Timer(2000);

            timer.Elapsed += (_, _) => activator.Bus.SendLocal("hello there");
            timer.Start();

            Console.WriteLine("Press ENTER to quit");
            Console.ReadLine();
        }
    }

    class RealisticHandler : IHandleMessages<string>
    {
        static readonly ILog Log = LogManager.GetLogger(typeof(RealisticHandler));

        public async Task Handle(string message)
        {
            Log.InfoFormat("Handling string message '{0}'", message);
        }
    }
}

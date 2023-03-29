using System;
using System.Threading.Tasks;
using log4net;
using Rebus.Messages;
using Rebus.Pipeline;

namespace Rebus.Log4net;

class Log4NetContextStep : IIncomingStep
{
    public async Task Process(IncomingStepContext context, Func<Task> next)
    {
        var transportMessage = context.Load<TransportMessage>();

        if (transportMessage.Headers.TryGetValue(Headers.CorrelationId, out var correlationId))
        {
            LogicalThreadContext.Properties["CorrelationId"] = correlationId;
        }

        try
        {
            await next();
        }
        finally
        {
            LogicalThreadContext.Properties.Remove("CorrelationId");
        }
    }
}
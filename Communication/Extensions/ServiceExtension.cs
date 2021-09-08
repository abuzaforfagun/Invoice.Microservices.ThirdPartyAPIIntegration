using System;
using System.Collections.Generic;
using System.Reflection;
using Communication.Attributes;
using Communication.Consumer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Communication.Extensions
{
    public static class ServiceExtension
    {
        public static void RegisterServiceBus(this IApplicationBuilder applicationBuilder, IServiceProvider serviceProvider, List<Type> distributedCommands, Assembly assembly, string connectionString)
        {
            foreach (var queue in distributedCommands)
            {
                using var scope = serviceProvider.CreateScope();
                var bus = scope.ServiceProvider.GetRequiredService<IDistributedConsumer>();

                var queueName = queue.GetAttributeValue((ServiceBusQueue att) => att.Name);

                var method = bus.GetType().GetMethod("RegisterQueueHandler");

                if (method is null) continue;

                var generic = method.MakeGenericMethod(queue);


                generic.Invoke(bus, new object[] { connectionString, queueName });
            }
        }
    }
}

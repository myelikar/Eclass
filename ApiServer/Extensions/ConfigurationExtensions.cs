using MassTransit;
using Messages.Institute;
using RabbitMQ.Client;

namespace ApiServer.Extensions
{
    public static class ConfigurationExtensions
    {
        public static void ConfigureMessageTopology(this IRabbitMqBusFactoryConfigurator configurator)
        {

        }
    }
}

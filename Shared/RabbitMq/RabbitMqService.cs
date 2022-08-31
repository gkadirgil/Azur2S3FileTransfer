using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.RabbitMq
{
	public class RabbitMqService
	{
        protected IConnection GetRabbitMQConnection()
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.UserName = Settings.RabbitMQUserName;
            factory.Password = Settings.RabbitMQPassword;
            factory.VirtualHost = "/";
            //factory.Protocol = Protocols.AMQP_0_9_1;
            factory.HostName = Settings.RabbitMqHost;
            factory.Port = 5672;

            return factory.CreateConnection();
        }
    }
}

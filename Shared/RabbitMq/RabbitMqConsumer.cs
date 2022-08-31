using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.RabbitMq
{
	public class RabbitMqConsumer : RabbitMqService
	{
		private string _queueName { get; set; }
		private IConnection _connection;
		private IModel _channel;

		private bool _AutoACK = true;

		public EventingBasicConsumer Consumer;

		public RabbitMqConsumer(string QueueName)
		{
            _queueName = QueueName;

            try
            {
                _connection = GetRabbitMQConnection();
                _channel = _connection.CreateModel();

                _channel.QueueDeclare(_queueName, true, false, false, null);

                Consumer = new EventingBasicConsumer(_channel);
              
            }
            catch (Exception ex)
            {
                Console.WriteLine("[Consumer Error] : ", ex.Message.ToString(), "\n", ex.InnerException.ToString());
            }
        }

        public string GetMessage()
        {
            var obj = _channel.BasicGet(_queueName, _AutoACK);
            var body = obj?.Body.ToArray();
            if (body is null)
                return null;

            return Encoding.UTF8.GetString(body);
        }
        public string Get(bool replace = true, bool loop = true)
        {
            string message = GetMessage();
            if (message is null && loop)
                Console.WriteLine("There is no data in queue, waiting...");
            while (message == null && loop)
                message = GetMessage();

            if (!string.IsNullOrEmpty(message) && replace == true)
                message = message.Replace("\"", "");
            return message;
        }
    }
}

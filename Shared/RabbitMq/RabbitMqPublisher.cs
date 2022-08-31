using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.RabbitMq
{
	public class RabbitMqPublisher:RabbitMqService
	{
		private string _queueName { get; set; }
		public RabbitMqPublisher(string QueueName)
		{
			_queueName = QueueName;
		}
		public bool Publish(string key)
		{
			using (var connection = GetRabbitMQConnection())
			{
				using (var channel = connection.CreateModel())
				{
					channel.QueueDeclare(_queueName, true, false, false, null);

					if (!string.IsNullOrEmpty(key))
					{
						channel.BasicPublish("",_queueName,false,null,Encoding.UTF8.GetBytes(key));
						return true;
					}

					return false;
				}
			}
		}
	}
}

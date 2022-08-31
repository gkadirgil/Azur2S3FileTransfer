using System;
using System.Configuration;

namespace AzurPublishFiles
{
	class Program
	{
		public static void Main(string[] args)
		{
			string containerName = ConfigurationManager.AppSettings["ContainerName"].ToString();

			string queueName = $"azure-{containerName}";			
			
			Console.WriteLine("Waiting...");

			var Publisher = new Shared.Azure.AzureService(containerName, queueName);

			Publisher.PublishListContainer();

			Console.WriteLine("Process Completed...");

			Console.ReadLine();

			
		}
	}
}

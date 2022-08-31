using System;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Azur2S3
{
	class Program
	{
		static async Task Main(string[] args)
		{
			string containerName = ConfigurationManager.AppSettings["ContainerName"].ToString();

			string queueName = $"azure-{containerName}";

			Console.WriteLine("Listening...");


			var consumer = new Shared.RabbitMq.RabbitMqConsumer(queueName);

			var s3Service = new Shared.AWS.S3Service();

			var azureService = new Shared.Azure.AzureService(containerName, queueName);



			while (true)
			{
				string keyName = consumer.Get();

				bool isExist = s3Service.ObjectCheck(keyName);

				if (!isExist)
				{
					int tryAgain = 5;
					while (true)
					{
						try
						{
							string fileName = keyName.Replace(containerName + "/", "");
							Task<MemoryStream> result = azureService.GetFileAsync(fileName);

							await s3Service.UploadFileAsync(result.Result, keyName);

							break;

						}
						catch (Exception ex)
						{

							Console.WriteLine($"*** Upload Error: (keyname:{keyName}): {ex.Message}");

							Thread.Sleep(1000);
							tryAgain--;
						}
						if (tryAgain == 0)
							break;
					}
				} 
			}

		}

	}


}

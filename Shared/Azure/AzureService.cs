using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Azure
{
	
	public class AzureService
	{
		BlobContainerClient _blobContainerClient;
		RabbitMq.RabbitMqPublisher _publish;
		string _containerName;
		public AzureService(string ContainerName, string QueueName)
		{
			_blobContainerClient = AzureBlobContainerClient(ContainerName);
			_containerName = ContainerName;
			_publish = new RabbitMq.RabbitMqPublisher(QueueName);
		}

		public void PublishListContainer()
		{
			foreach (BlobItem file in _blobContainerClient.GetBlobs())
			{

				try
				{
					string keyName = $"{_containerName}/{file.Name}";

					int tryAgain = 5;
					while (true)
					{
						try
						{
							_publish.Publish(keyName);

							Console.WriteLine($"{DateTime.Now} - {keyName}");

							break;
						}
						catch (Exception ex)
						{

							Console.WriteLine($"*** Publish Error: (keyname:{keyName}): {ex.Message}");
							GC.Collect();
							Thread.Sleep(1000);
							tryAgain--;
						}
						if (tryAgain == 0)
							break;
					}
					
				}
				
				catch (RequestFailedException e)
				{
					Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
				}
			}
		}

		public async Task<MemoryStream> GetFileAsync(string fileName)
		{

			var blobClient = _blobContainerClient.GetBlobClient(fileName);

			var memoryStream = new MemoryStream();
			await blobClient.DownloadToAsync(memoryStream);

			return memoryStream;
		}

		public BlobContainerClient AzureBlobContainerClient(string ContainerName)
		{
			return new BlobContainerClient(Settings.AzureBlobConnStr, ContainerName);
		}

		public static List<BlobDto> ListContainerFiles(BlobContainerClient client)
		{

			// Create a new list object for 
			List<BlobDto> files = new List<BlobDto>();
			foreach (BlobItem file in client.GetBlobs())
			{
				// Add each file retrieved from the storage container to the files list by creating a BlobDto object
				string uri = client.Uri.ToString();
				var name = file.Name;
				var fullUri = $"{uri}/{name}";

				files.Add(new BlobDto
				{
					Uri = fullUri,
					Name = name,
					ContentType = file.Properties.ContentType
				});


			}
			// Return all files to the requesting method
			return files;
		}

	}
}

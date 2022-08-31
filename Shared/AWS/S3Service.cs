using Amazon.S3;
using Amazon.S3.IO;
using Amazon.S3.Transfer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Shared.AWS
{
	public class S3Service
	{
		AmazonS3Client _client;
		string _bucketName;
		public S3Service()
		{
			_client = S3Client();
		}


		public async Task UploadFileAsync(MemoryStream stream, string keyName)
		{
			try
			{
			
				TransferUtility fileTransferUtility = new TransferUtility(_client);
				TransferUtilityUploadRequest uploadRequest = new TransferUtilityUploadRequest
				{
					BucketName = Settings.AwsBucketName,
					CannedACL = S3CannedACL.PublicReadWrite,
					InputStream = stream,
					Key = keyName,
				};
				await fileTransferUtility.UploadAsync(uploadRequest);
			
				Console.WriteLine(DateTime.Now + " - " + keyName);
			
			}
			catch (AmazonS3Exception e)
			{
				Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
			}
			catch (Exception e)
			{
				Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
			}
		}
		public bool ObjectCheck(string keyName)
		{
			S3FileInfo s3FileInfo = new Amazon.S3.IO.S3FileInfo(_client, Settings.AwsBucketName, keyName);

			if (s3FileInfo.Exists)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		public AmazonS3Client S3Client()
		{			
			return new AmazonS3Client(Settings.AwsAccessKeyId, Settings.AwsSecretAccessKey, Amazon.RegionEndpoint.EUCentral1);
		}
	}
}

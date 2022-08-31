namespace Shared
{
	public class Settings
	{


		#region AWS S3
		public const string AwsAccessKeyId = "AWS_ACCESS_KEY_ID";
		public const string AwsSecretAccessKey = "AWS_SECRET_ACCESSKEY";

		public const string AwsBucketName = "BUCKET_NAME";
		public const string AwsS3EndPoint = "AWS_ENDPOINT";

		#endregion

		#region Azure

		public const string AzureBlobConnStr = "AZURE_BLOB_CONNECTION_STRING";

		#endregion

		#region RabbitMQ

		public static string RabbitMqHost = "HOST_IP";
		public static string RabbitMQUserName = "USER_NAME";
		public static string RabbitMQPassword = "PASSWORD";

		#endregion


	}
}

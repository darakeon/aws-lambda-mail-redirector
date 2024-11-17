using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;

namespace Redirector;

public class S3
{
	public S3()
	{
		client = new AmazonS3Client(
			Cfg.File.Username,
			Cfg.File.Password,
			Cfg.Region
		);
	}

	private static AmazonS3Client client;

	public void UseFile(String objectKey, Action<String> action)
	{
		try
		{
			var content = getFile(objectKey);
			Console.WriteLine($"{objectKey}: {content}");

			action(content);

			deleteFile(objectKey);
		}
		catch (Exception e)
		{
			Console.WriteLine("Fail while reading S3:");
			Console.WriteLine(e);

			action($"Could not recover file, look for {objectKey}");
		}
	}

	private String getFile(String objectKey)
	{
		Console.WriteLine($"Trying to get file {objectKey}");
		var read = getFileAsync(objectKey);
		read.Wait();
		Console.WriteLine($"File {objectKey} got successfully!");

		return read.Result;
	}

	private async Task<String> getFileAsync(String objectKey)
	{
		var request = new GetObjectRequest
		{
			BucketName = Cfg.File.Bucket,
			Key = objectKey,
		};

		using var response = await client.GetObjectAsync(request);
		await using var responseStream = response.ResponseStream;
		using var reader = new StreamReader(responseStream);

		return await reader.ReadToEndAsync();
	}

	private Boolean deleteFile(String objectKey)
	{
		Console.WriteLine($"Trying to delete file {objectKey}");
		var read = deleteFileAsync(objectKey);
		read.Wait();
		Console.WriteLine($"File {objectKey} deleted successfully!");

		return read.Result;
	}

	private async Task<Boolean> deleteFileAsync(String objectKey)
	{
		var request = new DeleteObjectRequest
		{
			BucketName = Cfg.File.Bucket,
			Key = objectKey,
		};

		var response = await client.DeleteObjectAsync(request);

		return response.HttpStatusCode < HttpStatusCode.Ambiguous;
	}
}

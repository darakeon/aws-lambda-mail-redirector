using System;
using System.IO;
using System.Text;
using Amazon.Lambda.Serialization.SystemTextJson;

namespace Redirector;

internal class Json<T>
{
	public static T FromFile(String file)
	{
		var text = File.ReadAllText(file);
		return FromText(text);
	}

	public static T FromText(String text)
	{
		var bytes = Encoding.UTF8.GetBytes(text);
		var stream = new MemoryStream(bytes);

		return new DefaultLambdaJsonSerializer()
			.Deserialize<T>(stream);
	}
}

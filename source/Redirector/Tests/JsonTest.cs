using System;
using System.Collections.Generic;

namespace Redirector.Tests;

internal class JsonTest
{
	public static void Test()
	{
		var json = "{\"key1\": \"value1\", \"key2\": \"value2\"}";
		var dic = Json<Dictionary<String, String>>.FromText(json);

		Console.WriteLine($"Start");

		assert(dic.ContainsKey("key1"), true);
		assert(dic["key1"], "value1");
		assert(dic.ContainsKey("key2"), true);
		assert(dic["key2"], "value2");

		Console.WriteLine($"End");
	}

	private static void assert<T>(T value, T expected)
	{
		if (value.Equals(expected))
			return;

		Console.WriteLine($"Difference: {value}, expected {expected}");
	}
}

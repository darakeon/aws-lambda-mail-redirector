using System;

namespace Redirector.Tests
{
	class S3Test
	{
		public static void Test()
		{
			var s3 = new S3();
			s3.UseFile(
				"AMAZON_SES_SETUP_NOTIFICATION",
				Console.WriteLine
			);
		}
	}
}

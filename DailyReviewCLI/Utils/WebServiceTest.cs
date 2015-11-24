/*
 * Created by SharpDevelop.
 * User: terekhov-ev
 * Date: 24.11.2015
 * Time: 16:34
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using NUnit.Framework;

namespace DailyReviewCLI.Utils
{
	[TestFixture]
	public class WebServiceTest
	{
		[Test]
		public void TestMethod()
		{
			Console.WriteLine(WebServices.getProductivityData("2015-11-23"));
		}
	}
}

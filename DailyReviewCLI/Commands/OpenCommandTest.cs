/*
 * Created by SharpDevelop.
 * User: terekhov-ev
 * Date: 12.08.2015
 * Time: 11:53
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Specialized;
using NUnit.Framework;
using DailyReviewCLI.Utils;

namespace DailyReviewCLI.Commands {
	[TestFixture]
	public class OpenCommandTest {
		[Test]
		public void shouldCheckArguments() {
			StringDictionary context = new StringDictionary();
			OpenCommand target;
			FakeFileSystemWrapper fakeFSW = new FakeFileSystemWrapper();
			
			// 
			context["command"] = "open";
			context["date"]="2015-01-01";
			target = new OpenCommand(context);
			target.setFSWrapper(fakeFSW);
			target.run();
			Assert.IsTrue(target.dayExists());
		}
	}
}

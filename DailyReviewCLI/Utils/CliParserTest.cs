/*
 * Created by SharpDevelop.
 * User: terekhov-ev
 * Date: 11.08.2015
 * Time: 13:48
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using NUnit.Framework;

namespace DailyReviewCLI.Utils {
	[TestFixture]
	public class CliParserTest {
		
		[Test]
		public void shouldCreateHelpCommandForEmptyArgs() {
			var target = CliParser.parse(new string[] {});
			Assert.AreEqual(
				expected:"help", 
				actual:target["command"]);
		}
		
		[Test]
		public void shouldGetCorrectCommand() {
			
			// First argument should be a correct command
			var target = CliParser.parse(new [] {"open"});
			Assert.AreEqual("open", target["command"]);
			
			// If it's not - we should create Help command
			target = CliParser.parse(new string[] {"nonsense","open"});
			Assert.AreEqual("help", target["command"]);
			
		}
		
		[Test]
		public void shouldGetCorrectDate() {
			
			// -d or --date key is optional
			// If it's not set, then context's date is today
			var target = CliParser.parse(new string[] {"open"});
			Assert.AreEqual(
				expected:String.Format("{0:0000}-{1:00}-{2:00}", DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day), 
				actual:target["date"]);
			
			// If it's set, then check provided value...
			target = CliParser.parse(new string[] {"open", "-d", "not-a-date"});
			Assert.AreEqual(
				expected:"help",
				actual:target["command"]);
			
			// ...and use it if it's valid
			string birthDay = "2015-05-23";
			target = CliParser.parse(new string[] {"open", "-d", birthDay});
			Assert.AreEqual(
				expected:birthDay,
				actual:target["date"]);
			
			
			// The same should be applicable for long key format
			string newYear = "2015-01-01";
			target = CliParser.parse(new string[] {"open", "--date", newYear});
			Assert.AreEqual(
				expected:newYear,
				actual:target["date"]);			
			
			target = CliParser.parse(new string[] {"open", "-d", "2015-02-32"});
			Assert.AreEqual(
				expected:"help",
				actual:target["command"]);
			
			// If -d or --date key is the last argument, then we should create
			// help command
			target = CliParser.parse(new string[] {"open", "-d"});
			Assert.AreEqual(
				expected:"help",
				actual:target["command"]);
		}

				
	}
}

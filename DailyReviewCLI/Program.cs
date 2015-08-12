/*
 * Created by SharpDevelop.
 * User: terekhov-ev
 * Date: 11.08.2015
 * Time: 11:43
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Specialized;

using DailyReviewCLI.Commands;
using DailyReviewCLI.Utils;

namespace DailyReviewCLI {
	class Program {

		
		public static void Main(string[] args) {
			StringDictionary context = CliParser.parse(args);
			FileSystemWrapper fsw = new FileSystemWrapper();
			
			IRunnable command = getNewCommand(context, fsw);
			command.run();
			
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}

		static IRunnable getNewCommand(StringDictionary context, FileSystemWrapper fsw) {
			switch (context["command"]) {
				case "open":
					return new OpenCommand(context, fsw);
				default:
					throw new NotImplementedException();
			}
		}
	}
}
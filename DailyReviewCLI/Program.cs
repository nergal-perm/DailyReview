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
			StringDictionary context = Utils.CliParser.parse(args);
			// TODO: Выполнить нужную команду
			Console.WriteLine("Hello World!");
			
			// Прототип идеи
			//CreateMarkdownNote();
			foreach (string key in context.Keys) {
				Console.WriteLine("{0,-10}:\t{1}",key, context[key]);
			}
			
			FileSystemWrapper fsw = new FileSystemWrapper();
			Console.WriteLine("File {0} exists? {1}", context["date"], fsw.dayExists(context["date"]));
			OpenCommand command = new OpenCommand(context);
			command.setFSWrapper(fsw);
			command.run();
			
			
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}

	}
}
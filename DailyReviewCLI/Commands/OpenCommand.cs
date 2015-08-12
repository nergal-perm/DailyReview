/*
 * Created by SharpDevelop.
 * User: terekhov-ev
 * Date: 12.08.2015
 * Time: 11:25
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using DailyReviewCLI.Utils;

namespace DailyReviewCLI.Commands {
	/// <summary>
	/// Description of OpenCommand.
	/// </summary>
	public class OpenCommand : IRunnable {
		
		private StringDictionary _context;
		private FileSystemWrapper _fsw;
		private const string TODO_DIR = @"c:\users\terekhov-ev\dropbox\todo\";
		
		#region IRunnable implementation

		public void run() {
			// Пересоздавать файл нельзя
			if (_fsw.dayExists(_context["date"])) {
				Console.WriteLine("Файл уже существует");
				return;
			}
			
			var tasks = _fsw.getTasksForDate(_context["date"]);
			foreach (var task in tasks) {
				Console.WriteLine(task);
			}			
			
			CreateFilledNoteWith(tasks, _context["date"]);
		}
		#endregion

		public OpenCommand(StringDictionary context, FileSystemWrapper fsw) {
			_context = context;
			_fsw = fsw;
		}
		
		private void CreateFilledNoteWith(string[] tasks, string curDate) {
			var lines =  new List<string>();
			lines.Add("# План на день:");
			foreach (var task in tasks) {
				lines.Add("- [ ] " + task.Trim());
			}
			lines.Add("");
			lines.Add("# Триста букв:");
			lines.Add("");
			lines.Add("");
			lines.Add("# События и результаты:");
			lines.Add("");
			lines.Add("");
			
			_fsw.WriteToMarkdown(lines.ToArray(), _context["date"]);
		}		
	}
}

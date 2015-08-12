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
			
			//CreateFilledNoteWith(tasks, _context["date"]);
			// throw new NotImplementedException();
		}
		#endregion

		public OpenCommand(StringDictionary context) {
			_context = context;
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
			
			File.WriteAllLines(TODO_DIR + @"DayNotes\" + curDate + ".md", lines.ToArray());
		}		
		
		public void setFSWrapper(FileSystemWrapper fsw) {
			_fsw = fsw;
		}

		public bool dayExists() {
			return true;
		}
	}
}

/*
 * Created by SharpDevelop.
 * User: terekhov-ev
 * Date: 12.08.2015
 * Time: 11:25
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Specialized;
using System.Linq;
using DailyReviewCLI.Utils;

namespace DailyReviewCLI.Commands {
	/// <summary>
	/// Description of OpenCommand.
	/// </summary>
	public class OpenCommand : IRunnable {
		
		private StringDictionary _context;
		private FileSystemWrapper _fsw;
		
		#region IRunnable implementation

		public void run() {
			// Пересоздавать файл нельзя
			if (_fsw.dayExists(_context["date"])) {
				Console.WriteLine("Файл уже существует");
				return;
			}
			
			_fsw.WriteToMarkdown(_context["date"]);

		}
		#endregion

		public OpenCommand(StringDictionary context, FileSystemWrapper fsw) {
			_context = context;
			_fsw = fsw;
		}

	}
}

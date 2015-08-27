/*
 * Created by SharpDevelop.
 * User: terekhov-ev
 * Date: 13.08.2015
 * Time: 10:34
 *
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Specialized;
using System.Drawing;
using DailyReviewCLI.Utils;

namespace DailyReviewCLI.Commands {
	/// <summary>
	/// Description of CloseCommand.
	/// </summary>
	public class CloseCommand : IRunnable {
		private StringDictionary _context;
		private FileSystemWrapper _fsw;

		#region IRunnable implementation

		public void run() {
			// Пересоздавать файл нельзя
			if (!_fsw.dayExists(_context["date"])) {
				Console.WriteLine("Файла не существует");
				return;
			}

			Image chrImage = Chronodex.CreateChronodex(new ChronodexList(_fsw.getTimeData(_context["date"])));
			_fsw.SaveImage(chrImage, _context["date"]);
			_fsw.WriteToHtml(_context["date"]);

            _fsw.cleanUpFiles(_context["date"]);
        }

		#endregion

		public CloseCommand(StringDictionary context, FileSystemWrapper fsw) {
			_context = context;
			_fsw = fsw;
		}



	}
}

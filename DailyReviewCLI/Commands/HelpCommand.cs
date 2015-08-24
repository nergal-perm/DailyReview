/*
 * Created by SharpDevelop.
 * User: terekhov-ev
 * Date: 14.08.2015
 * Time: 10:21
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using DailyReviewCLI.Utils;

namespace DailyReviewCLI.Commands {
	/// <summary>
	/// Description of HelpCommand.
	/// </summary>
	public class HelpCommand : IRunnable {

		#region IRunnable implementation

		public void run() {
			string[] td = {
				"% 0800-1-3-н-всякая фигня за компом",
				"% 0815-1-1-у-итоги выходных",
				"% 0830-1-2-р-реализация выбора отчета в redmine:excel",
				"% 0845-1-1-о-тестовая многострочная деятельность",
				"% 0900-3-2-у-план дня / недели, итоги",
				"% 0945-4-3-д-связные списки Chronodex",
				"% 1045-2-3-н-кофе и фейсбук",
				"% 1115-1-1-д-связные списки Chronodex",
				"% 1130-1-3-н",
				"% 1145-1-2-д-связные списки Chronodex",
				"% 1200-3-1-н-обед",
				"% 1245-5-3-д-выноски Chronodex"
			};

			ChronodexList cl = new ChronodexList(td);
			FileSystemWrapper fs = new FileSystemWrapper();
			fs.SaveImage(Chronodex.CreateChronodex(cl), "test");
			
		}
		#endregion
		
	}
}

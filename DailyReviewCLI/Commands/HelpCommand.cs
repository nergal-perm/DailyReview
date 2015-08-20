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
			Chronodex _chr = new Chronodex();
			string[] td = {
				"% 0815-1-3-у-план дня",
				"% 0830-1-1-р-консультация Redmine",
				"% 0845-2-2-д-JavaScript в Mnemosyne"
			};
			_chr.CreateChronodex(td);
			_chr.Dispose();
		}
		#endregion
		
	}
}

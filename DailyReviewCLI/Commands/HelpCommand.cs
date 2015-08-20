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
			Chronodex chr = new Chronodex();
			chr.DrawChronodexSector("1215", 3, FocusLevel.Normal);
			chr.DrawChronodexSector("1630", 6, FocusLevel.High);
			chr.Dispose();
		}
		#endregion
		
	}
}

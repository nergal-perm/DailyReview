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

namespace DailyReviewCLI.Commands
{
	/// <summary>
	/// Description of HelpCommand.
	/// </summary>
	public class HelpCommand : IRunnable
	{
		public HelpCommand()
		{
		}

		#region IRunnable implementation

		public void run() {
			string[] lines = WebServices.getForecast();
			
			foreach (var line in lines) {
				Console.WriteLine(line);
			}
		}

		#endregion
	}
}

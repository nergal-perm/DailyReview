/*
 * Created by SharpDevelop.
 * User: terekhov-ev
 * Date: 14.08.2015
 * Time: 10:21
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.IO;
using System.Reflection;

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
			Assembly myAssembly = Assembly.GetExecutingAssembly();
			Stream myStream = myAssembly.GetManifestResourceStream("Chronodex");
			Image myImage = Image.FromStream(myStream);
			myStream.Close();
			Graphics go = Graphics.FromImage(myImage);
			
			go.FillEllipse(Brushes.Red, new Rectangle(0,0,100,100));
			myImage.Save(@"D:\Temp\test.png");
			
			myImage.Dispose();
			go.Dispose();
		}

		#endregion
	}
}

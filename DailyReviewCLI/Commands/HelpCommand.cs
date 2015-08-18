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
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;

namespace DailyReviewCLI.Commands {
	/// <summary>
	/// Description of HelpCommand.
	/// </summary>
	public class HelpCommand : IRunnable {
		public HelpCommand() {
		}

		#region IRunnable implementation

		public void run() {
			Assembly myAssembly = Assembly.GetExecutingAssembly();
			Stream myStream = myAssembly.GetManifestResourceStream("Chronodex");
			Image myImage = Image.FromStream(myStream);
			myStream.Close();
			Graphics go = Graphics.FromImage(myImage);
			
			var rectInner = new Rectangle(471, 365, 288, 288);
			var rectFirst = new Rectangle(415, 309, 400, 400);
			var rectSecond = new Rectangle(355, 249, 519, 520);
			var rectOuter = new Rectangle(298, 192, 634, 634);

//			GraphicsPath gp = new GraphicsPath();
//			gp.AddArc(rectOuter, 0, 45);	
//			gp.AddArc(rectInner, 0, 45);
//			gp.AddLine(715.8233f, 610.8233f, 838.2993f, 732.2992f);
//			gp.AddLine(757.9999f, 509f, 930.9999f, 509f);
//			
//			foreach (var point in gp.PathPoints) {
//				Console.WriteLine(point.ToString());
//			}
//			go.DrawLine(new Pen(Color.Blue, 4), 715.8233f, 610.8233f, 838.2993f, 732.2992f);
//			go.DrawLine(new Pen(Color.Red, 4), 757.9999f, 509f, 930.9999f, 509f);
//			go.FillPath(Brushes.Aqua, gp);
			
			go.DrawEllipse(new Pen(Color.Red, 1), rectOuter);
			go.DrawEllipse(new Pen(Color.Blue, 1), rectSecond);
			go.DrawEllipse(new Pen(Color.Green, 1), rectFirst);
			go.DrawEllipse(new Pen(Color.Yellow, 1), rectInner);
			
			
			myImage.Save(@"D:\Temp\test.png");
			
			myImage.Dispose();
			go.Dispose();
		}

		#endregion
	}
}

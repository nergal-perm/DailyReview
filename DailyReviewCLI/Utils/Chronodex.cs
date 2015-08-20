/*
 * Created by SharpDevelop.
 * User: terekhov-ev
 * Date: 20.08.2015
 * Time: 13:25
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;

namespace DailyReviewCLI.Utils {
	/// <summary>
	/// Description of Chronodex.
	/// </summary>
	public class Chronodex : IDisposable {
		private Dictionary<string, float> angles;
		private readonly Rectangle[] levels = {
			new Rectangle(471, 365, 288, 288),
			new Rectangle(415, 309, 400, 400),
			new Rectangle(355, 249, 519, 520),
			new Rectangle(298, 192, 634, 634)
		};
		private Dictionary<AreaType, Color> colors;
		private Graphics go;
		private Image myImage;
			
		public Chronodex() {
			initializeAnglesDictionary();
			initializeColorsDictionary();
			initializeGraphicsObject();
		}

		void initializeGraphicsObject() {
			Assembly myAssembly = Assembly.GetExecutingAssembly();
			Stream myStream = myAssembly.GetManifestResourceStream("Chronodex");
			myImage = Image.FromStream(myStream);
			myStream.Close();
			go = Graphics.FromImage(myImage);
		}
		
		
		
		private void DrawChronodexSector(TimeData timeData) {
			GraphicsPath gp = new GraphicsPath();
			
			gp.AddArc(levels[(int)timeData.Focus], angles[timeData.StartTime], timeData.Duration * 7.5f);
			gp.AddArc(levels[0], angles[timeData.StartTime], timeData.Duration * 7.5f);
			gp.AddLine(gp.PathPoints[0], gp.PathPoints[4]);
			gp.AddLine(gp.PathPoints[3], gp.PathPoints[7]);
			
			HatchBrush brush = new HatchBrush(HatchStyle.DarkDownwardDiagonal, colors[timeData.Area], Color.Transparent);
			go.FillPath(brush, gp);
			gp.Dispose();			
		}

		public Image CreateChronodex(string[] timeData) {
			foreach (var line in timeData) {
				TimeData td = CreateTimeDataFromString(line);
				DrawChronodexSector(td);
			}

			return myImage;
		}
		
		
		void initializeAnglesDictionary() {
			angles = new Dictionary<string, float>();
			
			angles.Add("0000", 270.0f);
			angles.Add("0015", 277.5f);
			angles.Add("0030", 285.0f);
			angles.Add("0045", 292.5f);
			angles.Add("0100", 300.0f);
			angles.Add("0115", 307.5f);
			angles.Add("0130", 315.0f);
			angles.Add("0145", 322.5f);
			angles.Add("0200", 330.0f);
			angles.Add("0215", 337.5f);
			angles.Add("0230", 345.0f);
			angles.Add("0245", 352.5f);
			angles.Add("0300", 0.0f);
			angles.Add("0315", 7.5f);
			angles.Add("0330", 15.0f);
			angles.Add("0345", 22.5f);
			angles.Add("0400", 30.0f);
			angles.Add("0415", 37.5f);
			angles.Add("0430", 45.0f);
			angles.Add("0445", 52.5f);
			angles.Add("0500", 60.0f);
			angles.Add("0515", 67.5f);
			angles.Add("0530", 75.0f);
			angles.Add("0545", 82.5f);
			angles.Add("0600", 90.0f);
			angles.Add("0615", 97.5f);
			angles.Add("0630", 105.0f);
			angles.Add("0645", 112.5f);
			angles.Add("0700", 120.0f);
			angles.Add("0715", 127.5f);
			angles.Add("0730", 135.0f);
			angles.Add("0745", 142.5f);
			angles.Add("0800", 150.0f);
			angles.Add("0815", 157.5f);
			angles.Add("0830", 165.0f);
			angles.Add("0845", 172.5f);
			angles.Add("0900", 180.0f);
			angles.Add("0915", 187.5f);
			angles.Add("0930", 195.0f);
			angles.Add("0945", 202.5f);
			angles.Add("1000", 210.0f);
			angles.Add("1015", 217.5f);
			angles.Add("1030", 225.0f);
			angles.Add("1045", 232.5f);
			angles.Add("1100", 240.0f);
			angles.Add("1115", 247.5f);
			angles.Add("1130", 255.0f);
			angles.Add("1145", 262.5f);
			angles.Add("1200", 270.0f);
			angles.Add("1215", 277.5f);
			angles.Add("1230", 285.0f);
			angles.Add("1245", 292.5f);
			angles.Add("1300", 300.0f);
			angles.Add("1315", 307.5f);
			angles.Add("1330", 315.0f);
			angles.Add("1345", 322.5f);
			angles.Add("1400", 330.0f);
			angles.Add("1415", 337.5f);
			angles.Add("1430", 345.0f);
			angles.Add("1445", 352.5f);
			angles.Add("1500", 0.0f);
			angles.Add("1515", 7.5f);
			angles.Add("1530", 15.0f);
			angles.Add("1545", 22.5f);
			angles.Add("1600", 30.0f);
			angles.Add("1615", 37.5f);
			angles.Add("1630", 45.0f);
			angles.Add("1645", 52.5f);
			angles.Add("1700", 60.0f);
			angles.Add("1715", 67.5f);
			angles.Add("1730", 75.0f);
			angles.Add("1745", 82.5f);
			angles.Add("1800", 90.0f);
			angles.Add("1815", 97.5f);
			angles.Add("1830", 105.0f);
			angles.Add("1845", 112.5f);
			angles.Add("1900", 120.0f);
			angles.Add("1915", 127.5f);
			angles.Add("1930", 135.0f);
			angles.Add("1945", 142.5f);
			angles.Add("2000", 150.0f);
			angles.Add("2015", 157.5f);
			angles.Add("2030", 165.0f);
			angles.Add("2045", 172.5f);
			angles.Add("2100", 180.0f);
			angles.Add("2115", 187.5f);
			angles.Add("2130", 195.0f);
			angles.Add("2145", 202.5f);
			angles.Add("2200", 210.0f);
			angles.Add("2215", 217.5f);
			angles.Add("2230", 225.0f);
			angles.Add("2245", 232.5f);
			angles.Add("2300", 240.0f);
			angles.Add("2315", 247.5f);
			angles.Add("2330", 255.0f);
			angles.Add("2345", 262.5f);
		}

		void initializeColorsDictionary() {
			colors = new Dictionary<AreaType, Color>();
			colors.Add(AreaType.Work, Color.Blue);
			colors.Add(AreaType.Leisure, Color.LimeGreen);
			colors.Add(AreaType.Community, Color.Violet);
			colors.Add(AreaType.Health, Color.Coral);
			colors.Add(AreaType.Improvement, Color.Black);
			colors.Add(AreaType.Relationships, Color.MistyRose);
			colors.Add(AreaType.Unproductive, Color.Red);
		}

		TimeData CreateTimeDataFromString(string line) {
			var details = line.Split("%-".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
			return new TimeData {
				StartTime = details[0].Trim(),
				Duration = Int32.Parse(details[1]),
				Focus = (FocusLevel)Int32.Parse(details[2]),
				Area = GetAreaFrom(details[3]),
				Description = details.Length >4 ? details[4] : ""
			};
		}

		AreaType GetAreaFrom(string str) {
			switch (str.ToUpper()) {
				case "Р":
					return AreaType.Work;
				case "Д":
					return AreaType.Leisure;
				case "О":
					return AreaType.Relationships;
				case "У":
					return AreaType.Improvement;
				case "С":
					return AreaType.Community;
				case "З":
					return AreaType.Health;
				default:
					return AreaType.Unproductive;
			}
		}
		
		#region IDisposable implementation

		public void Dispose() {
			myImage.Dispose();
			go.Dispose();	
		}

		#endregion
	}
	
	public struct TimeData {
		public string StartTime;
		public int Duration;
		public FocusLevel Focus;
		public string Description;
		public AreaType Area;
	}

	public enum AreaType {
		Work = 0,
		Leisure = 1,
		Health = 2,
		Improvement = 3,
		Relationships = 4,
		Community = 5,
		Unproductive = 6
	}
	
	public enum FocusLevel {
		Low = 1,
		Normal = 2,
		High = 3
	}
}

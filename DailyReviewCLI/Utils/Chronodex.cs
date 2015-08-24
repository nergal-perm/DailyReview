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
	public static class Chronodex {
		# region Static Dictionaryies and fields
		static Dictionary<string, float> angles = new Dictionary<string, float>() {
			{"0000", 270.0f},
			{"0015", 277.5f},
			{"0030", 285.0f},
			{"0045", 292.5f},
			{"0100", 300.0f},
			{"0115", 307.5f},
			{"0130", 315.0f},
			{"0145", 322.5f},
			{"0200", 330.0f},
			{"0215", 337.5f},
			{"0230", 345.0f},
			{"0245", 352.5f},
			{"0300", 0.0f},
			{"0315", 7.5f},
			{"0330", 15.0f},
			{"0345", 22.5f},
			{"0400", 30.0f},
			{"0415", 37.5f},
			{"0430", 45.0f},
			{"0445", 52.5f},
			{"0500", 60.0f},
			{"0515", 67.5f},
			{"0530", 75.0f},
			{"0545", 82.5f},
			{"0600", 90.0f},
			{"0615", 97.5f},
			{"0630", 105.0f},
			{"0645", 112.5f},
			{"0700", 120.0f},
			{"0715", 127.5f},
			{"0730", 135.0f},
			{"0745", 142.5f},
			{"0800", 150.0f},
			{"0815", 157.5f},
			{"0830", 165.0f},
			{"0845", 172.5f},
			{"0900", 180.0f},
			{"0915", 187.5f},
			{"0930", 195.0f},
			{"0945", 202.5f},
			{"1000", 210.0f},
			{"1015", 217.5f},
			{"1030", 225.0f},
			{"1045", 232.5f},
			{"1100", 240.0f},
			{"1115", 247.5f},
			{"1130", 255.0f},
			{"1145", 262.5f},
			{"1200", 270.0f},
			{"1215", 277.5f},
			{"1230", 285.0f},
			{"1245", 292.5f},
			{"1300", 300.0f},
			{"1315", 307.5f},
			{"1330", 315.0f},
			{"1345", 322.5f},
			{"1400", 330.0f},
			{"1415", 337.5f},
			{"1430", 345.0f},
			{"1445", 352.5f},
			{"1500", 0.0f},
			{"1515", 7.5f},
			{"1530", 15.0f},
			{"1545", 22.5f},
			{"1600", 30.0f},
			{"1615", 37.5f},
			{"1630", 45.0f},
			{"1645", 52.5f},
			{"1700", 60.0f},
			{"1715", 67.5f},
			{"1730", 75.0f},
			{"1745", 82.5f},
			{"1800", 90.0f},
			{"1815", 97.5f},
			{"1830", 105.0f},
			{"1845", 112.5f},
			{"1900", 120.0f},
			{"1915", 127.5f},
			{"1930", 135.0f},
			{"1945", 142.5f},
			{"2000", 150.0f},
			{"2015", 157.5f},
			{"2030", 165.0f},
			{"2045", 172.5f},
			{"2100", 180.0f},
			{"2115", 187.5f},
			{"2130", 195.0f},
			{"2145", 202.5f},
			{"2200", 210.0f},
			{"2215", 217.5f},
			{"2230", 225.0f},
			{"2245", 232.5f},
			{"2300", 240.0f},
			{"2315", 247.5f},
			{"2330", 255.0f},
			{"2345", 262.5f}			
		};
		
		static Rectangle[] levels = {
			new Rectangle(471, 365, 288, 288),
			new Rectangle(415, 309, 400, 400),
			new Rectangle(355, 249, 519, 520),
			new Rectangle(298, 192, 634, 634)
		};
		
		static Dictionary<AreaType, Color> colors = new Dictionary<AreaType, Color>() {
			{AreaType.Work, Color.Blue},
			{AreaType.Leisure, Color.LimeGreen},
			{AreaType.Community, Color.Orange},
			{AreaType.Health, Color.DarkViolet},
			{AreaType.Improvement, Color.Black},
			{AreaType.Relationships, Color.Fuchsia},
			{AreaType.Unproductive, Color.Red}
		};		
		
		#endregion
		
		private static void DrawChronodexSector(ChronodexSector cs, Graphics go) {
			
			GraphicsPath gp = new GraphicsPath();
			
			gp.AddArc(levels[(int)cs.Focus], angles[cs.StartTime], cs.Duration * 7.5f);
			gp.AddArc(levels[0], angles[cs.StartTime], cs.Duration * 7.5f);
			gp.AddLine(gp.PathPoints[0], gp.PathPoints[4]);
			gp.AddLine(gp.PathPoints[3], gp.PathPoints[7]);
			
			HatchBrush brush = new HatchBrush(HatchStyle.DarkDownwardDiagonal, colors[cs.Area], Color.Transparent);
			go.FillPath(brush, gp);
			gp.Dispose();			
		}

		public static Image CreateChronodex(ChronodexList cl) {
			
			Assembly myAssembly = Assembly.GetExecutingAssembly();
			Stream myStream = myAssembly.GetManifestResourceStream("Chronodex");
			Image myImage = Image.FromStream(myStream);
			myStream.Close();
			Graphics go = Graphics.FromImage(myImage);			
			
			ChronodexSector cs = cl.getFirst();
			DrawChronodexSector(cs, go);
			
			while (cl.getNext() != cs) {
				cl.moveForward();
				DrawChronodexSector(cl.getCurrent(), go);
			}
			
			return myImage;
		}

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

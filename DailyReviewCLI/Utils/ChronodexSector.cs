/*
 * Created by SharpDevelop.
 * User: terekhov-ev
 * Date: 24.08.2015
 * Time: 10:21
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace DailyReviewCLI.Utils {
	/// <summary>
	/// Description of ChronodexSector.
	/// </summary>
	public class ChronodexSector {
		string _startTime;
		int _duration;
		FocusLevel _focus;
		AreaType _area;
		string _description;
		
		public ChronodexSector() {
		}

		public ChronodexSector(string[] timeData) {
			_startTime = timeData[0].Trim();
			_duration = Int32.Parse(timeData[1]);
			_focus = (FocusLevel)Int32.Parse(timeData[2]);
			_area = GetAreaFrom(timeData[3]);
			_description = timeData.Length > 4 ? timeData[4] : "";
		}
		
		public string StartTime {
			get {return _startTime;}
		}
		
		public int Duration {
			get { return _duration; }
		}
		
		public FocusLevel Focus {
			get { return _focus; }
		}
		
		public AreaType Area {
			get { return _area; }
		}
		
		public string Description {
			get { return _description; }
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
	}
}

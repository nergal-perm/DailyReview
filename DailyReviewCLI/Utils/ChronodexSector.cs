/*
 * Created by SharpDevelop.
 * User: terekhov-ev
 * Date: 24.08.2015
 * Time: 10:21
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

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
		
		float _angleBegin, _angleEnd, _angleMiddle;
		PointF _centralPoint;
		Color _color;
		
		RectangleF _labelRectangle;
		HatchStyle _hatchType;
		PointF _calloutEnd;
		
		bool _layedOut;
		
		public ChronodexSector(string[] timeData) {
			// basic fields
			_startTime = timeData[0].Trim();
			_duration = Int32.Parse(timeData[1]);
			_focus = (FocusLevel)Int32.Parse(timeData[2]);
			_area = GetAreaFrom(timeData[3]);
			_description = timeData.Length > 4 ? String.Join("-", timeData.Skip(4)) : "";
			_layedOut = false;
			
			// derivative fields
			_angleBegin = Chronodex.angles[_startTime];
			_angleEnd = _angleBegin + _duration * 7.5f;
			_angleMiddle = (_angleBegin + _angleEnd) / 2;
			_centralPoint = new PointF(615.0f + (float)Math.Cos(_angleMiddle * Math.PI / 180) * (144 + (Int32)_focus * 30), 
				509.0f + (float)Math.Sin(_angleMiddle * Math.PI / 180) * (144 + (Int32)_focus * 30));
			_calloutEnd = new PointF(615.0f + (float)Math.Cos(_angleMiddle * Math.PI / 180) * 380, 
				509.0f + (float)Math.Sin(_angleMiddle * Math.PI / 180) * 380);
			_color = Chronodex.colors[_area];
		}

		public void LayoutRespecting(Graphics go, RectangleF labelRectangle) {
			Font stringFont = new Font("Monoid", 12);
			SizeF stringSize = new SizeF();
			int maxWidth = (int)Math.Min(_calloutEnd.X, (1200-_calloutEnd.X));
			int i = 0;
			
			do {
				stringSize = go.MeasureString(_description, stringFont, maxWidth);

				// adjust callout line length
				_calloutEnd = new PointF(615.0f + (float)Math.Cos((_angleMiddle + i * 3.75f) * Math.PI / 180) * 380 ,
				509.0f + (float)Math.Sin((_angleMiddle + i * 3.75f) * Math.PI / 180) * 380 );
				
				// adjust label rectangle dimensions
				PointF calloutPosition = new PointF();
				if (_angleMiddle > 90 && _angleMiddle <= 270) {
					calloutPosition.X = _calloutEnd.X - stringSize.Width;
					maxWidth = (int)_calloutEnd.X;
				} else {
					calloutPosition.X = _calloutEnd.X;
					maxWidth = (int)(1200 - _calloutEnd.X);
				}
				calloutPosition.Y = _calloutEnd.Y - stringSize.Height;
			
				_labelRectangle = new RectangleF(calloutPosition, stringSize);
				
				i += 1;
			} while (_labelRectangle.IntersectsWith(labelRectangle));
			
			_layedOut = true;
		}
		public string StartTime {
			get { return _startTime; }
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
		
		public HatchStyle HatchType {
			get { return _hatchType; }
			set { _hatchType = value; }
		}
		
		public RectangleF LabelRectangle {
			get { return _labelRectangle; }
			set { _labelRectangle = value; }
		}
		
		public PointF CentralPoint {
			get { return _centralPoint; }
		}
		
		public PointF CalloutEnd {
			get { return _calloutEnd; }
			set { _calloutEnd = value; }
		}

		public float MiddleAngle {
			get { return _angleMiddle; }
		}
		
		public bool LayedOut {
			get { return _layedOut; }
			set { _layedOut = value; }
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

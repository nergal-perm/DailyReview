/*
 * Created by SharpDevelop.
 * User: terekhov-ev
 * Date: 24.08.2015
 * Time: 10:04
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace DailyReviewCLI.Utils {
	/// <summary>
	/// Description of ChronodexList.
	/// </summary>
	public class ChronodexList {
		string[] _dayData;
		readonly LinkedList<ChronodexSector> _sectors;
		LinkedListNode<ChronodexSector> _currentNode;
		Dictionary<string, ChronodexSector> _descriptions;
		
		public ChronodexList(string[] dayData) {
			_sectors = new LinkedList<ChronodexSector>();
			Array.Sort(dayData);
			_dayData = dayData;
			foreach (string line in _dayData) {
				string[] timeData = line.Split("%-".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
				_sectors.AddLast(new ChronodexSector(timeData));
			}
			_currentNode = _sectors.First;
			_descriptions = new Dictionary<string, ChronodexSector>();
		}
		
		public Dictionary<string, ChronodexSector> Descriptions {
			get { return _descriptions; }
		}
		
		public int getSectorsCount() {
			return _sectors.Count;
		}

		public ChronodexSector getFirst() {
			return _sectors.First.Value;
		}

		public ChronodexSector getNext() {
			return _currentNode.Next == null ? _sectors.First.Value : _currentNode.Next.Value;
		}

		public ChronodexSector getPrevious() {
			return _currentNode.Previous == null ? _sectors.Last.Value : _currentNode.Previous.Value;
		}
		public void moveForward() {
			_currentNode = _currentNode.Next ?? _sectors.First;
		}
		
		public ChronodexSector getCurrent() {
			return _currentNode.Value;
		}

		public ChronodexSector findLayoutStartingSector() {
//			_currentNode = (ChronodexSector)_sectors.Nodes().FirstOrDefault(n => n.Value.MiddleAngle == _sectors.Nodes().Min(m => m.Value.MiddleAngle));
//			return _currentNode.Value;
			_currentNode = _sectors.First;
			return _sectors.First.Value;
			
		}
		

	}
}

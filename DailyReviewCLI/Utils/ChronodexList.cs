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

namespace DailyReviewCLI.Utils {
	/// <summary>
	/// Description of ChronodexList.
	/// </summary>
	public class ChronodexList {
		string[] _dayData;
		readonly LinkedList<ChronodexSector> _sectors;
		LinkedListNode<ChronodexSector> _currentNode;
		
		public ChronodexList(string[] dayData) {
			_sectors = new LinkedList<ChronodexSector>();
			Array.Sort(dayData);
			_dayData = dayData;
			foreach (string line in _dayData) {
				string[] timeData = line.Split("%-".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
				_sectors.AddLast(new ChronodexSector(timeData));
			}
			_currentNode = _sectors.First;
		}
		
		public int getSectorsCount() {
			return _sectors.Count;
		}

		public ChronodexSector getFirst() {
			return _sectors.First.Value;
		}

		public ChronodexSector getNext() {
			if (_currentNode.Next == null)
				return _sectors.First.Value;
			return _currentNode.Next.Value;
		}
		
		public void moveForward() {
			_currentNode = _currentNode.Next;
		}
		
		public ChronodexSector getCurrent() {
			return _currentNode.Value;
		}
	}
}

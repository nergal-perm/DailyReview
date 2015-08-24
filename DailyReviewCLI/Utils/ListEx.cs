/*
 * Created by SharpDevelop.
 * User: terekhov-ev
 * Date: 24.08.2015
 * Time: 13:47
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace DailyReviewCLI.Utils
{
	/// <summary>
	/// Description of ListEx.
	/// </summary>
	public static class ListEx
	{
		public static IEnumerable<LinkedListNode<T>> Nodes<T>(this LinkedList<T> list) {
			for (var node = list.First; node != null; node = node.Next) {
				yield return node;
			}
		}
	}
}

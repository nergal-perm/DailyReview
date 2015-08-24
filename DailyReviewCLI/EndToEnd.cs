/*
 * Created by SharpDevelop.
 * User: terekhov-ev
 * Date: 24.08.2015
 * Time: 9:59
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using NUnit.Framework;
using DailyReviewCLI.Utils;

namespace DailyReviewCLI {
	[TestFixture]
	public class EndToEnd {
		string[] dayData_simple;
		string[] dayData_complex;
		
		[SetUp]
		public void initializeContext() {
			dayData_simple = new[] {
				"% 0800-1-3-н-всякая фигня за компом",
				"% 0815-1-1-у-итоги выходных",
				"% 0830-2-2-р-реализация выбора отчета в redmine:excel",
				"% 0900-3-2-у-план дня / недели, итоги"
			};
			
			dayData_complex = new[] {
				"% 1000-1-3-н-кофе",
				"% 0815-1-1-у-итоги выходных",
				"% 0830-2-2-р-реализация выбора отчета в redmine:excel",
				"% 0900-3-2-у-план дня / недели, итоги",
				"% 0945-4-3-д-реализация связного списка Хронодекс"
			};
		}
		
		[Test]
		public void shouldCreateChronodexListFromTextTime() {
			ChronodexList cl = new ChronodexList(dayData_simple);
			Assert.AreEqual(expected: 4,
				actual: cl.getSectorsCount());
			
			cl = new ChronodexList(dayData_complex);
			Assert.AreEqual(expected: 5,
				actual: cl.getSectorsCount());			
		}
		
		[Test]
		public void shouldGetFirstChronodexSector() {
			ChronodexList cl = new ChronodexList(dayData_simple);
			Assert.AreEqual(expected: "0800",
			                actual: cl.getFirst().StartTime);

			cl = new ChronodexList(dayData_complex);
			Assert.AreEqual(expected: "0815",
				actual: cl.getFirst().StartTime);			
		}
	}
}

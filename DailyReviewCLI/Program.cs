/*
 * Created by SharpDevelop.
 * User: terekhov-ev
 * Date: 11.08.2015
 * Time: 11:43
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;

namespace DailyReviewCLI {
	class Program {
		private const string TODO_DIR = @"c:\users\terekhov-ev\dropbox\todo\";
		
		public static void Main(string[] args) {
			// TODO: Разобрать аргументы командной строки
			// TODO: Установить контекст (прочитать / обновить свойства)
			StringDictionary context = Utils.CliParser.parse(args);
			// TODO: Выполнить нужную команду
			Console.WriteLine("Hello World!");
			
			// Прототип идеи
			//CreateMarkdownNote();
			foreach (string key in context.Keys) {
				Console.WriteLine("{0,-10}:\t{1}",key, context[key]);
			}
			
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
		
		
		private static void CreateMarkdownNote() {
			// Представить текущую дату в виде yyy-mm-dd
			string curDate = String.Format("{0:0000}-{1:00}-{2:00}", DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
			// Получить из файла todo.txt список задач на указанную дату
			// пока без просроченных задач
			var tasks = GetTodoTxtTasksFor(curDate);
			foreach (var task in tasks) {
				Console.WriteLine(task);
			}
			// Создать markdown-заметку в папке Dropbox
			CreateFilledNoteWith(tasks, curDate);
			
		}
		
		private static string[] GetTodoTxtTasksFor(string curDate) {
			var lines = File.ReadAllLines(TODO_DIR + "todo.txt").Select(l=>l.Trim()).Where(l=>l.Contains("due:" + curDate));
			return lines.ToArray();
		}
		
		private static void CreateFilledNoteWith(string[] tasks, string curDate) {
			var lines =  new List<string>();
			lines.Add("# План на день:");
			foreach (var task in tasks) {
				lines.Add("- [ ] " + task.Trim());
			}
			lines.Add("");
			lines.Add("# Триста букв:");
			lines.Add("");
			lines.Add("");
			lines.Add("# События и результаты:");
			lines.Add("");
			lines.Add("");
			
			File.WriteAllLines(TODO_DIR + @"DayNotes\" + curDate + ".md", lines.ToArray());
		}
	}
}
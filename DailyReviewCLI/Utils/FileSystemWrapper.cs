/*
 * Created by SharpDevelop.
 * User: terekhov-ev
 * Date: 12.08.2015
 * Time: 13:37
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace DailyReviewCLI.Utils {
	/// <summary>
	/// Description of FileSystemWrapper.
	/// </summary>
	public class FileSystemWrapper {
		private DirectoryInfo _dropboxTodo;
		private readonly DirectoryInfo _markdownFolder;
		
		public FileSystemWrapper() {
			// Проверить наличие папки Dropbox и DayNotes, создать при необходимости
			 
			if (getDropboxFolder() == null) {
				_dropboxTodo = new DirectoryInfo(getCurrentFolderPath());
				_markdownFolder = new DirectoryInfo(getCurrentFolderPath() + @"\DayNotes\");
			} else {
				_dropboxTodo = new DirectoryInfo(getDropboxFolder().FullName + @"\todo\");
				_markdownFolder = new DirectoryInfo(getDropboxFolder().FullName + @"\todo\DayNotes\");
			}
			
			if (!_dropboxTodo.Exists) {
				_dropboxTodo.Create();
			}
			
		}

		#region Files, folders and paths
		public bool dayExists(string date) {
			return File.Exists(_markdownFolder.FullName + @"\" + date + ".md");
		}
		
		private string getCurrentFolderPath() {
			return Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
		}
		
		DirectoryInfo getDropboxFolder() {
			var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			var dbPath = Path.Combine(appDataPath, "Dropbox\\host.db");

			if (!File.Exists(dbPath)) {
				appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
				dbPath = Path.Combine(appDataPath, "Dropbox\\host.db");
				if (!File.Exists(dbPath))
					return null;
			}

			var lines = File.ReadAllLines(dbPath);
			var dbBase64Text = Convert.FromBase64String(lines[1]);
			var folderPath = Encoding.UTF8.GetString(dbBase64Text);

			return new DirectoryInfo(folderPath);
		}
		#endregion
		

		public string[] getTasksForDate(string curDate) {
			DateTime currentDate = getDateFromString(curDate);
			DateTime taskDate;
			var lines = File.ReadAllLines(getDropboxFolder() + @"\todo\todo.txt").Select(l => l.Trim()).Where(l => l.Contains("due:"));
			var activeTasks = new List<string>();
			
			foreach (var line in lines) {
				string tDate = line.Substring(line.IndexOf("due:", StringComparison.CurrentCulture) + 4, 10);
				taskDate = getDateFromString(tDate);
				if (taskDate <= currentDate)
					activeTasks.Add(line);
			}
			activeTasks.Sort();
			return activeTasks.ToArray();
		}
		
		public void WriteToMarkdown(string curDate) {
			if (dayExists(curDate))
				return;
			
			string[] tasks = getTasksForDate(curDate);
			string[] weather = getDateFromString(curDate) == DateTime.Today ? WebServices.getForecast() : WebServices.getHistoryFor(curDate);
			string[] lines = CreateFilledNoteWith(tasks, weather, curDate);
			
			File.WriteAllLines(_markdownFolder.FullName + @"\" + curDate + ".md", lines);
			Console.WriteLine("Successfully written {0}.md", curDate);
		}
		
		private string[] CreateFilledNoteWith(string[] tasks, string[] weather, string curDate) {
			var lines = new List<string>();
			lines.Add("# План на день:");
			foreach (var task in tasks) {
				lines.Add("[ ] " + task.Trim());
			}
			lines.AddRange(new [] {"---", "# Под чертой:", "", ""});
			lines.AddRange(weather);
			lines.AddRange(new [] {
				"# Триста букв:", "", "",
				"# События и результаты:", "", ""
			});
			
			return lines.ToArray();
		}

		public void WriteToHtml(string curDate) {
			string[] mdLines = File.ReadAllLines(_markdownFolder.FullName + @"\" + curDate + ".md");
			string[] htmlLines = ConvertMd2Html(mdLines);
			
			List<string> enexNote = new List<string>();
			
			enexNote.AddRange(getEnexNoteHeader(curDate));
			enexNote.AddRange(htmlLines);
			enexNote.AddRange(getEnexNoteFooter(curDate));
			
			File.WriteAllLines(_markdownFolder.FullName + @"\" + curDate + ".enex", enexNote.ToArray());
		}

		private string[] getEnexNoteHeader(string curDate) {
			List<string> lines = new List<string>();
			
			lines.AddRange(new [] {"<?xml version=\"1.0\" encoding=\"UTF-8\"?>",
			       "<!DOCTYPE en-export SYSTEM \"http://xml.evernote.com/pub/evernote-export2.dtd\">",
			       "<en-export export-date=\"20150813T042235Z\" application=\"Evernote/Windows\" version=\"5.x\">",
			       String.Format("<note><title>{0} - {1} - обзор дня</title><content>", curDate, getDayAbbr(curDate)),
			       "<![CDATA[<?xml version=\"1.0\" encoding=\"UTF-8\"?><!DOCTYPE en-note SYSTEM \"http://xml.evernote.com/pub/enml2.dtd\">",
			       "<en-note style=\"word-wrap: break-word; -webkit-nbsp-mode: space; -webkit-line-break: after-white-space;\">"});
			
			return lines.ToArray();
			       
		}
		
		private string[] getEnexNoteFooter(string curDate) {
			List<string> lines = new List<string>();
			
			lines.AddRange(new [] {String.Format("</en-note>]]></content><created>{0}</created>", getTimeStampFor(curDate)),
			               	"<tag>план</tag><tag>день</tag><tag>обзор</tag><tag>триста_букв</tag>",
			               	"<note-attributes><author>Евгений Терехов</author></note-attributes></note></en-export>"
			               });
			
			return lines.ToArray();
		}
		
		private string getTimeStampFor(string curDate) {
			return curDate.Replace("-","") + "T185959Z";
		}
		
		private string getDayAbbr(string curDate) {
			DateTime dt = getDateFromString(curDate);
			
			string[] dayNames = {"Вс","Пн","Вт","Ср","Чт","Пт","Сб"};
			
			return dayNames[(int)dt.DayOfWeek];
		}

		
		private string[] ConvertMd2Html(string[] mdLines) {
			var htmlLines = new List<string>();
			bool isInList = false;
			
			foreach (string iter in mdLines) {
				string mdLine = iter;
				if (isInList && !mdLine.StartsWith("* ", StringComparison.CurrentCulture)) {
					isInList = false;
					htmlLines.Add("</ul>");
				}
	
//				if (mdLine.Contains("](")) {
//					mdLine = replaceHyperLink(mdLine);
//				}
				
				if (mdLine == "") {
					htmlLines.Add(isInList ? "</ul>" : "<br/>");
					isInList = false;
				} else if (mdLine.StartsWith("# ", StringComparison.CurrentCulture)) {
					htmlLines.Add(String.Format("<div><b style=\"background-color:rgb(255, 250, 165);-evernote-highlight:true;\">{0}</b></div>", 
						mdLine.Replace("# ", "")));
				} else if (mdLine.StartsWith("## ", StringComparison.CurrentCulture)) {
					htmlLines.Add(String.Format("<div><b>{0}</b></div>", 
						mdLine.Replace("## ", "")));
					
				} else if (mdLine.StartsWith("[", StringComparison.CurrentCulture)) {
					htmlLines.Add(getColoredTaskString(mdLine));
				} 
				else if (mdLine.StartsWith("* ", StringComparison.CurrentCulture)) {
					if (!isInList) {
						isInList = true;
						htmlLines.Add("<ul>");
					}
					htmlLines.Add(String.Format("<li>{0}</li>", mdLine.Replace("* ","")));
				}
				else if (mdLine.StartsWith("---", StringComparison.CurrentCulture)) {
					htmlLines.Add("<hr/>");
				} else {
					htmlLines.Add(String.Format("<div>{0}</div>", mdLine));
				}
			}
			
			return htmlLines.ToArray();
		}
		
		private string getColoredTaskString(string task) {
			string colorHex;
			switch (task[5].ToString()) {
				case "A":
					colorHex = "#E30000";
					break;
				case "B":
					colorHex = "#2D4FC9";
					break;
				case "C":
					colorHex = "#41AD1C";
					break;
				case "D":
					colorHex = "797979";
					break;
				default:
					colorHex = "#000000";
					break;
			}
			
			return String.Format("<div><font color=\"{0}\"><en-todo checked=\"{1}\"/>{2}</font></div>", 
			                     colorHex, task[1].ToString() == "x" ? "true" : "false", task.Replace("[x] ", "").Replace("[ ] ",""));
		}

		private string replaceHyperLink(string line) {
			string[] linkParts = line.Split(new [] {"]("}, StringSplitOptions.RemoveEmptyEntries);
			
			return String.Format("<a href=\"{0}\" style=\"color: rgb(105, 170, 53);\">{1}</a>",
			                     linkParts[1].Split(")"[0])[0],
			                     linkParts[0].Split("["[0])[linkParts[0].Split("["[0]).Length-1]);
		}
		
		private DateTime getDateFromString(string curDate) {
		DateTime result;
		
			if (!DateTime.TryParseExact(curDate, "yyyy-MM-dd",
				    new CultureInfo("ru-RU"),
				    DateTimeStyles.None,
				    out result)) {
				throw new ArgumentException("Неверная дата {0}", curDate);
			}		
		return result;
	}

	}

}

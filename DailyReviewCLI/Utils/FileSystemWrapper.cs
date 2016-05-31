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
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using Microsoft.Win32;

namespace DailyReviewCLI.Utils {
	/// <summary>
	/// Description of FileSystemWrapper.
	/// </summary>
	public class FileSystemWrapper {
		private DirectoryInfo _dropboxTodo;
		private readonly DirectoryInfo _markdownFolder;
		private int[] plan = { 0, 0, 0, 0 };
		private int[] fact = { 0, 0, 0, 0 };
		private Dictionary<char, int> priorities = new Dictionary<char, int>();	
		private string productivityData = "";
		private List<string> imageResources = new List<string>();

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
			
			priorities.Add('A', 0);
			priorities.Add('B', 1);
			priorities.Add('C', 2);
			priorities.Add('D', 3);			
		}

		#region Files, folders and paths
		public bool dayExists(string date) {
			return File.Exists(_markdownFolder.FullName + @"\" + date + ".md");
		}

		private string getCurrentFolderPath() {
			return Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
		}

		DirectoryInfo getDropboxFolder() {
			return new DirectoryInfo(ConfigurationManager.AppSettings.Get("DropboxFolder"));
			var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			var dbPath = Path.Combine(appDataPath, "Dropbox\\host.db");

			if (!File.Exists(dbPath)) {
				appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
				dbPath = Path.Combine(appDataPath, "Dropbox\\host.db");
				if (!File.Exists(dbPath))
					return new DirectoryInfo(ConfigurationManager.AppSettings.Get("DropboxFolder"));
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

	    public string GetFirstOpenDay() {        
			DateTime minDate = DateTime.MaxValue;
			foreach (var mdFile in _markdownFolder.GetFiles("*.md")) {
				var fileDate = getDateFromString(mdFile.Name.Replace(mdFile.Extension, "").Trim(".".ToCharArray()));
				//Debug only
				//Console.Write("file date: {0:0000}-{1:00}-{2:00}, ", fileDate.Year, fileDate.Month, fileDate.Day);
				//Console.WriteLine("min date: {0:0000}-{1:00}-{2:00}", minDate.Year, minDate.Month, minDate.Day);
				minDate = fileDate < minDate ? fileDate : minDate;
			}
			return String.Format("{0:0000}-{1:00}-{2:00}", minDate.Year, minDate.Month, minDate.Day);
	    }

        public string[] getTimeData(string date) {
			return File.ReadAllLines(_markdownFolder.FullName + @"\" + date + ".md").Select(l => l.Trim()).Where(l => l.StartsWith("%", 	StringComparison.CurrentCulture)).ToArray();
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
			lines.AddRange(new [] {"# Триста букв:", "", "" });
			lines.AddRange(new [] {
				"# Питание:",
				"* З - ",
				"* О - ",
				"* У - ",
				"* П - ",
				""
			});
			lines.AddRange(new [] {
				"# План на день:",
				"[ ] (A)",
				"[ ] (B)",
				"[ ] (B)",
				"[ ] (B)",
				"[ ] (C)",
				"[ ] (C)",
				"[ ] (C)",
				"[ ] (C)",
				"[ ] (C)"
			});
			
//			foreach (var task in tasks) {
//				lines.Add("[ ] " + task.Trim());
//			}
			lines.AddRange(new [] { "---", "", "## Исполнение плана: {plan}", "## {productivity}", "" });
			lines.AddRange(weather);
			lines.AddRange(new [] {
				"# События и результаты:", "", "",
				"# Хронодекс:", ""
			});

			return lines.ToArray();
		}

		public void WriteToHtml(string curDate) {
			productivityData = WebServices.getProductivityData(curDate);
			string[] mdLines = File.ReadAllLines(_markdownFolder.FullName + @"\" + curDate + ".md");
			string[] htmlLines = ConvertMd2Html(mdLines);

			List<string> enexNote = new List<string>();

			enexNote.AddRange(getEnexNoteHeader(curDate));
			enexNote.AddRange(htmlLines);
            
			// Добавляем картинку с Хронодексом
			if (File.Exists(_markdownFolder.FullName + @"\" + curDate + ".png")) { 
				enexNote.Add("<en-media hash=\"" + getImageMediaHash(curDate) + "\" style=\"cursor: default; height: auto;\" type=\"image/png\"/>");
			}

			enexNote.AddRange(getEnexNoteFooter(curDate));

			File.WriteAllLines(_markdownFolder.FullName + @"\" + curDate + ".enex", enexNote.ToArray());
			
		}
		
		private string[] getEnexNoteHeader(string curDate) {
			List<string> lines = new List<string>();

			lines.AddRange(new [] {
				"<?xml version=\"1.0\" encoding=\"UTF-8\"?>",
				"<!DOCTYPE en-export SYSTEM \"http://xml.evernote.com/pub/evernote-export2.dtd\">",
				"<en-export export-date=\"20150813T042235Z\" application=\"Evernote/Windows\" version=\"5.x\">",
				String.Format("<note><title>{0} - {1} - обзор дня</title><content>", curDate, getDayAbbr(curDate)),
				"<![CDATA[<?xml version=\"1.0\" encoding=\"UTF-8\"?><!DOCTYPE en-note SYSTEM \"http://xml.evernote.com/pub/enml2.dtd\">",
				"<en-note style=\"word-wrap: break-word; -webkit-nbsp-mode: space; -webkit-line-break: after-white-space;\">"
			});

			return lines.ToArray();

		}

		private string[] getEnexNoteFooter(string curDate) {
			List<string> lines = new List<string>();

			lines.AddRange(new [] {String.Format("</en-note>]]></content><created>{0}</created>", getTimeStampFor(curDate)),
				"<tag>план</tag><tag>день</tag><tag>обзор</tag><tag>триста_букв</tag>",
				"<note-attributes><author>Евгений Терехов</author></note-attributes>"
			});
			
			if (File.Exists(_markdownFolder.FullName + @"\" + curDate + ".png")) {
				lines.AddRange(new [] {
					"<resource><data encoding=\"base64\">",
					getImageFileResource(curDate), "</data><mime>image/png</mime><width>1200</width><height>1000</height></resource>"
				});
			}
			foreach (var resource in imageResources) {
				lines.AddRange(new [] {
					"<resource><data encoding=\"base64\">",
					resource, "</data><mime>image/png</mime><width>50</width><height>50</height></resource>"
				});
			}
			
			lines.Add("</note></en-export>");

			return lines.ToArray();
		}

		private string getImageMediaHash(string curDate) {
			using (var md5 = MD5.Create()) {
				using (var stream = File.OpenRead(_markdownFolder.FullName + @"\" + curDate + ".png")) {
					byte[] b = md5.ComputeHash(stream);
					stream.Close();
					return BitConverter.ToString(b).Replace("-", "").ToLower();
				}
			}
        }
		
		private string getImageLinkMediaHash(string sUrl) {
			using (var md5 = MD5.Create()) {
				var resource = WebServices.getResourceByUrl(sUrl);
				using (var stream = new MemoryStream(resource)) {
					byte[] b = md5.ComputeHash(stream);
					stream.Close();
					imageResources.Add(Convert.ToBase64String(resource));
					return BitConverter.ToString(b).Replace("-", "").ToLower();
				}
			}			
		}

		private string getImageFileResource(string curDate) {
            byte[] file = File.ReadAllBytes(_markdownFolder.FullName + @"\" + curDate + ".png");
            return Convert.ToBase64String(file);
        }
		
		private string getTimeStampFor(string curDate) {
			return curDate.Replace("-", "") + "T185959Z";
		}

		private string getDayAbbr(string curDate) {
			DateTime dt = getDateFromString(curDate);

			string[] dayNames = { "Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб" };

			return dayNames[(int)dt.DayOfWeek];
		}


		private string[] ConvertMd2Html(string[] mdLines) {
			var htmlLines = new List<string>();
			bool isInList = false;

			foreach (string iter in mdLines) {
				string mdLine = iter.Replace("<", "&lt;").Replace(">", "&gt;");;
				if (isInList && !mdLine.StartsWith("* ", StringComparison.CurrentCulture)) {
					isInList = false;
					htmlLines.Add("</ul>");
				}

				if (mdLine == "") {
					htmlLines.Add(isInList ? "</ul>" : "<br/>");
					isInList = false;
				} else if (mdLine.StartsWith("# ", StringComparison.CurrentCulture)) {
					htmlLines.Add(String.Format("<div><b style=\"background-color:rgb(255, 250, 165);-evernote-highlight:true;\">{0}</b></div>",
						mdLine.Replace("# ", "")));
				} else if (mdLine.StartsWith("## ", StringComparison.CurrentCulture)) {
					htmlLines.Add(String.Format("<div><b>{0}</b></div>",
					                            mdLine.Replace("## ", "")
					                            .Replace("{plan}", string.Format("{0,3:P2}", getPlanPercentage()))
					                            .Replace("{productivity}", productivityData)));
				} else if (mdLine.StartsWith("[", StringComparison.CurrentCulture)) {
					htmlLines.Add(getColoredTaskString(mdLine));
					updatePlanPercentage(mdLine);
				} else if (mdLine.StartsWith("* ", StringComparison.CurrentCulture)) {
					if (!isInList) {
						isInList = true;
						htmlLines.Add("<ul>");
					}
					htmlLines.Add(String.Format("<li>{0}</li>", mdLine.Replace("* ", "")));
				} else if (mdLine.StartsWith("---", StringComparison.CurrentCulture)) {
                    htmlLines.Add("<hr/>");
                } else if (mdLine.StartsWith("% ", StringComparison.CurrentCulture)) {
					// do nothing
				} else if (mdLine.StartsWith("{", StringComparison.CurrentCulture)) {
					htmlLines.Add("<en-media hash=\"" + getImageLinkMediaHash(mdLine.Replace("{", "").Replace("}","")) + "\" style=\"cursor: default; height: auto;\" type=\"image/png\"/>");
				} else {
					htmlLines.Add(String.Format("<div>{0}</div>", mdLine));
				}
			}

			return htmlLines.ToArray();
		}

		private double getPlanPercentage() {
			double result = 0;
			if (plan[0] > 0) {
				result += (0.7 * fact[0] / plan[0]);
			}
			if (plan[1] > 0) {
				result += (0.2 * fact[1] / plan[1]);
			}
			if (plan[2] > 0) {
				if (plan[3] > 0) {
					result += (0.07 * fact[2] / plan[2]);
				} else {
					result += (0.1 * fact[2] / plan[2]);
				}
			}
			if (plan[3] > 0) {
				result += (0.03 * fact[3] / plan[3]);
			}
			return result;
		}
		
		private void updatePlanPercentage(string task) {

			if (!priorities.ContainsKey(task[5])) {
				plan[3]++;
				if (task[1].ToString() == "x")
					fact[3]++;
			} else {
				plan[priorities[task[5]]]++;
				if (task[1].ToString() == "x")
					fact[priorities[task[5]]]++;
			}
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
				colorHex, task[1].ToString() == "x" ? "true" : "false", task.Replace("[x] ", "").Replace("[ ] ", ""));
		}

		private string replaceHyperLink(string line) {
			string[] linkParts = line.Split(new [] { "](" }, StringSplitOptions.RemoveEmptyEntries);

			return String.Format("<a href=\"{0}\" style=\"color: rgb(105, 170, 53);\">{1}</a>",
				linkParts[1].Split(")"[0])[0],
				linkParts[0].Split("["[0])[linkParts[0].Split("["[0]).Length - 1]);
		}

		private DateTime getDateFromString(string curDate) {
			DateTime result;

			if (!DateTime.TryParseExact(curDate, "yyyy-MM-dd",
				    new CultureInfo("ru-RU"),
				    DateTimeStyles.None,
				    out result)) {
				throw new ArgumentException(String.Format("Неверная дата {0}", curDate));
			}
			return result;
		}

		public void SaveImage(Image chrImage, string curDate) {
			chrImage.Save(_markdownFolder.FullName + @"\" + curDate + ".png");
		}

		public void cleanUpFiles(string curDate) {
			string enscript;
			try {
				enscript = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\App Paths\ENScript.exe", "", "").ToString();
			} catch (Exception e) {
				try {
					enscript = Registry.GetValue(@"HKEY_CURRENT_MACHINE\Software\Microsoft\Windows\CurrentVersion\App Paths\ENScript.exe", "", "").ToString();
				} catch (Exception ex) {
					enscript = Registry.GetValue(@"HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\App Paths\ENScript.exe", "", "").ToString();
				}
			}
			
			string file = _markdownFolder.FullName + curDate + ".enex";
			ProcessStartInfo psi = new ProcessStartInfo(enscript,  @" importNotes /s " + file + @" /n " + ConfigurationManager.AppSettings.Get("EvernoteNotebook"));
			psi.UseShellExecute = false;
			Process p = Process.Start(psi);

			p.WaitForExit();

			File.Delete(_markdownFolder.FullName + @"\" + curDate + ".enex");
		}
		
		public void moveFilesForZim(string curDate) {
			string text = File.ReadAllText(_markdownFolder.FullName + @"\" + curDate + ".md");
			text = text.Replace("{plan}", string.Format("{0,3:P2}", getPlanPercentage()))
				.Replace("{productivity}", productivityData) 
				.Replace("[x]", "[*]")
				.Replace("[ ]", "[x]")
				.Replace("## ", "**")
				.Replace("#", "======");
			
			
			
			File.WriteAllText(_markdownFolder.FullName + @"\" + curDate + ".md", text);
			File.AppendAllText(_markdownFolder.FullName + @"\" + curDate + ".md", Environment.NewLine + @"{{../" + curDate + ".png}}");
			if (File.Exists(_markdownFolder.FullName + @"\" + curDate + ".png")) {
				File.Move(_markdownFolder.FullName + @"\" + curDate + ".png", _markdownFolder.FullName + @"\" + curDate.Substring(0, 4) + @"\" +
					curDate.Substring(5, 2) + @"\" + curDate + ".png");
			}
			File.Move(_markdownFolder.FullName + @"\" + curDate + ".md", _markdownFolder.FullName + @"\" + curDate.Substring(0,4) + @"\" +
			          curDate.Substring(5,2) + @"\" + curDate.Substring(8,2) + ".txt");
			
		}
	}

}

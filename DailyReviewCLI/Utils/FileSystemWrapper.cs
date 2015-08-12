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
		private DirectoryInfo _markdownFolder;
		
		public FileSystemWrapper() {
			// Проверить наличие папки Dropbox и DayNotes, создать при необходимости
			 
			if(getDropboxFolder() == null) {
				_dropboxTodo =new DirectoryInfo(getCurrentFolderPath());
				_markdownFolder = new DirectoryInfo(getCurrentFolderPath() + @"\DayNotes\");
			} else {
				_dropboxTodo = new DirectoryInfo(getDropboxFolder().FullName + @"\todo\");
				_markdownFolder = new DirectoryInfo(getDropboxFolder().FullName + @"\todo\DayNotes\");
			}
			
			if (!_dropboxTodo.Exists) {
				_dropboxTodo.Create();
			}
			
		}

		public bool dayExists(string date) {
			return File.Exists(_markdownFolder.FullName + @"\" + date + ".md");
		}

		public string[] getTasksForDate(string curDate) {
			DateTime currentDate;
			DateTime taskDate;
			if (!DateTime.TryParseExact(curDate, "yyyy-MM-dd",
				new CultureInfo("ru-RU"),
				DateTimeStyles.None,
				out currentDate)) {
				throw new ArgumentException("Неверная дата {0}", curDate);
			}
			var lines = File.ReadAllLines(getDropboxFolder() + @"\todo\todo.txt").Select(l=>l.Trim()).Where(l=>l.Contains("due:"));
			var activeTasks = new List<string>();
			
			foreach (var line in lines) {
				string tDate = line.Substring(line.IndexOf("due:", StringComparison.CurrentCulture) + 4,10);
				if (!DateTime.TryParseExact(tDate, "yyyy-MM-dd",
					new CultureInfo("ru-RU"),
					DateTimeStyles.None,
					out taskDate)) {
					throw new ArgumentException("Неверная дата {0}", curDate);
				}				
				if (taskDate <= currentDate) activeTasks.Add(line);
			}
			return activeTasks.ToArray();
		}
		
		private string getCurrentFolderPath() {
			return Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
		}
		
		DirectoryInfo getDropboxFolder() {
			var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			var dbPath = Path.Combine(appDataPath, "Dropbox\\host.db");

            if (!File.Exists(dbPath))
            {
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
	}
}

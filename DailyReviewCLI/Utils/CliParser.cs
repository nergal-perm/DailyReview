/*
 * User: terekhov-ev
 * Date: 11.08.2015 13:40
 * 
 * Класс, используемый для разбора аргументов командной строки и заполнения
 * словаря свойствами, актуальными для конкретного вызова приложения.
 * 
 */
using System;
using System.Collections.Specialized;
using System.Globalization;

namespace DailyReviewCLI.Utils {
	/// <summary>
	/// Статический класс, разбирающий аргументы командной строки
	/// </summary>
	public static class CliParser {
		private static StringDictionary context;
		public static StringDictionary parse(string[] args) {
			// Командная строка имеет вид:
			// 		команда [ключ [значение]]
			// Возможные ключи:
			//	-d		--date			Дата, за которую нужно создать / закрыть заметку в формате yyyy-mm-dd
			refresh();
			createDefaults();

			// Empty command line should result in help message
			if (args.Length == 0)
				return context;
			
			parseCommandName(args[0]);
			for (int i = 1; i < args.Length; i++) {
				if (args[i].StartsWith("-", StringComparison.CurrentCulture))
					parseKeyValue(args[i], i == args.Length - 1 ? "" : args[i + 1]);
			}
			
			return context;
		}

		private static void createDefaults() {
			//context["date"] = String.Format("{0:0000}-{1:00}-{2:00}", DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
			context["command"] = "help";
		}
		
		/// <summary>
		/// Разбирает пары "ключ-значение" и создает соответствующие записи в словаре свойств
		/// </summary>
		/// <param name="key">Значение ключа (-s или --long)</param>
		/// <param name="value">Следующий за ключом аргумент командной строки</param>
		private static void parseKeyValue(string key, string value) {
			switch (key) {
				case "-d":
				case "--date":
					if (isValidDate(value)) {
						context["date"] = value;	
					} else {
						context["command"] = "help";
						context.Remove("date");
					}
					break;
				default:
					
					break;
					
			}
		}

		
		/// <summary>
		/// Определяет конкретную команду и создает свойство command
		/// </summary>
		/// <param name="comName">Первый аргумент командной строки (всегда должна
		/// быть конкретная команда</param>
		private static void parseCommandName(string comName) {
			// Команда всегда должна быть первым аргументом
			switch (comName) {
				case "open":
					context["command"] = "open";
					context["date"] = String.Format("{0:0000}-{1:00}-{2:00}", DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
					break;
				case "close":
					context["command"] = "close";
					break;
				default:
					context["command"] = "help";
					break;
			}			
		}

		/// <summary>
		/// Проверяет корректность строкового представления даты в формате yyyy-MM-dd
		/// </summary>
		/// <param name="value">Строковое представление даты</param>
		/// <returns>true, если строка является корректной датой</returns>
		static bool isValidDate(string value) {
			DateTime dateValue;
			return DateTime.TryParseExact(value, "yyyy-MM-dd",
				new CultureInfo("ru-RU"),
				DateTimeStyles.None,
				out dateValue);
		}
		
		
		/// <summary>
		/// Создает новый справочник контекста
		/// </summary>
		private static void refresh() {
			context = new StringDictionary();
		}
	}
}

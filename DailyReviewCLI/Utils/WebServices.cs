/*
 * Created by SharpDevelop.
 * User: terekhov-ev
 * Date: 14.08.2015
 * Time: 9:59
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Xml;

namespace DailyReviewCLI.Utils {

	/// <summary>
	/// Description of WebServices.
	/// </summary>
	public class WebServices {
		public WebServices() {
		}
		
		public static string[] getForecast() {
			var sb = new StringBuilder();
			sb.Append("http://api.wunderground.com/api/");
			sb.Append(ConfigurationManager.AppSettings.Get("WeatherAPI"));
			sb.Append("/forecast/lang:RU/q/uspp.xml");
				
			var result = new List<string>();
			
			using (var webClient = new System.Net.WebClient()) {
				webClient.Encoding = Encoding.UTF8;
				XmlDocument xDoc = new XmlDocument();
				xDoc.LoadXml(webClient.DownloadString(sb.ToString()));
				
				result.Add(String.Format("# Прогноз погоды ({0})", xDoc.SelectSingleNode("//forecast/txt_forecast/date").InnerText));
				var xArray = xDoc.SelectNodes("//forecast/txt_forecast/forecastdays/forecastday");
				result.Add("|  | Иконка |Прогноз |");
				result.Add("|-|-|-|");
				int i = 0;
				var lines = new string[4];	
				foreach (XmlNode xNode in xArray) {
					if (int.Parse(xNode.SelectSingleNode("period").InnerText) <= 1) {
						lines[i] = "![](" + xNode.SelectSingleNode("icon_url").InnerText + ")";
						lines[++i] = xNode.SelectSingleNode("fcttext_metric").InnerText;
						i++;
					}
				}
				result.Add("| День | " + lines[0] + " | " + lines[1] + " |");
				result.Add("| Ночь | " + lines[2] + " | " + lines[3] + " |");
				result.Add("");
			}
			return result.ToArray();
		}
		
		public static byte[] getResourceByUrl(string sUrl) {
			byte[] result;
			using (var webClient = new System.Net.WebClient()) {
				result = webClient.DownloadData(sUrl);
			}
			return result;
		}

		public static string getProductivityData(string curDate) {
			var sb = new StringBuilder();
			sb.Append("https://www.rescuetime.com/anapi/daily_summary_feed?key=");
			sb.Append(ConfigurationManager.AppSettings.Get("RescueTimeAPI"));
			
			using (var webClient = new System.Net.WebClient()) {
				webClient.Encoding = Encoding.UTF8;
				string[] result = webClient.DownloadString(sb.ToString()).Split(',');
				bool isCurrentDate = false;
				StringBuilder resBuilder = new StringBuilder();
				foreach (var line in result) {
					isCurrentDate |= line.Contains(curDate);
					isCurrentDate &= !line.Contains("}");
					
					if (isCurrentDate) {
						if (line.Contains("\"productivity_pulse\"")) {
							resBuilder.Append(line.Replace("\"productivity_pulse\":", "Индекс продуктивности: ")).Append(", ");
						}
						if (line.Contains("\"all_productive_duration_formatted\"")) {
							resBuilder.Append(line.Replace("\"all_productive_duration_formatted\":", "продуктивное время: ").Replace("\"", "")).Append(", ");
						}
						if (line.Contains("\"all_distracting_duration_formatted\"")) {
							resBuilder.Append(line.Replace("\"all_distracting_duration_formatted\":", "непродуктивное время: ").Replace("\"", "")).Append(".");
						}
							
					}
				}
				return resBuilder.ToString();
			}
			
		}
		
		public static string getGoogleDocData(string cellAddress) {
			var sb = new StringBuilder();
			sb.Append(ConfigurationManager.AppSettings.Get("GoogleDoc"));
			sb.Append("&single=true&output=csv&range=");
			sb.Append(cellAddress);
			
			using (var webClient = new System.Net.WebClient()) {
				webClient.Encoding = Encoding.UTF8;
				string result = webClient.DownloadString(sb.ToString());
				Console.WriteLine(result);
				return result;
			}
			
			
		}
		
		
		public static string[] getHistoryFor(string curDate) {
			return new string[0] ;
		}
	}
}

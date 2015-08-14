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
				foreach (XmlNode xNode in xArray) {
					if (int.Parse(xNode.SelectSingleNode("period").InnerText) <= 1) {
						string line = String.Format("{0}: {1}", xNode.SelectSingleNode("title").InnerText, xNode.SelectSingleNode("fcttext_metric").InnerText); 
						result.Add(line);
					}
				}
				result.Add("");
			}
			return result.ToArray();
		}

		public static string[] getHistoryFor(string curDate) {
			return new string[0] ;
		}
	}
}

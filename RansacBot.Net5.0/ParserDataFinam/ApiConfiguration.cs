using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FinamDataLoader
{
	internal static class ApiConfiguration
	{
		/// <summary>
		/// Ссылка на соответсвующий инструмент и рынок, задается пользователем.
		/// </summary>
		public static string UsersFinamLink { get; set; }
		/// <summary>
		/// Ссылка на интсрументы с финама, но кажется она морально устарела.
		/// </summary>
		public static string SymbolsFeedUrl { get; private set; } = "http://www.finam.ru/cache/icharts/icharts.js";
		/// <summary>
		/// Базовый сайт экспорта данных с финама.
		/// </summary>
		public static string FinamExportHost { get; private set; } = "http://export.finam.ru";
		/// <summary>
		/// Путь для хранения данных по инструментам.
		/// </summary>
		public static string SymbolsPath { get; private set; } = @"JsonData\symbols.json";
		/// <summary>
		/// Путь для хранения данных по рынкам.
		/// </summary>
		public static string MarketsPath { get; private set; } = @"JsonData\markets.json";
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AutoWin {
	class Utils {

		private static Random _random = new Random();
		private static ConsoleColor GetRandomConsoleColor() {
			var consoleColors = Enum.GetValues(typeof(ConsoleColor));
			return (ConsoleColor)consoleColors.GetValue(_random.Next(consoleColors.Length));
		}

		public static void echoRandomColor(string line) {
			Console.ForegroundColor = GetRandomConsoleColor();
			Console.WriteLine(line);
		}

		public static void echoBanner() {

			echoRandomColor("");
			echoRandomColor(" █████╗ ██╗   ██╗████████╗ ██████╗    ██╗    ██╗ ██╗███╗   ██╗ ");
			echoRandomColor("██╔══██╗██║   ██║╚══██╔══╝██╔═████╗   ██║    ██║███║████╗  ██║ ");
			echoRandomColor("███████║██║   ██║   ██║   ██║██╔██║   ██║ █╗ ██║╚██║██╔██╗ ██║ ");
			echoRandomColor("██╔══██║██║   ██║   ██║   ████╔╝██║   ██║███╗██║ ██║██║╚██╗██║ ");
			echoRandomColor("██║  ██║╚██████╔╝   ██║   ╚██████╔╝██╗╚███╔███╔╝ ██║██║ ╚████║ ");
			echoRandomColor("╚═╝  ╚═╝ ╚═════╝    ╚═╝    ╚═════╝ ╚═╝ ╚══╝╚══╝  ╚═╝╚═╝  ╚═══╝ ");
			echoRandomColor("                                                $Pwn vs 0.0.1  ");
		}
		public static void echo(string line, string type = "info") {
			string box = "";
			switch (type) {
				case "info":
					box = "[*]";
					Console.ForegroundColor = ConsoleColor.Cyan;
					break;
				case "alert":
					box = "[!]";
					Console.ForegroundColor = ConsoleColor.Red;
					break;
				case "title":
					box = "[-]";
					Console.ForegroundColor = ConsoleColor.Yellow;
					break;
				case "success":
					box = "[#]";
					Console.ForegroundColor = ConsoleColor.Green;
					break;
			}
			Console.Write(box);
			Console.ResetColor();
			Console.WriteLine(" " + line);
		}

	}
}

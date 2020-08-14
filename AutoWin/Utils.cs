using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AutoWin {
	class Utils {

		public static void echoBanner() {

			Console.Write( "\n" +
				" █████╗ ██╗   ██╗████████╗ ██████╗    ██╗    ██╗ ██╗███╗   ██╗ \n" +
				"██╔══██╗██║   ██║╚══██╔══╝██╔═████╗   ██║    ██║███║████╗  ██║ \n" +
				"███████║██║   ██║   ██║   ██║██╔██║   ██║ █╗ ██║╚██║██╔██╗ ██║ \n" +
				"██╔══██║██║   ██║   ██║   ████╔╝██║   ██║███╗██║ ██║██║╚██╗██║ \n" +
				"██║  ██║╚██████╔╝   ██║   ╚██████╔╝██╗╚███╔███╔╝ ██║██║ ╚████║ \n" +
				"╚═╝  ╚═╝ ╚═════╝    ╚═╝    ╚═════╝ ╚═╝ ╚══╝╚══╝  ╚═╝╚═╝  ╚═══╝ \n" +
				"                                       $Pwn vs 0.0.1 \n");

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

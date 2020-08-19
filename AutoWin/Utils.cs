using System;

namespace AutoWin {
	class Utils {

		private static Random _random = new Random();

		public static string readError(string message) {
			switch(message) {
				case "Object reference not set to an instance of an object":
					return "Check if the Main() function is public!";
			}
			return message;
		}
		private static ConsoleColor GetRandomConsoleColor() {
			int[] allowed = {15,7,10,2,3,11};
			var consoleColors = Enum.GetValues(typeof(ConsoleColor));
			return (ConsoleColor)consoleColors.GetValue(allowed[_random.Next(allowed.Length)]);
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

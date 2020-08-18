using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Technique {

    public static Dictionary<string, string> EntryData { get; set; }
    public static Dictionary<string, string> ExitData { get; set; }

    public static bool execDiscovery(string cmd, string user, string context) {
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo {
            WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden
        };
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine((user != "" ? "[T1087-000] - Retrieving information about '" + user + "'" : "[T1087-000] - Querying all " + context + " users"));
        Console.ForegroundColor = ConsoleColor.Yellow;
        startInfo.FileName = "net.exe";
        // Get information from a user or a list of them
        startInfo.Arguments = cmd + (user != "" ? user : "");
        startInfo.RedirectStandardOutput = true;
        startInfo.UseShellExecute = false;
        startInfo.RedirectStandardOutput = true;
        process.StartInfo = startInfo;
        process.Start();
        process.WaitForExit();
        string result = process.StandardOutput.ReadToEnd();
        // Show entire output of command execution
        Console.WriteLine(result.Substring(0,result.Length-3));
        Console.ForegroundColor = ConsoleColor.Red;
        string valid = result.Substring(0, 6).Trim();
        Console.WriteLine((valid == "User n" || valid == "Nome d" ? "[T1087-000] - End of content about user '" + user + "'." : "[T1087-000] End of query [T1087-000]"));
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Gray;

        return (result != "" ? true : false);
    }

    public static bool searchUsers(string[] args) {
        try {
            Console.WriteLine("[T1087-000] Setting up arguments on flow");
            bool check = false;

            for (int i = (args.Length > 1 ? 1 : 0); i < args.Length; i++) {
                string toDo = args[0];
                switch (toDo.ToLower()) {
                    case "local":
                        toDo = "user ";
                        break;
                    case "domain":
                        toDo = "user /domain ";
                        break;
                    default:
                        Console.WriteLine("[ERROR] Atribute '" + args[0] + "' not found!");
                        Console.Write("[T1087-000] Try insert as a valid value in the 1st argument: user | domain\n\n");
                        return false;
                }
                
                check = execDiscovery(toDo, (args.Length > 1 ? args[i] : ""), args[0]);
            }

            if (check) {
                return true;
            } else {
                Console.WriteLine("[!!!] Something went wrong [!!!]\nNo valid results retrived.");
            }
        } catch (Exception ex) {
            Console.WriteLine("Error:" + ex.Message);
            throw;
        }
        return false;
    }
    public static void Main(string[] args) {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("[T1087-000] Started Execution!");
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine("[T1087-000] Getting data from: '" + args[0] + "' context");

        if (searchUsers(args)) {
            Console.WriteLine("[T1087-000] Module successfully executed!");
        } else {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[T1087-000] Something strange occurred!");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}

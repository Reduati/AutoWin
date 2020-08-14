using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Hosting;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


class Technique {
    public static Dictionary<string, string> EntryData { get; set; }
    public static Dictionary<string, string> ExitData { get; set; }
    
    public static bool schedRun(string[] args) {
        try {
            Console.WriteLine("[T1053-005] - Setting up arguments on flow");
            string argRun = "";
            switch (args[0].ToLower()) {
                case "create":
                    argRun = "/create /tn \"" + args[1] + "\" /tr \"" + args[2] + "\" /sc " + args[3] + " " + (args.Length >= 5 ? args[4]: "");
                    break;
                case "run":
                    argRun = "/run /tn \"" + args[1] + "\" " + args[2];
                    break;
                case "query":
                    argRun = "/query /tn \"" + args[1] + "/fo LIST /v " + args[2];
                    break;
                case "privesc":                   
                    if (EntryData.ContainsKey("password") && EntryData.ContainsKey("username")) {
                        string username = EntryData["username"];
                        string password = EntryData["password"];

                        argRun = "/create /tn \"" + args[1] + "\" /tr \"" + args[2] + "\" /sc " + args[3] + " /ru " + username + " /rp " + password + " /rl HIGHEST" + args[4];
                        break;
                    } else {
                        Console.WriteLine("To try scheduled a task with a privileged user, set 'username' AND 'password' keys in the 'EntryData' at the file .flow!");
                        return false;
                    }
            }

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            
            Console.WriteLine("### Starting Scheduled Task Module - [T1053-005] ###");

            startInfo.FileName = "schtasks.exe";
            startInfo.Arguments = argRun;
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            if (process.ExitCode == 0) {
                return true;
            } else {
                Console.WriteLine("!!! Something went wrong: " + Environment.NewLine + process.StandardOutput.ReadToEnd()
                    + Environment.NewLine);
            }
        } catch (Exception ex) {
            Console.WriteLine("Error:" + ex.Message);
        }
        return false;
}

    public static void Main(string[] args) {
        Console.WriteLine("[T1053-005] Started Execution!");
        Console.WriteLine("[T1053-005] Attempting to '" + args[0] + "' ");
        
        if (schedRun(args)) {
            Console.WriteLine("[T1053-005] Scheduled module successfully executed!");
        }
    }
}

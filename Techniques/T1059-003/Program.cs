using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Technique{
    /*
        * 
        * T1059-003 (Command and Scripting Interpreter: Windows Command Shell).
        *
        * Possible Parameters:
        *   1 - command - Required
        *       Command to be executed by cmd.exe. If your command has spaces, make sure to surround them with double quotes.
        *   
    */

    public static Dictionary<string, string> EntryData { get; set; }
    public static Dictionary<string, string> ExitData { get; set; } = new Dictionary<string, string>();

    public static bool execCommand(string executable, string parameters){
        try{
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();

            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = executable;
            startInfo.Arguments = "/C \"" + parameters + "\"";
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;

            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            if (process.ExitCode == 0){
                return true;
            }
            Console.WriteLine(process.StandardOutput.ReadToEnd());
        }
        catch (Exception ex){
            Console.WriteLine("Error:" + ex.Message);
        }
        return false;
    }
    public static void Main(string[] args){
        Console.WriteLine("[T1059-003] Starting Execution!");
        string execMethod = "cmd.exe";

        Console.WriteLine("[T1059-003] Using " + execMethod + " with: command '" + args[0] +"'");
        if (execCommand(execMethod, args[0])){
            Console.WriteLine("[T1059-003] Successfully executed Technique (return 0)! ");
        }
        else{
            Console.WriteLine("[T1059-003] Oops, something went wrong! ");
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class Program {
    /*
	 * T1059.007 (Javascript / Jscript) - For this technique, we have a couple of entry points that end up using jscript.dll. For testing purpose, this module will
	 * test our enviroment using both cscript for JS files and mshta for HTA.
	 */

    public static bool execCommand(string with, string command) {

        try {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = with;
            startInfo.Arguments = command;
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            if (process.ExitCode == 0) {
                return true;
            }
            Console.WriteLine(process.StandardOutput.ReadToEnd());
        }
        catch (Exception ex) {
            Console.WriteLine("Error:" + ex.Message);
        }
        return false;
    }
    public static void Main(string[] args) {

        Console.WriteLine("[T1059.007] Starting Execution!");
        string execMethod = "cscript.exe";

        if (File.Exists(args[1])) {
            if (args[0] == "hta") {
                execMethod = "mshta.exe";
            }
            Console.WriteLine("[T1059.007] Using " + execMethod + " with: " + args[1]);
            if (execCommand(execMethod, args[1])) {
                Console.WriteLine("[T1059.007] Successfully executed Technique (return 0)! ");
            }
            else {
                Console.WriteLine("[T1059.007] Oops, something went wrong! ");
            }
        }

    }
}


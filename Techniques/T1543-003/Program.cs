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
    * T1543-003 (Create or Modify System Process: Windows Service).
    *
    * Possible Parameters:
    *   1 - binPath - Required
    *       Path to the desired service script/executable. i.e. \Users\Public\script.bat
    *       
    *   2 - serviceName - Optional
    *       Name to be given to the service created. Defaults to a random UUID.
    *       
    *   Requires Admin privileges
    *   
*/

    public static Dictionary<string, string> EntryData { get; set; }
    public static Dictionary<string, string> ExitData { get; set; }

    public static bool execCommand(string executable, string parameters, string uuid)
    {
        try
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();

            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = executable;
            startInfo.Arguments = "create " + uuid + " binpath='" + parameters + "'";
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;

            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            if (process.ExitCode == 0)
            {
                return true;
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
        Console.WriteLine("[T1543.003] Starting Execution!");
        string execMethod = "sc.exe";
        string uuid;

        if (args.Length != 2)
        {
            uuid = System.Guid.NewGuid().ToString();
        }
        else
        {
            uuid = args[1];
        }

        Console.WriteLine("[T1543.003] Using " + execMethod + " with: ServiceName '" + uuid + "' with binPath '" + args[0] + "'");
        if (execCommand(execMethod, args[0], uuid))
        {
            Console.WriteLine("[T1543.003] Successfully executed Technique (return 0)! ");
        }
        else
        {
            Console.WriteLine("[T1543.003] Oops, something went wrong! ");
        }
    }
}
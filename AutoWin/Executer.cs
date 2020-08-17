using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AutoWin {
    class Executer {

		public static bool Start(Program.AttackFlowTechnique techniqueData) {

			string technique = techniqueData.Technique;
			try {
				var technique_path = Program.project_path + technique + ".exe";
				if (File.Exists(technique_path)) {
					var bytes = File.ReadAllBytes(technique_path);
					if(Inject(bytes, techniqueData.Parameters)) {
						return true;
					}
				} else {
					Utils.echo("Cand find technique binary for " + technique, "alert");
				}
			} catch (Exception ex) {
				Utils.echo("Error during executer start: " + ex.Message,"alert");
			}
			
			return false;
        }

		public static bool Inject(byte[] bytes, string[] techniqueParams) {

			
			try {

				string returnMessage = null;
				string returnCode = null;

				var assembly = System.Reflection.Assembly.Load(bytes);
				Type t = assembly.GetType("Technique");
				object o = Activator.CreateInstance(t);
				object[] args = new object[] { techniqueParams };
				PropertyInfo entryDataRef = o.GetType().GetProperty("EntryData");
				entryDataRef.SetValue(o, Program.EntryData);
				t.GetMethod("Main").Invoke(o, args);
				PropertyInfo exitDataRef = o.GetType().GetProperty("ExitData");
				Dictionary<string, string> ExitData = (Dictionary<string, string>)exitDataRef.GetValue(o);
				
				try {
                    if (ExitData.ContainsKey("returnmessage") || ExitData.ContainsKey("returncode")) {
						returnMessage = ExitData["returnmessage"];
						returnCode = ExitData["returncode"];

						Utils.echo("[DEBUG] Return Code:" + returnCode + " Return Message:" + returnMessage);
					}
				} catch (Exception ex) {
					Console.WriteLine("[ERROR] " + ex.Message);
				}

				return true;
			} catch (Exception ex) {
				Console.WriteLine("error injecting code:" + ex.Message);
			}
			return false;
		}
	}
}

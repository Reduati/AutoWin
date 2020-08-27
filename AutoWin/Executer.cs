using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AutoWin {
    class Executer {

		public static bool Start(string campaignName, string campaignDatetime, Program.AttackFlowTechnique techniqueData) {

			Program.logger.Trace("Validating the parameters received on execution module for technique " + techniqueData.Technique + " and preparing for injection.");
			string technique = techniqueData.Technique;
			try {

				var technique_bin_path = Program.project_path + technique + ".exe";
				var technique_module_path = Program.project_path + technique + ".m";

				if (File.Exists(technique_bin_path) || File.Exists(technique_module_path)) {

					byte[] bytes = null;

					if (File.Exists(technique_bin_path)) {
						bytes = File.ReadAllBytes(technique_bin_path);
					} else {
						var bytes_f = File.ReadAllText(technique_module_path);
						bytes = Convert.FromBase64String(bytes_f);
					}
					Program.logger.Info("Injecting technique " + technique + ".");
					if (Inject(bytes, techniqueData.Parameters)) {
						return true;
					}

				} else {
					Program.logger.Error("Cand find technique binary or module for " + technique + ".");
					Utils.echo("Cand find technique binary or module for " + technique + ".", "alert");
				}
			} catch (Exception ex) {
				Program.logger.Error("Error during executer start: " + ex.Message + ".");
				Utils.echo("Error during executer start: " + ex.Message + ".", "alert");
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
				}
				catch (Exception ex) {
					Console.WriteLine("[ERROR] " + ex.Message);
				}
				
				Program.logger.Info("Executed technique with success.");
				return true;
			} catch (Exception ex) {
				Program.logger.Error("Could not execute technique.");

				Utils.echo( String.Format("Error injecting code ({0})", Utils.readError(ex.Message)), "alert");
			}
			return false;
		}
	}
}

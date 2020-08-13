using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
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
					Inject(bytes, techniqueData.Parameters);
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
				var assembly = System.Reflection.Assembly.Load(bytes);
				foreach (var type in assembly.GetTypes()) {
					object instance = Activator.CreateInstance(type);
					//object[] args = new object[] { techniqueParams, techniqueParams };
					object[] args = new object[] { techniqueParams };
					try {
						type.GetMethod("Main").Invoke(instance, args);
					} catch (Exception ex) {
						Console.WriteLine("Error during injection: " + ex.Message); 
					}
				}
				return true;
			} catch (Exception ex) {
				Console.WriteLine("error injecting code:" + ex.Message);
			}
			return false;
		}
	}
}

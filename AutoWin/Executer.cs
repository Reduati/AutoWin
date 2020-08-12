using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoWin {
    class Executer {

		public static bool Start(string technique) {

			var technique_path =  Program.project_path + technique+".exe";

			if(File.Exists(technique_path)) {
				var bytes = File.ReadAllBytes(technique_path);
				Console.WriteLine("Executing: " + technique);
				Inject(bytes);
				Console.WriteLine(technique);
			} else {
				Console.WriteLine("Cand find technique binary for ("+ technique_path + ")" + technique);
            }
			
			
			return false;
        }
		public static bool Inject(byte[] bytes) {

			try {
				var assembly = System.Reflection.Assembly.Load(bytes);

				foreach (var type in assembly.GetTypes()) {

					object instance = Activator.CreateInstance(type);
					object[] args = new object[] { new string[] { "" } };
					try {
						type.GetMethod("Main").Invoke(instance, args);
					} catch { Console.WriteLine("error"); }
				}
				return true;
			} catch (Exception ex) {
				Console.WriteLine("error injecting code:" + ex.Message);
			}
			return false;
		}
	}
}

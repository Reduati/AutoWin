using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoWin {
    class Executer {
		public static bool Inject(string assemblyB64) {

			try {
				var bytes = Convert.FromBase64String(assemblyB64);
				var assembly = System.Reflection.Assembly.Load(bytes);

				foreach (var type in assembly.GetTypes()) {

					object instance = Activator.CreateInstance(type);
					object[] args = new object[] { new string[] { "" } };
					try {
						type.GetMethod("Main").Invoke(instance, args);
					} catch { }
				}
				return true;
			} catch (Exception ex) {
				Console.WriteLine("error injecting code:" + ex.Message);
			}
			return false;
		}
	}
}

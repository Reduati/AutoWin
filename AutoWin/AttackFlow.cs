using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AutoWin {
	class AttackFlow {

		public static bool Start(string AttackFlowPath) {

            Program.JSONParseAttack ParsedAttackFlowTechniques;
            string AttackFlowTemp;
           
            if (File.Exists(AttackFlowPath)) {

                AttackFlowTemp = File.ReadAllText(AttackFlowPath);
                ParsedAttackFlowTechniques = JsonSerializer.Deserialize<Program.JSONParseAttack>(AttackFlowTemp);
                Utils.echo("Campaign: " + ParsedAttackFlowTechniques.Campaign + "\n    Datetime:" + ParsedAttackFlowTechniques.Datetime);

                foreach (var tech in ParsedAttackFlowTechniques.Techniques.Values) {
                    Utils.echo("Trying to run technique: " + tech.Technique);
                    Executer.Start(tech);
                }
            }

            return true;
        }
	}
}

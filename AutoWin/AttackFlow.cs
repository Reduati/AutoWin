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
            
            // check if the JSON file really exists
            if (File.Exists(AttackFlowPath)) {

                try {

                    AttackFlowTemp = File.ReadAllText(AttackFlowPath);
                    ParsedAttackFlowTechniques = JsonSerializer.Deserialize<Program.JSONParseAttack>(AttackFlowTemp);

                    Utils.echo("Campaign: " + ParsedAttackFlowTechniques.Campaign + "\n    Datetime:" + ParsedAttackFlowTechniques.Datetime);

                    // Define public scope of entrydata
                    foreach (var data in ParsedAttackFlowTechniques.EntryData) {
                        Program.EntryData[data.Key] = ParsedAttackFlowTechniques.EntryData[data.Key];
                    }

                    // iterate over each technique and start execution process
                    foreach (var tech in ParsedAttackFlowTechniques.Techniques.Values) {

                        // Define private scope of entrydata
                        foreach (var data in tech.EntryData) {
                            Program.EntryData[data.Key] = tech.EntryData[data.Key];
                        }
						
                        Utils.echo("Trying to run technique: " + tech.Technique);
                        Executer.Start(tech);
                    }

                    return true;

                } catch (Exception ex) {
                    Utils.echo("Error reading JSON file -  " + ex.Message ,"alert");
				}
                
            }

            return false;
        }
	}
}

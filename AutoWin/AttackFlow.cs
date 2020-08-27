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

            Program.logger.Trace("Validating the received flow file.");
            // check if the JSON file really exists
            if (File.Exists(AttackFlowPath)) {
                Program.logger.Trace("The received flow file exists.");
                try {

                    AttackFlowTemp = File.ReadAllText(AttackFlowPath);
                    ParsedAttackFlowTechniques = JsonSerializer.Deserialize<Program.JSONParseAttack>(AttackFlowTemp);

                    Program.logger.Info("The flow file received has the following setting: Campaign: " + ParsedAttackFlowTechniques.Campaign + "| Datetime:" + ParsedAttackFlowTechniques.Datetime);
                    Utils.echo("Campaign: " + ParsedAttackFlowTechniques.Campaign + "\n    Datetime:" + ParsedAttackFlowTechniques.Datetime);

                    // Define public scope of entrydata
                    Program.logger.Trace("Defining public scope of entrydata.");
                    foreach (var data in ParsedAttackFlowTechniques.EntryData) {
                        Program.EntryData[data.Key] = ParsedAttackFlowTechniques.EntryData[data.Key];
                    }

                    // Iterate over each technique and start execution process
                    foreach (var tech in ParsedAttackFlowTechniques.Techniques.Values) {

                        // Define private scope of entrydata
                        foreach (var data in tech.EntryData) {
                            Program.EntryData[data.Key] = tech.EntryData[data.Key];
                        }

                        Program.logger.Info("Starting the execution process for techinque " + tech.Technique + ".");
                        Utils.echo("Trying to run technique: " + tech.Technique);
                        Executer.Start(ParsedAttackFlowTechniques.Campaign, ParsedAttackFlowTechniques.Datetime, tech);
                    }

                    return true;

                } catch (Exception ex) {
                    Program.logger.Error("Got an exception while reading flow file.");
                    Utils.echo("Error reading JSON file -  " + ex.Message ,"alert");
				}
                
            }
            Program.logger.Error("The flow file received does not exist.");
            return false;
        }
	}
}

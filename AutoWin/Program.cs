using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AutoWin {
    class Program {

        public static string project_path = AppDomain.CurrentDomain.BaseDirectory + @"lib\";

        public class AttackFlowTechnique {
            public string Technique { get; set; }
            public string[] Parameters { get; set; }
        }
        public class JSONParseAttack {
            public string Campaign { get; set; }
            public string Datetime { get; set; }
            public Dictionary<string, AttackFlowTechnique> Techniques { get; set; }
        }

        static void Main(string[] args) {
            //stores the execution method supplied on the arguments during execution. 0 is full, 1 is flow and 2 is debug.
            int ExecutionMethod;
            //stores the path to the attack flow file supplied by execution argument on execution method 1 - attack flow.
            string AttackFlowPath = "";
            //stores the techniques indicated on the attack flow file
            JSONParseAttack ParsedAttackFlowTechniques;

            //dealing with arguments
            if (args.Length == 0) {
                System.Console.WriteLine("No argument was received. See --help for instructions.");
                return;
            } else {
                switch (args[0]) {
                    case "--full":
                        ExecutionMethod = 1;
                        break;
                    case "--flow":
                        if (args.Length == 2) {
                            AttackFlowPath = args[1];
                            ExecutionMethod = 2;
                        } else {
                            System.Console.WriteLine("The Attack Flow execution requires an attack flow file as a parameter following --flow. Dying.");
                            return;
                        }
                        break;
                    case "--debug":
                        ExecutionMethod = 3;
                        break;
                    case "--help":
                        System.Console.WriteLine("./executavel.exe [--full, --flow path_to_flow_file, --debug]\n\n--full\n    Invokes all of the possible techniques without any sense of progression or intent of simulating a real attack. Useful for testing general detection and prevention capabilities.\n\n--flow path_to_flow_file\n    Requires a flow file defining techniques to be used in succession with the intent of simulating a real attack. Useful for adversary emulation exercises.\n\n--debug\n    General debugging mode, tests the dependencies, error handling and validity of the execution with a sample technique and/or a blank technique.");
                        return;
                    default:
                        System.Console.WriteLine("No valid argument was received. See --help for instructions.");
                        return;
                }
            }

            switch (ExecutionMethod) {
                //Execution Method 0 - Full
                case 1:
                    //executor("Full", "2020-07-30 08:28:13", tech.Method, tech.Parameters)
                    break;
                //Execution Method 1 - Attack Flow
                case 2:
                    string AttackFlowTemp;

                    AttackFlowTemp = File.ReadAllText(AttackFlowPath);
                    
                    ParsedAttackFlowTechniques = JsonSerializer.Deserialize<JSONParseAttack>(AttackFlowTemp);
                    Console.WriteLine(ParsedAttackFlowTechniques.Campaign);
                    foreach (var tech in ParsedAttackFlowTechniques.Techniques.Values) {

                        Console.WriteLine(tech.Technique);
                        Executer.Start(tech.Technique);
                        //executor(ParsedAttackFlowTechniques.Campaign, ParsedAttackFlowTechniques.Datetime, tech.Method, tech.Parameters)
                        //Console.WriteLine(tech.Technique.);
                        //loop through all of the parameters, if we need it.
                        //string[] TempParams = new string[tech.Parameters.Count()];
                        //for (int i = 0; i < tech.Parameters.Count(); i++)
                        //{
                        //    TempParams[i] = tech.Parameters[i];
                        //}
                    }
                    break;
                //Execution Method 2 - Debug
                case 3:
                    //executor('1')
                    break;
            }
        
    
        }
    }
}

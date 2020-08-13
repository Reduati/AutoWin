using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

            Utils.echoBanner();

            //stores the execution method supplied on the arguments during execution. 0 is full, 1 is flow and 2 is debug.
            int ExecutionMethod;
            //stores the path to the attack flow file supplied by execution argument on execution method 1 - attack flow.
            string AttackFlowPath = "";
   
            //dealing with arguments
            if (args.Length == 0) {
                Utils.echo("No argument was received. See --help for instructions.","alert");
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
                            Utils.echo("The Attack Flow execution requires an attack flow file as a parameter following --flow. Dying.", "alert");
                            return;
                        }
                        break;
                    case "--debug":
                        ExecutionMethod = 3;
                        break;
                    case "--help":
                        Utils.echo("./executavel.exe [--full, --flow path_to_flow_file, --debug]\n\n--full\n    Invokes all of the possible techniques without any sense of progression or intent of simulating a real attack. Useful for testing general detection and prevention capabilities.\n\n--flow path_to_flow_file\n    Requires a flow file defining techniques to be used in succession with the intent of simulating a real attack. Useful for adversary emulation exercises.\n\n--debug\n    General debugging mode, tests the dependencies, error handling and validity of the execution with a sample technique and/or a blank technique.", "alert");
                        return;
                    default:
                        Utils.echo("No valid argument was received. See --help for instructions.","alert");
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

                    Utils.echo("Starting executing using flow","title");
                    if (AttackFlow.Start(AttackFlowPath)) {
                        Utils.echo("Finished executing Attack flow!", "success");
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

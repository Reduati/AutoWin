using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace AutoWin {
    class Program {

        public static string project_path = AppDomain.CurrentDomain.BaseDirectory + @"lib\";
        public static Dictionary<string, string> EntryData = new Dictionary<string, string>();
        public static SimpleLogger logger = new SimpleLogger();

        public class AttackFlowTechnique {
            public string Technique { get; set; }
            public Dictionary<string, string> EntryData { get; set; } = new Dictionary<string, string>();
            public string[] Parameters { get; set; }
        }
        public class JSONParseAttack {
            public string Campaign { get; set; }
            public string Datetime { get; set; }
            public Dictionary<string, string> EntryData { get; set; } = new Dictionary<string, string>();
            public Dictionary<string, AttackFlowTechnique> Techniques { get; set; }
        }

        public static void readConfigFlags(string[] args) {

            for (int k=1;k<=args.Length-1;k++) {
                switch (args[k]) {
                    case "-v":
                        Console.WriteLine("Setting verbose to 2.");
                        logger.SetVerboseLevel(2);
                        break;
                    case "--verbose":
                        Console.WriteLine("Setting verbose to 2.");
                        logger.SetVerboseLevel(2);
                        break;
                    case "-s":
                        Console.WriteLine("Setting verbose to 0.");
                        logger.SetVerboseLevel(0);
                        break;
                    case "--succinct":
                        Console.WriteLine("Setting verbose to 0.");
                        logger.SetVerboseLevel(0);
                        break;
                    case "--lib":
                        Program.project_path = new Uri(args[k + 1]).LocalPath;
                        break;
                    case "--workfolder":
                        EntryData["Workfolder"] = new Uri(args[k + 1]).LocalPath;
                        break;
                }
            }
		}

        static void Main(string[] args) {

            Utils.echoBanner();

            //define information that will be shared with techniques during execution time
            EntryData["Workfolder"] = @"c:\users\public\"; // default path

            //stores the execution method supplied on the arguments during execution. 0 is full, 1 is flow and 2 is debug.
            int ExecutionMethod;
            //stores the path to the attack flow file supplied by execution argument on execution method 1 - attack flow.
            string AttackFlowPath = "";

            //dealing with arguments
            if (args.Length == 0)
            {
                Utils.echo("No argument was received. See --help for instructions.","alert");
                return;
            } else {
                readConfigFlags(args);

                switch (args[0]) {
                    case "--full":
                        ExecutionMethod = 1;
                        break;
                    case "--flow":
                        if (args.Length >= 2) {
                            AttackFlowPath = args[1];
                            ExecutionMethod = 2;
                        } else { 
                            Utils.echo("The Attack Flow execution requires an attack flow file as a parameter following --flow. Dying.", "alert");
                            return;
                        }
                        break;
                    case "--technique":
                        ExecutionMethod = 3;
                        break;
                    case "--help":
                        Utils.echo("./executavel.exe [--full, --flow path_to_flow_file, --debug]\n\n--full\n    Invokes all of the possible techniques without any sense of progression or intent of simulating a real attack. Useful for testing general detection and prevention capabilities.\n\n--flow path_to_flow_file\n    Requires a flow file defining techniques to be used in succession with the intent of simulating a real attack. Useful for adversary emulation exercises.\n\n--debug\n    General debugging mode, tests the dependencies, error handling and validity of the execution with a sample technique and/or a blank technique.", "alert");
                        return;
                    default:
                        Utils.echo("No valid execution method parameter was received. See --help for instructions.","alert");
                        return;
                }


            }

            logger.Info("Defining execution method.");
            switch (ExecutionMethod)
            {
                //Execution Method 0 - Full
                case 1:
                    logger.Info("Full execution method selected, initiating.");
                    //executor("Full", "2020-07-30 08:28:13", tech.Method, tech.Parameters)
                    break;
                //Execution Method 1 - Attack Flow
                case 2:
                    logger.Info("Attack flow execution method selected, initiating.");
                    Utils.echo("Starting executing using flow","title");
                    
                    if (AttackFlow.Start(AttackFlowPath)) {
                        Utils.echo("Finished executing Attack flow!", "success");
                    }
                    
                    break;
                //Execution Method 2 - Debug
                case 3:
                    logger.Info("Debug execution method selected, initiating.");
                    //executor('1')
                    break;
            }
        }
    }
}

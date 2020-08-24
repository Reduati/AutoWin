using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


class Technique {

    /*
     *  T1046 - Simple TCP Network Scan
     */

    public static Dictionary<string, string> EntryData { get; set; }
    public static Dictionary<string, string> ExitData { get; set; }


    static bool checkTCPPort(string target, int port) {

        try {
            var client = new TcpClient();
            var result = client.BeginConnect(target, port, null, null);
            var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(1));

            if (!success) {
                return false;
            }

            client.EndConnect(result);
            
        } catch (Exception ex) {
            Console.WriteLine("Error doing TCP check on {0}:{1} ({2})", target, port, ex);
		}

        return true;
    }

    static string[] convertCIDRToRange(string IP) {

        string[] parts = IP.Split('.', '/');

        uint ipnum = (Convert.ToUInt32(parts[0]) << 24) |
            (Convert.ToUInt32(parts[1]) << 16) |
            (Convert.ToUInt32(parts[2]) << 8) |
            Convert.ToUInt32(parts[3]);

        int maskbits = Convert.ToInt32(parts[4]);
        uint mask = 0xffffffff;
        mask <<= (32 - maskbits);

        uint ipstart = ipnum & mask;
        uint ipend = ipnum | (mask ^ 0xffffffff);
        string[] result = { String.Format("{0}.{1}.{2}.{3}", ipstart >> 24, (ipstart >> 16) & 0xff, (ipstart >> 8) & 0xff, ipstart & 0xff),
                            String.Format("{0}.{1}.{2}.{3}", ipend >> 24, (ipend >> 16) & 0xff, (ipend >> 8) & 0xff, ipend & 0xff)};
        return result;
    }

    static uint ToUInt(string ipAddress) {
        var ip = IPAddress.Parse(ipAddress);
        var bytes = ip.GetAddressBytes();
        Array.Reverse(bytes);
        return BitConverter.ToUInt32(bytes, 0);
    }

    static string ToString(uint ipInt) {
        return ToIPAddress(ipInt).ToString();
    }

    static IPAddress ToIPAddress(uint ipInt) {
        var bytes = BitConverter.GetBytes(ipInt);
        Array.Reverse(bytes);
        return new IPAddress(bytes);
    }

    public static bool scanTCPTarget(string target, string ports) {

        try {
            string[] address_range = convertCIDRToRange(target);
            Console.WriteLine("[T1046] {0} - {1} on port(s) {2} ", address_range[0], address_range[1], ports);
            uint current = ToUInt(address_range[0]), last = ToUInt(address_range[1]);

            while (current <= last) {
                var ip = ToIPAddress(current++);
                Console.WriteLine("[T1046] Checking {0}", ip.ToString());
                foreach (var port in ports.Split(',')) {
                    if (checkTCPPort(ip.ToString(), int.Parse(port))) {
                        Console.WriteLine("[T1046] - {0}:{1} - open", ip.ToString(), port);
                    }

                }
            }

        } catch (Exception ex) {
            Console.WriteLine("[T1046] Error: {0} ", ex.Message);
		}

        return true;
    }

	public static void Main(string[] args) {

        string target_range = null;
        string target_ports = null;
        ExitData = new Dictionary<string, string>();
        ExitData["returncode"] = "1";
        ExitData["returnmessage"] = "Something went wrong, check debug logs!";

        if (args.Length < 2) {
            string hostName = Dns.GetHostName();
            var local_ip = Dns.GetHostByName(hostName).AddressList[0].ToString();
            Console.WriteLine(local_ip);
            target_range = String.Format("{0}/24", local_ip);
            target_ports = "21,22,80,443";
		} else {
            target_range = args[0];
            target_ports = args[1];
        }

        Console.WriteLine("[T1046] Starting Simple TCP Scan...");
        if(scanTCPTarget(target_range, target_ports)) {
            Console.WriteLine("[T1046] Done!");
            ExitData["returncode"] = "0";
            ExitData["returnmessage"] = "Technique executed without errors";
        }

    


    }
}


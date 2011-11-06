using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using snmputil;
using System.Net;

namespace SNMPDetector {
    class Program {
        static void Main(string[] args) {

            // THIS BIT WORKS

            // we will need 2 EP's because the broadcast IP won't find a local SNMP agent
            IPAddress broadcastIP = IPAddress.Parse("10.0.0.255");
            IPEndPoint broadcastEP = new IPEndPoint(broadcastIP, 161); // default snmp port is 161

            // local
            IPAddress localIP = IPAddress.Parse("127.0.0.1");
            IPEndPoint localEP = new IPEndPoint(localIP, 161);

            SNMPInteractor.SNMPAgentFound += new snmputil.Events.SNMPAgentFoundHandler(SNMPInteractor_SNMPAgentFound);
            SNMPInteractor.DiscoverSNMPAgents(broadcastEP);
            SNMPInteractor.DiscoverSNMPAgents(localEP);


            // get mib            
            IPAddress extIP = IPAddress.Parse("10.0.0.7");
            Console.WriteLine("Getting MIB of "+extIP+", this may take a while, please wait.");
            IPEndPoint extEP = new IPEndPoint(extIP, 161);
            Dictionary<String, String> mib = SNMPInteractor.GetMIB(extEP);

            Console.WriteLine("MIB received, "+mib.Count+" elements");
            for (int i = 0; i < 100; i++) {
                Console.WriteLine(mib.Keys.ElementAt(i) + " : " + mib.Values.ElementAt(i));
            }
            Console.WriteLine("=======================\n100 elements shown, " + (mib.Count - 100) + " elements omitted.");

            // don't end program
            Console.ReadLine();
            
            // reading the entire MIB is a bit much. Need more specific info.
            // info that is actually useful: 
            // something that shows the device type
            // get 1.3.6.1.2.1.4.21.1 => switches will not return data
            // in one broadcastdomain => if ip = default gateway => router
            // something else that might be interesting:
            // get 1.3.6.1.2.1.1.7.0 => 2 if switch, 4 if router, if 70 (end to end + apps) > end device 
            // see also http://www.oid-info.com/get/1.3.6.1.2.1.1.7

        }

        static void SNMPInteractor_SNMPAgentFound(snmputil.Events.SNMPAgentFoundEventArgs e) {
            Console.WriteLine("An SNMP agent was found at "+e.Agent.Address.ToString());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using snmputil.Events;

namespace snmputil {
    public static class SNMPInteractor {

        public static event SNMPAgentFoundHandler SNMPAgentFound;

        /// <summary>
        /// Discover SNMP agents on a network using a broadcast IP. Listen for the SNMPAgentFound Event.
        /// </summary>
        /// <param name="broadcastIP">Broadcast IP, must be IPv4</param>
        public static void DiscoverSNMPAgents(IPEndPoint broadcastIP) {
            SNMP.DiscoverSNMPAgents(broadcastIP);
            SNMP.SNMPAgentFound += new SNMPAgentFoundHandler(SNMPAgentFoundInternal);
        }

        /// <summary>
        /// Get the MIB of an SNMP agent.
        /// </summary>
        /// <param name="agentEP">IPEndPoint of the device that holds the agent.</param>
        /// <returns>MIB as a dictionary</returns>
        public static Dictionary<String, String> GetMIB(IPEndPoint agentEP) {
            return SNMP.GetMIB(agentEP);
        }

        static void SNMPAgentFoundInternal(SNMPAgentFoundEventArgs e) {
            // pass on to the user of this dll
            SNMPAgentFound(e);
        }

    }
}

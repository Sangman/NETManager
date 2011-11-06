using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lextm.SharpSnmpLib.Messaging;
using System.Net;
using Lextm.SharpSnmpLib;
using snmputil.Events;
using System.Threading;
using Lextm.SharpSnmpLib.Security;

namespace snmputil {
    internal static class SNMP {
        internal static event SNMPAgentFoundHandler SNMPAgentFound;

        /// <summary>
        /// Discover SNMP V1, V2 and V3 agents on a network.
        /// </summary>
        /// <param name="broadcastIP">Endpoint that signifies the broadcast IP, must be IPv4</param>
        internal static void DiscoverSNMPAgents(IPEndPoint broadcastIP) {
            Discoverer disc = new Discoverer();
            disc.AgentFound += new EventHandler<AgentFoundEventArgs>(SNMPAgentFoundHandler);
            disc.Discover(VersionCode.V1, broadcastIP, new OctetString("public"), 50);
        }

        /// <summary>
        /// Get the Management Information Base of one SNMP agent.
        /// </summary>
        /// <param name="agentEP">IPEndPoint of the device with the agent.</param>
        /// <returns>MIB as a Dictionary</returns>
        internal static Dictionary<String, String> GetMIB(IPEndPoint agentEP) {
            Dictionary<String, String> mib = new Dictionary<String, String>();
            // start
            String id = ".0.0";

            do {
                GetNextRequestMessage message = new GetNextRequestMessage(0,
                    VersionCode.V1,
                    new OctetString("public"), // returns messageexception if false community string
                    new List<Variable> { new Variable(new ObjectIdentifier(id)) });

                ResponseMessage response = (ResponseMessage)message.GetResponse(100, agentEP, new UserRegistry(), agentEP.GetSocket());

                // get id
                id = response.Scope.Pdu.Variables[0].Id.ToString();
                if (mib.ContainsKey(id)) break;
                mib.Add(id, response.Scope.Pdu.Variables[0].Data.ToString());
            } while (true);

            return mib;
            

        }

        private static void SNMPAgentFoundHandler(object sender, AgentFoundEventArgs e) {
            // this event needs to be passed on to bl layer
            IPEndPoint agent = e.Agent;

            // wait some milliseconds or we'll get a nullreference exception
            Thread.Sleep(150);

            SNMPAgentFoundEventArgs a = new SNMPAgentFoundEventArgs(e.Agent);
            SNMPAgentFound(a);
        }
    }
}

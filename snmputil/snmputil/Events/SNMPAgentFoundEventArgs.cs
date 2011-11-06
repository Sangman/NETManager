using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace snmputil.Events {
    public class SNMPAgentFoundEventArgs {
        /// <summary>
        /// Gets the agent.
        /// </summary>
        /// <value>The agent.</value>
        public IPEndPoint Agent { get; private set; }

        /// <summary>
        /// Initializes a new instance of SNMPAgentFoundEventArgs class
        /// </summary>
        /// <param name="agent">The agent</param>
        public SNMPAgentFoundEventArgs(IPEndPoint agent)
        {
            Agent = agent;
        }
    }
}

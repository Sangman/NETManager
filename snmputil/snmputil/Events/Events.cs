using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace snmputil.Events {

    /// <summary>
    /// SNMP Agent Found Handler
    /// </summary>
    /// <param name="e">SNMP Agent Found Event arguments</param>
    public delegate void SNMPAgentFoundHandler(SNMPAgentFoundEventArgs e);
}

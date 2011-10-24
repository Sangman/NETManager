using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace netscan.Events {
    /// <summary>
    /// Handler for ARP replies, there is no sender object because this handler is called by an event in a static
    /// class.
    /// </summary>
    /// <param name="e">ARP Reply Arguments</param>
    internal delegate void ARPReplyHandler(ARPReplyArgs e);    

    /// <summary>
    /// Handler for found hosts, there is no sender object because this handler is called by an event in a static
    /// class.
    /// </summary>
    /// <param name="e">Host found arguments</param>
    public delegate void HostFoundHandler(HostFoundArgs e);
}

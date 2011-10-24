using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace netscan.Events {
    /// <summary>
    /// Arguments for an ARP reply.
    /// </summary>
    internal class ARPReplyArgs : System.EventArgs {
        private String mac;
        private IPAddress ipv4;

        /// <summary>
        /// Create a new instance of ARPReplyArgs.
        /// </summary>
        /// <param name="mac">The MAC address.</param>
        /// <param name="ipv4">The IPv4 address.</param>
        public ARPReplyArgs(String mac, IPAddress ipv4) {
            this.mac = mac;
            this.ipv4 = ipv4;
        }

        /// <summary>
        /// Return the MAC address of the reply.
        /// </summary>
        public String MAC {
            get {
                return mac;
            }
        }

        /// <summary>
        /// Return the IP address of the reply.
        /// </summary>
        public IPAddress IPV4 {
            get {
                return ipv4;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace netscan {
    /// <summary>
    /// The NetworkDevice class is a representation of a device on the network.
    /// IPv4 Address, MAC Address and Hostname are stored and can be read.
    /// </summary>
    public class NetworkDevice {

        private IPAddress ipV4address;
        private String macAddress;
        private String hostname;

        public IPAddress IPV4 {
            get {
                return ipV4address;
            }
        }

        public String Mac {
            get {
                return macAddress;
            }
        }

        public String Hostname {
            get {
                return hostname;
            }
        }

        internal NetworkDevice(IPAddress ipv4, String macaddress, String hostname) {
            this.ipV4address = ipv4;
            this.macAddress = macaddress;
            this.hostname = hostname;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using netscan.Internal;
using netscan.Events;
using System.Threading;
using System.Net.Sockets;
using netscan.Exceptions;

namespace netscan {
    /// <summary>
    /// Contains methods for finding network devices.
    /// </summary>
    public static class Scanner {

        /// <summary>
        /// This event is raised when a host is discovered.
        /// </summary>
        public static event HostFoundHandler OnHostFound;

        /// <summary>
        /// Find live hosts in a network using a range of IPv4 addresses.
        /// </summary>
        /// <param name="ipLow">Lower end of IPv4 range</param>
        /// <param name="ipHigh">Higher end of IPv4 range</param>
        public static void ScanNetworkIPv4(IPAddress ipLow, IPAddress ipHigh) {
            DateTime start = DateTime.Now;

            // clear local cache
            Console.WriteLine("Clearing local cache");
            try {
                Tools.ExecuteCMD("netsh interface ip delete arpcache", true);
            }
            catch (CMDException cmd) {
                Console.WriteLine(cmd.Message);
            }

            // send arp messages
            ARP.OnARPReply += new ARPReplyHandler(ARP_OnARPReply);
            for (int i = 1; i <= 254; i++) {
                try {
                    Thread t = new Thread(new ParameterizedThreadStart(ARP.SendARPMessageIPv4));
                    t.IsBackground = true;
                    t.Start(IPAddress.Parse("10.0.0." + i));
                }
                catch (OutOfMemoryException) {
                    // wait 3 seconds for working threads to finish
                    Thread.Sleep(3000);

                    // decrease counter
                    i--;
                }
            }

            DateTime end = DateTime.Now;
            TimeSpan ts = end - start;
            Console.WriteLine("All ARP messages sent in " + ts.Seconds + "." + ts.Milliseconds + " seconds.");

            
        }

        /// <summary>
        /// ARP replies are handled here.
        /// </summary>
        /// <param name="e">ARP Reply event args</param>
        private static void ARP_OnARPReply(ARPReplyArgs e) {
            // get hostname of IP
            Console.WriteLine("ARP reply received from " + e.IPV4.ToString());
            Console.WriteLine("Attempting to look up hostname.");
            String hostname;
            try {
                IPHostEntry iphe = Dns.GetHostByAddress(e.IPV4);
                hostname = iphe.HostName;
            }
            catch (SocketException) {
                Console.WriteLine("Hostname lookup failed.");
                hostname = "UNKNOWN";
            }

            NetworkDevice nd = new NetworkDevice(e.IPV4, e.MAC, hostname);
            HostFoundArgs ha = new HostFoundArgs(nd);
            OnHostFound(ha);

        }

    }
}

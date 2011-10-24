using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Net;
using netscan.Events;

namespace netscan.Internal {
    /// <summary>
    /// Class used to send and receive ARP information
    /// </summary>
    public static class ARP {
        /// <summary>
        /// This event is raised when an ARP reply is received.
        /// </summary>
        internal static event ARPReplyHandler OnARPReply;

        [DllImport("iphlpapi.dll", ExactSpelling = true)]
        private static extern int SendARP(int DestIP, int SrcIP, byte[] pMacAddr, ref uint PhyAddrLen);

        /// <summary>
        /// Send an ARP broadcast to a specified destination.
        /// The OnARPReply Event will be raised when a message is received.
        /// </summary>
        /// <param name="ipv4">A valid IPv4 address.</param>
        public static void SendARPMessageIPv4(object ipv4) {
            IPAddress dst = IPAddress.Parse(ipv4.ToString());
            IPAddress src = IPAddress.Parse("10.0.0.1");
            byte[] macAddr = new byte[6];
            uint macAddrLen = (uint)macAddr.Length;

            //Console.WriteLine("ARP message sent to " + dst);
            if (SendARP((int)dst.Address, 0, macAddr, ref macAddrLen) == 0) {
                // arp reply received
                string[] str = new string[(int)macAddrLen];
                for (int i = 0; i < macAddrLen; i++)
                    str[i] = macAddr[i].ToString("x2");

                String mac = string.Join("-", str);

                ARPReplyArgs arpArg = new ARPReplyArgs(mac, dst);
                OnARPReply(arpArg);
            }

        }
    }
}

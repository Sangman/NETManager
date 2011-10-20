using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Security.Principal;
using System.Reflection;
using System.IO;
using System.Net.NetworkInformation;
using System.Threading;
using System.Runtime.InteropServices;

namespace HostDetector {
    
    class Program {
        [DllImport("iphlpapi.dll", ExactSpelling = true)]
        public static extern int SendARP(int DestIP, int SrcIP, byte[] pMacAddr, ref uint PhyAddrLen);

        public List<IPAddress> activeIPs;
        public List<String> activeMacs;

        public static void Main(string[] args) {            
            Console.WriteLine("HostDetector Program is attempting to detect hosts");
            Console.WriteLine("The first step in doing this is clearing your ARP cache.");

            Program prog = new Program();
            prog.activeIPs = new List<IPAddress>();
            prog.activeMacs = new List<String>();

            if (prog.ClearARPCache()) {
                Console.WriteLine("ARP cache cleared.");
            }
            else {
                Console.WriteLine("Something went wrong while trying to clear ARP cache.");
            }

            Console.WriteLine("The second step is re-acquiring ARP information. Sending ARP messages to hosts.");

                    // this needs rewriting
                    //for (int k = 64; k <= 95; k++) {
                        for (int j = 1; j <= 254; j++) {
                            try {
                                Thread t = new Thread(new ParameterizedThreadStart(prog.SendARPMessage));
                                t.Start(IPAddress.Parse("192.168.0." + j));
                            }
                            catch (OutOfMemoryException) {
                                Console.WriteLine("OOM");

                                // out of memory, just wait a few seconds for existing threads to finish
                                Thread.Sleep(1000);

                                // decrement j
                                j--;
                            }
                        }
                    //}

                    // wait a few seconds for threads to finish
                    Thread.Sleep(9000);
                    Console.WriteLine("The following IPs have been determined as being up:");
                    
                    for (int i = 0; i < prog.activeIPs.Count; i++) {
                        Console.WriteLine("=================================================");
                        Console.WriteLine("IP Address " + prog.activeIPs[i].ToString() + " is up.");
                        
                        try {
                            IPHostEntry iphe = Dns.GetHostByAddress(prog.activeIPs[i]);
                            Console.WriteLine("Hostname: " + iphe.HostName);
                        }
                        catch (SocketException) {
                            Console.WriteLine("Hostname: Couldn't determine hostname. Possibly this machine has been secured.");
                        }
                        Console.WriteLine("MAC Address: " + prog.activeMacs[i]);
                        Console.WriteLine("Device type: ");
                        Console.WriteLine("=================================================");

                    }            
        }

        public void SendARPMessage(object dest) {
            IPAddress dst = IPAddress.Parse(dest.ToString());
            byte[] macAddr = new byte[6];
            uint macAddrLen = (uint)macAddr.Length;
            if (SendARP((int)dst.Address, 0, macAddr, ref macAddrLen) == 0) {
                string[] str = new string[(int)macAddrLen];
                for (int i = 0; i < macAddrLen; i++)
                    str[i] = macAddr[i].ToString("x2");

                //Console.WriteLine(DateTime.Now.TimeOfDay + " IP Address " + dst.ToString() + " found at " + string.Join(":", str));

                activeIPs.Add(dst);
                String mac = string.Join("-", str);
                activeMacs.Add(mac.ToUpper());
            }
            else {
                //Console.WriteLine("Nothing found at " + dst.ToString());
            }
        }

        /// <summary>
        /// Clear ARP cache using CMD prompt. This requires that you elevate user rights first (call ElevateRights).
        /// </summary>
        /// <returns>True or false depending on succesful clearing</returns>
        public bool ClearARPCache() {
            try {
                ProcessStartInfo psi = new ProcessStartInfo("cmd", " /c netsh interface ip delete arpcache");
                psi.Verb = "runas";
                Process p = Process.Start(psi);
                return true;
            }
            catch (Exception) {
                return false;
            }
        }

        /// <summary>
        /// Elevate application rights to Administrator. This will cause a UAC box to pop up.
        /// </summary>
        /// <param name="createWindow">Run the elevated process in a new window.</param>
        /// <returns>True or false depending on succesful elevation.</returns>
        public bool ElevateRights(bool createWindow) {
            try {
                WindowsPrincipal principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
                bool isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);

                if (isAdmin) {
                    return true;
                }
                else {
                    // basically: give this application admin rights
                    ProcessStartInfo psiElev = new ProcessStartInfo();
                    psiElev.Verb = "runas";
                    psiElev.UserName = null;
                    psiElev.Password = null;
                    psiElev.FileName = Assembly.GetExecutingAssembly().Location;
                    psiElev.CreateNoWindow = createWindow;
                    Process pElev = Process.Start(psiElev);
                    return true;
                }
            }
            catch (Exception) {
                return false;
            }
        }
    }
}
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
using netscan;

namespace HostDetector {
    
    class Program {


        public static void Main(string[] args) {
            Scanner.OnHostFound += new netscan.Events.HostFoundHandler(Scanner_OnHostFound);
            Scanner.ScanNetworkIPv4(IPAddress.Parse("10.0.0.1"), IPAddress.Parse("10.0.0.255"));
            
            // don't terminate program:
            Console.ReadLine();
        }

        public static void Scanner_OnHostFound(netscan.Events.HostFoundArgs e) {
            Console.WriteLine(Environment.NewLine+"======================================");
            Console.WriteLine("IP "+e.Host.IPV4+" is up.");
            Console.WriteLine("MAC: " + e.Host.Mac);
            Console.WriteLine("Hostname: " + e.Host.Hostname);
            Console.WriteLine("======================================"+Environment.NewLine);
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
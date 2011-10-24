using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using netscan.Exceptions;

namespace netscan {
    /// <summary>
    /// This static class combines a number of tools for general use.
    /// </summary>
    public static class Tools {

        /// <summary>
        /// Call a CMD prompt and execute a given command. Throws CMDException upon failure.
        /// </summary>
        /// <param name="command">Command to execute.</param>
        /// <param name="adminMode">Enable admin mode for this command (this will give the user a prompt)</param>
        public static void ExecuteCMD(String command, bool adminMode) {
            try {
                ProcessStartInfo psi = new ProcessStartInfo("cmd", " /c "+command);
                if (adminMode) {
                    psi.Verb = "runas";
                }
                Process p = Process.Start(psi);
            }
            catch (Exception e) {
                throw new CMDException("Admin rights are required for the command " + command);
            }

        }

    }
}

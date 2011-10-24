using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace netscan.Events {
    /// <summary>
    /// Arguments available for a found host.
    /// </summary>
    public class HostFoundArgs {

        private NetworkDevice device;

        /// <summary>
        /// Create a new instance of HostFoundArgs.
        /// </summary>
        /// <param name="device">The found device.</param>
        public HostFoundArgs(NetworkDevice device) {
            this.device = device;
        }

        /// <summary>
        /// Get the found host.
        /// </summary>
        public NetworkDevice Host {
            get {
                return device;
            }
        }
    }
}

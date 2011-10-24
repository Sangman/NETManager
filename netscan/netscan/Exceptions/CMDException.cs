using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace netscan.Exceptions {
    public class CMDException : Exception {

        /// <summary>
        /// Create a new CMD exception
        /// </summary>
        /// <param name="message">The message of the exception.</param>
        internal CMDException(String message) : base(message) { }

    }
}

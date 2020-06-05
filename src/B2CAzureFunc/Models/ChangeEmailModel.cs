using System;
using System.Collections.Generic;
using System.Text;

namespace B2CAzureFunc.Models
{
    /// <summary>
    /// Change EmailModel
    /// </summary>
    public class ChangeEmailModel
    {
        /// <summary>
        /// New email
        /// </summary>
        public string NewEmail { get; set; }
        /// <summary>
        /// Email to be changed
        /// </summary>
        public string CurrentEmail { get; set; }

        /// <summary>
        /// Is resend
        /// </summary>
        public bool IsResend { get; set; }
    }
}
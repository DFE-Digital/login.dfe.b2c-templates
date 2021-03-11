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
        /// Object id of user
        /// </summary>
        public string ObjectId { get; set; }

        /// <summary>
        /// Is resend
        /// </summary>
        public bool IsResend { get; set; }

        /// <summary>
        /// SendTokenBackRequired
        /// </summary>
        public bool SendTokenBackRequired { get; set; }
    }
}
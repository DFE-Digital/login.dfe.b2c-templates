using System;
using System.Collections.Generic;
using System.Text;

namespace B2CAzureFunc.Models
{
    /// <summary>
    /// Signup Confirmation Model
    /// </summary>
    public class SignupConfirmationModel
    {
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Given Name
        /// </summary>
        public string GivenName { get; set; }
        /// <summary>
        /// Object Id
        /// </summary>
        public string ObjectId { get; set; }
        /// <summary>
        /// Is Resend
        /// </summary>
        public bool IsResend { get; set; }
    }
}
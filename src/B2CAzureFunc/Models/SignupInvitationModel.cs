using System;
using System.Collections.Generic;
using System.Text;

namespace B2CAzureFunc.Models
{
    /// <summary>
    /// Signup Invitation Model
    /// </summary>
    public class SignupInvitationModel
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
        /// Last Name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Customer Id
        /// </summary>
        public string CustomerId { get; set; }
        /// <summary>
        /// Is Resend
        /// </summary>
        public bool IsResend { get; set; }
    }
}
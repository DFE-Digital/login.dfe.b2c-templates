using System;
using System.Collections.Generic;
using System.Text;

namespace B2CAzureFunc.Models
{
    /// <summary>
    /// PasswordProfileMdel
    /// </summary>
    public class PasswordProfileMdel
    {
        /// <summary>
        /// password
        /// </summary>
        public object password { get; set; }
        /// <summary>
        /// forceChangePasswordNextLogin
        /// </summary>
        public bool forceChangePasswordNextLogin { get; set; }
        /// <summary>
        /// enforceChangePasswordPolicy
        /// </summary>
        public bool enforceChangePasswordPolicy { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace B2CAzureFunc.Models
{
    public class PasswordProfileMdel
    {
        public object password { get; set; }
        public bool forceChangePasswordNextLogin { get; set; }
        public bool enforceChangePasswordPolicy { get; set; }
    }
}
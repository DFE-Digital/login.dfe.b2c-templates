using System;
using System.Collections.Generic;
using System.Text;

namespace B2CAzureFunc.Models
{
    /// <summary>
    /// ValidateUserInfoModel
    /// </summary>
    public class ValidateUserInfoModel
    {
        /// <summary>
        /// Given Name
        /// </summary>
        public string GivenName { get; set; }

        /// <summary>
        /// Surname
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Day
        /// </summary>
        public string Day { get; set; }

        /// <summary>
        /// Month
        /// </summary>
        public string Month { get; set; }

        /// <summary>
        /// Year
        /// </summary>
        public string Year { get; set; }

        /// <summary>
        /// PostalCode
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// CustomerId
        /// </summary>
        public string CustomerId { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace B2CAzureFunc.Models
{
    /// <summary>
    /// User Creation Model
    /// </summary>
    public class UserCreationModel
    {
        /// <summary>
        /// First name
        /// </summary>
        public string GivenName { get; set; }

        /// <summary>
        /// Last name
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

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
        /// User id
        /// </summary>
        public string ObjectId { get; set; }

        /// <summary>
        /// Customer id
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// Is aided registration
        /// </summary>
        public bool IsAided { get; set; }
    }
}
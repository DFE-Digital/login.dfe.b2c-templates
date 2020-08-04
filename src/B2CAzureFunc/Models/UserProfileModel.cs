using System;
using System.Collections.Generic;
using System.Text;

namespace B2CAzureFunc.Models
{
    /// <summary>
    /// UserProfileModel
    /// </summary>
    public class UserProfileModel
    {
        /// <summary>
        /// B2C user objectId
        /// </summary>
        public string ObjectId { get; set; }
        /// <summary>
        /// First name
        /// </summary>
        public string GiveName { get; set; }
        /// <summary>
        /// Last name
        /// </summary>
        public string Surname { get; set; }
        /// <summary>
        /// DisplayName
        /// </summary>
        public string DisplayName { get; set; }
    }
}
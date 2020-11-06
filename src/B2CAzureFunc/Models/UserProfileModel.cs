using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Required]
        public string ObjectId { get; set; }
        /// <summary>
        /// First name
        /// </summary>
        [RegularExpression(@"^[\w'\-,.][^0-9_!¡?÷?¿\\+=@#$%ˆ&*(){}|~<>;:[\]]{1,}$", ErrorMessage = "Invalid first name")]
        public string FirstName { get; set; }
        /// <summary>
        /// Last name
        /// </summary>
        [RegularExpression(@"^[\w'\-,.][^0-9_!¡?÷?¿\\+=@#$%ˆ&*(){}|~<>;:[\]]{1,}$", ErrorMessage = "Invalid last name")]
        public string LastName { get; set; }
        /// <summary>
        /// DisplayName
        /// </summary>
        [RegularExpression(@"^[\w'\-,.][^0-9_!¡?÷?¿\\+=@#$%ˆ&*(){}|~<>;:[\]]{1,}$", ErrorMessage = "Invalid display name")]
        public string DisplayName { get; set; }
    }
}
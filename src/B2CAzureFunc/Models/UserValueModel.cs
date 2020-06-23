using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace B2CAzureFunc.Models
{
    /// <summary>
    /// UserValueModel
    /// </summary>
    public class UserValueModel
    {
        /// <summary>
        /// type
        /// </summary>
        [JsonProperty(PropertyName = "odata.type")]
        public string type { get; set; }
        /// <summary>
        /// objectType
        /// </summary>
        public string objectType { get; set; }
        /// <summary>
        /// objectId
        /// </summary>
        public string objectId { get; set; }
        /// <summary>
        /// deletionTimestamp
        /// </summary>
        public object deletionTimestamp { get; set; }
        /// <summary>
        /// accountEnabled
        /// </summary>
        public bool accountEnabled { get; set; }
        /// <summary>
        /// ageGroup
        /// </summary>
        public object ageGroup { get; set; }
        /// <summary>
        /// assignedLicenses
        /// </summary>
        public List<object> assignedLicenses { get; set; }
        /// <summary>
        /// assignedPlans
        /// </summary>
        public List<object> assignedPlans { get; set; }
        /// <summary>
        /// city
        /// </summary>
        public object city { get; set; }
        /// <summary>
        /// companyName
        /// </summary>
        public object companyName { get; set; }
        /// <summary>
        /// consentProvidedForMinor
        /// </summary>
        public object consentProvidedForMinor { get; set; }
        /// <summary>
        /// country
        /// </summary>
        public string country { get; set; }
        /// <summary>
        /// createdDateTime
        /// </summary>
        public DateTime createdDateTime { get; set; }
        /// <summary>
        /// creationType
        /// </summary>
        public string creationType { get; set; }
        /// <summary>
        /// department
        /// </summary>
        public object department { get; set; }
        /// <summary>
        /// dirSyncEnabled
        /// </summary>
        public object dirSyncEnabled { get; set; }
        /// <summary>
        /// displayName
        /// </summary>
        public string displayName { get; set; }
        /// <summary>
        /// employeeId
        /// </summary>
        public object employeeId { get; set; }
        /// <summary>
        /// facsimileTelephoneNumber
        /// </summary>
        public object facsimileTelephoneNumber { get; set; }
        /// <summary>
        /// givenName
        /// </summary>
        public string givenName { get; set; }
        /// <summary>
        /// immutableId
        /// </summary>
        public object immutableId { get; set; }
        /// <summary>
        /// isCompromised
        /// </summary>
        public object isCompromised { get; set; }
        /// <summary>
        /// jobTitle
        /// </summary>
        public object jobTitle { get; set; }
        /// <summary>
        /// lastDirSyncTime
        /// </summary>
        public object lastDirSyncTime { get; set; }
        /// <summary>
        /// legalAgeGroupClassification
        /// </summary>
        public object legalAgeGroupClassification { get; set; }
        /// <summary>
        /// mail
        /// </summary>
        public object mail { get; set; }
        /// <summary>
        /// mailNickname
        /// </summary>
        public string mailNickname { get; set; }
        /// <summary>
        /// mobile
        /// </summary>
        public object mobile { get; set; }
        /// <summary>
        /// onPremisesDistinguishedName
        /// </summary>
        public object onPremisesDistinguishedName { get; set; }
        /// <summary>
        /// onPremisesSecurityIdentifier
        /// </summary>
        public object onPremisesSecurityIdentifier { get; set; }
        /// <summary>
        /// otherMails
        /// </summary>
        public List<object> otherMails { get; set; }
        /// <summary>
        /// passwordPolicies
        /// </summary>
        public string passwordPolicies { get; set; }
        /// <summary>
        /// passwordProfile
        /// </summary>
        public PasswordProfileMdel passwordProfile { get; set; }
        /// <summary>
        /// physicalDeliveryOfficeName
        /// </summary>
        public object physicalDeliveryOfficeName { get; set; }
        /// <summary>
        /// postalCode
        /// </summary>
        public object postalCode { get; set; }
        /// <summary>
        /// preferredLanguage
        /// </summary>
        public object preferredLanguage { get; set; }
        /// <summary>
        /// provisionedPlans
        /// </summary>
        public List<object> provisionedPlans { get; set; }
        /// <summary>
        /// provisioningErrors
        /// </summary>
        public List<object> provisioningErrors { get; set; }
        /// <summary>
        /// proxyAddresses
        /// </summary>
        public List<object> proxyAddresses { get; set; }
        /// <summary>
        /// refreshTokensValidFromDateTime
        /// </summary>
        public DateTime refreshTokensValidFromDateTime { get; set; }
        /// <summary>
        /// showInAddressList
        /// </summary>
        public object showInAddressList { get; set; }
        /// <summary>
        /// signInNames
        /// </summary>
        public List<UserSignInNameModel> signInNames { get; set; }
        /// <summary>
        /// sipProxyAddress
        /// </summary>
        public object sipProxyAddress { get; set; }
        /// <summary>
        /// state
        /// </summary>
        public object state { get; set; }
        /// <summary>
        /// streetAddress
        /// </summary>
        public object streetAddress { get; set; }
        /// <summary>
        /// surname
        /// </summary>
        public string surname { get; set; }
        /// <summary>
        /// telephoneNumber
        /// </summary>
        public object telephoneNumber { get; set; }
        /// <summary>
        /// mediaEditLink
        /// </summary>
        [JsonProperty(PropertyName = "odata.mediaEditLink")]
        public string mediaEditLink { get; set; }
        /// <summary>
        /// usageLocation
        /// </summary>
        public object usageLocation { get; set; }
        /// <summary>
        /// userIdentities
        /// </summary>
        public List<object> userIdentities { get; set; }
        /// <summary>
        /// userPrincipalName
        /// </summary>
        public string userPrincipalName { get; set; }
        /// <summary>
        /// userState
        /// </summary>
        public object userState { get; set; }
        /// <summary>
        /// userStateChangedOn
        /// </summary>
        public object userStateChangedOn { get; set; }
        /// <summary>
        /// userType
        /// </summary>
        public string userType { get; set; }
    }
}

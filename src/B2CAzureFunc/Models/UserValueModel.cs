using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace B2CAzureFunc.Models
{
    public class UserValueModel
    {
        [JsonProperty(PropertyName = "odata.type")]
        public string type { get; set; }
        public string objectType { get; set; }
        public string objectId { get; set; }
        public object deletionTimestamp { get; set; }
        public bool accountEnabled { get; set; }
        public object ageGroup { get; set; }
        public List<object> assignedLicenses { get; set; }
        public List<object> assignedPlans { get; set; }
        public object city { get; set; }
        public object companyName { get; set; }
        public object consentProvidedForMinor { get; set; }
        public string country { get; set; }
        public DateTime createdDateTime { get; set; }
        public string creationType { get; set; }
        public object department { get; set; }
        public object dirSyncEnabled { get; set; }
        public string displayName { get; set; }
        public object employeeId { get; set; }
        public object facsimileTelephoneNumber { get; set; }
        public string givenName { get; set; }
        public object immutableId { get; set; }
        public object isCompromised { get; set; }
        public object jobTitle { get; set; }
        public object lastDirSyncTime { get; set; }
        public object legalAgeGroupClassification { get; set; }
        public object mail { get; set; }
        public string mailNickname { get; set; }
        public object mobile { get; set; }
        public object onPremisesDistinguishedName { get; set; }
        public object onPremisesSecurityIdentifier { get; set; }
        public List<object> otherMails { get; set; }
        public string passwordPolicies { get; set; }
        public PasswordProfileMdel passwordProfile { get; set; }
        public object physicalDeliveryOfficeName { get; set; }
        public object postalCode { get; set; }
        public object preferredLanguage { get; set; }
        public List<object> provisionedPlans { get; set; }
        public List<object> provisioningErrors { get; set; }
        public List<object> proxyAddresses { get; set; }
        public DateTime refreshTokensValidFromDateTime { get; set; }
        public object showInAddressList { get; set; }
        public List<UserSignInNameModel> signInNames { get; set; }
        public object sipProxyAddress { get; set; }
        public object state { get; set; }
        public object streetAddress { get; set; }
        public string surname { get; set; }
        public object telephoneNumber { get; set; }
        [JsonProperty(PropertyName = "odata.mediaEditLink")]
        public string mediaEditLink { get; set; }
        public object usageLocation { get; set; }
        public List<object> userIdentities { get; set; }
        public string userPrincipalName { get; set; }
        public object userState { get; set; }
        public object userStateChangedOn { get; set; }
        public string userType { get; set; }
    }
}

using System;

namespace B2CAzureFunc.Helpers
{
    /// <summary>
    ///     URLBuilder
    /// </summary>
    public static class UrlBuilder
    {
        /// <summary>
        ///     BuildUrl
        /// </summary>
        /// <param name="token"></param>
        /// <param name="b2CAuthURL"></param>
        /// <param name="b2cTenant"></param>
        /// <param name="b2cPolicyId"></param>
        /// <param name="b2cClientId"></param>
        /// <param name="b2cRedirectURI"></param>
        /// <returns>string</returns>
        public static string BuildUrl(string token,string b2CAuthURL,string b2cTenant,string b2cPolicyId,string b2cClientId,string b2cRedirectURI)
        {
            string nonce = Guid.NewGuid().ToString("n");

            return string.Format(b2CAuthURL,
                    b2cTenant,
                    b2cPolicyId,
                    b2cClientId,
                    Uri.EscapeDataString(b2cRedirectURI),
                    nonce) + "&id_token_hint=" + token;
        }
    }
}
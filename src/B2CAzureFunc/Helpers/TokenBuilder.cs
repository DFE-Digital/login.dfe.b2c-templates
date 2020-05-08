using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace B2CAzureFunc.Helpers
{
    public class TokenBuilder
    {
        public static string BuildIdToken(string email, string givenName, string surname, DateTime expiry, string requestScheme, string host, string path)
        {
            string issuer = $"{requestScheme}://{host}{path}/";

            // All parameters send to Azure AD B2C needs to be sent as claims
            IList<System.Security.Claims.Claim> claims = new List<System.Security.Claims.Claim>();
            claims.Add(new System.Security.Claims.Claim("email", email, System.Security.Claims.ClaimValueTypes.String, issuer));
            claims.Add(new System.Security.Claims.Claim("expiry", expiry.ToString(), System.Security.Claims.ClaimValueTypes.DateTime, issuer));
            claims.Add(new System.Security.Claims.Claim("givenName", givenName.ToString(), System.Security.Claims.ClaimValueTypes.String, issuer));
            claims.Add(new System.Security.Claims.Claim("surname", surname.ToString(), System.Security.Claims.ClaimValueTypes.String, issuer));

            // Note: This key phrase needs to be stored also in Azure B2C Keys for token validation
            var securityKey = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("ClientSigningKey", EnvironmentVariableTarget.Process));

            var signingKey = new SymmetricSecurityKey(securityKey);
            SigningCredentials signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            // Create the token
            JwtSecurityToken token = new JwtSecurityToken(
                    issuer,
                    Environment.GetEnvironmentVariable("B2CClientId", EnvironmentVariableTarget.Process),
                    claims,
                    DateTime.Now,
                    expiry,
                    signingCredentials);

            // Get the representation of the signed token
            JwtSecurityTokenHandler jwtHandler = new JwtSecurityTokenHandler();

            return jwtHandler.WriteToken(token);
        }

        public static string BuildIdToken(string email, DateTime expiry, string requestScheme, string host, string path, string objectId)
        {
            string issuer = $"{requestScheme}://{host}{path}/";

            // All parameters send to Azure AD B2C needs to be sent as claims
            IList<System.Security.Claims.Claim> claims = new List<System.Security.Claims.Claim>();
            claims.Add(new System.Security.Claims.Claim("email", email, System.Security.Claims.ClaimValueTypes.String, issuer));
            claims.Add(new System.Security.Claims.Claim("expiry", expiry.ToString(), System.Security.Claims.ClaimValueTypes.DateTime, issuer));
            claims.Add(new System.Security.Claims.Claim("objectId", objectId.ToString(), System.Security.Claims.ClaimValueTypes.String, issuer));

            // Note: This key phrase needs to be stored also in Azure B2C Keys for token validation
            var securityKey = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("ClientSigningKey", EnvironmentVariableTarget.Process));

            var signingKey = new SymmetricSecurityKey(securityKey);
            SigningCredentials signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            // Create the token
            JwtSecurityToken token = new JwtSecurityToken(
                    issuer,
                    Environment.GetEnvironmentVariable("B2CClientId", EnvironmentVariableTarget.Process),
                    claims,
                    DateTime.Now,
                    expiry,
                    signingCredentials);

            // Get the representation of the signed token
            JwtSecurityTokenHandler jwtHandler = new JwtSecurityTokenHandler();

            return jwtHandler.WriteToken(token);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Toast.Gamebase.Internal.Auth
{
    public class IdPUriBuilder
    {
        private readonly string _baseUrl;
        private readonly Dictionary<string, string> _parameters = new Dictionary<string, string>();

        public IdPUriBuilder(string providerName, string clientId, string subCode)
        {
            _baseUrl = GamebaseLaunchingImplementation.Instance.GetLaunchingInformations().launching.app.loginUrls.gamebaseUrl;
            _parameters["socialNetworkingServiceCode"] = providerName;
            _parameters["clientId"] = clientId;
            _parameters["callbackType"] = "INTERNAL";

            if (providerName.Equals(GamebaseAuthProvider.LINE))
            {
                _parameters["socialNetworkingServiceSubCode"] = subCode;
            }
            else if (providerName.Equals(GamebaseAuthProvider.APPLEID))
            {
                _parameters["socialNetworkingServiceSubCode"] = "sign_in_with_apple_js";
            }
            else if (providerName.Equals(GamebaseAuthProvider.TWITTER))
            {
                _parameters["authorizationProtocol"] = "oauth2";
                _parameters["codeChallenge"] = GeneratePKCECodeChallenge();
            }
            else if (providerName.Equals(GamebaseAuthProvider.HANGAME))
            {
                _parameters["socialNetworkingServiceSubCode"] = "hangame";
            }
        }

        /// <summary>
        /// cryptographically random string using the characters A-Z, a-z, 0-9, and the punctuation characters -._~ (hyphen, period, underscore, and tilde),
        /// between 43 and 128 characters long.
        /// <para><see href="https://www.oauth.com/oauth2-servers/pkce/authorization-request/">Protecting Apps with PKCE: Authorization Request</see></para>
        /// 
        /// Code Challenge has two modes (plain or SHA-256). Plain is used to maintain the same specifications as the member server.
        /// <para><see href="https://nhnent.dooray.com/share/pages/ItYTumRbQ16jSjpEc0rfog/3919056334930446922">Member spec</see></para>
        /// </summary>
        /// <returns>code challenge</returns>
        private string GeneratePKCECodeChallenge()
        {
            using var generator = RandomNumberGenerator.Create();
            var randomBytes = new byte[33];
            generator.GetBytes(randomBytes);

            return Convert.ToBase64String(randomBytes)
                .Replace("+", "-")
                .Replace("/", "_")
                .TrimEnd('=');
        }

        public IdPUriBuilder AppendTicket(string ticket)
        {
            _parameters["state"] = ticket;
            return this;
        }

        public string Build()
        {
            var queryString = string.Join("&",
                _parameters.Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"));

            return $"{_baseUrl}?{queryString}";
        }
    }
}

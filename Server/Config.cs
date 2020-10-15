using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServerAspNetIdentity
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };


        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("api1", "My API")
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "mvc",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    AllowOfflineAccess = true,
                    RequirePkce = false,
                    //AllowAccessTokensViaBrowser = true,
                    //AlwaysIncludeUserClaimsInIdToken = true,

                    RequireConsent = true,
                    // where to redirect to after login
                    RedirectUris = { "https://localhost:44392/signin-oidc" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "https://localhost:44392/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1"
                    }
                }
                //// machine to machine client
                //new Client
                //{
                //    ClientId = "client",
                //    ClientSecrets = { new Secret("secret".Sha256()) },

                //    AllowedGrantTypes = GrantTypes.ClientCredentials,
                //    // scopes that client has access to
                //    AllowedScopes = { "api1" }
                //},
                
                //// interactive ASP.NET Core MVC client
                //new Client
                //{
                //    ClientId = "mvc",
                //    ClientSecrets = { new Secret("secret".Sha256()) },

                //    AllowedGrantTypes = GrantTypes.Code,
                    
                //    // where to redirect to after login
                //    RedirectUris = { "https://localhost:5002/signin-oidc" },

                //    // where to redirect to after logout
                //    PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc" },

                //    AllowedScopes = new List<string>
                //    {
                //        IdentityServerConstants.StandardScopes.OpenId,
                //        IdentityServerConstants.StandardScopes.Profile,
                //        "api1"
                //    }
                //}
            };
    }
}
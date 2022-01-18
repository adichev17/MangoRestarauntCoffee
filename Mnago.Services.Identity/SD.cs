﻿using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Mnago.Services.Identity
{
    //class static objects
    public class SD
    {
        public const string Admin = "Admin";
        public const string Customer = "Customer";

        public static IEnumerable<IdentityResource> IdentityResource => new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Email(),
            new IdentityResources.Profile()
        };

        public static IEnumerable<ApiScope> ApiScopes => new List<ApiScope>
        {
            new ApiScope("mango", "Mango Server"),
            new ApiScope(name: "read", displayName: "Read your data"),
            new ApiScope(name: "write", displayName: "Write your data"),
            new ApiScope(name: "delete", displayName: "Delete your data"),
        };

        //The client requests a token from the authentication server, which uses authentication, or access to some resource
        public static IEnumerable<Client> Clients => new List<Client>
        {
            new Client
            {
                ClientId = "client",
                ClientSecrets = { new Secret("secret".Sha256())},
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = {"read", "write", "profile"} //profile inbuilt type.
            },
            new Client
            {
                ClientId = "mango",
                ClientSecrets = { new Secret("secret".Sha256())},
                AllowedGrantTypes = GrantTypes.Code,
                RedirectUris={ "https://localhost:44384/signin-oidc" },
                PostLogoutRedirectUris={ "https://localhost:44384/signout-callback-oidc" },
                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    "mango"
                } //profile inbuilt type.
            }
        };
    }
}

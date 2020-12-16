﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {    new ApiScope("api1", "My API"),new ApiScope("api2", "My API2")};

        public static IEnumerable<Client> Clients =>
            new Client[] { 
                new Client {
                   ClientId = "client",

                         // no interactive user, use the clientid/secret for authentication   
                          AllowedGrantTypes = GrantTypes.ClientCredentials,
                  
                         // secret for authentication
                         ClientSecrets =
                         {
                             new Secret("secret".Sha256())
                         },
                  
                         // scopes that client has access to
                         AllowedScopes = { "api1","api2" }
                     }, 
                new Client {
                         ClientId = "mvc",
                       
                         // no interactive user, use the clientid/secret for authentication
                         AllowedGrantTypes = GrantTypes.Code,
                       
                         // secret for authentication
                         ClientSecrets =
                         {
                             new Secret("secret".Sha256())
                         },
                          RedirectUris = { "https://localhost:5002/signin-oidc" },
                          PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc" },
                          // 允许刷新令牌 
                          AllowOfflineAccess=true,
                         // scopes that client has access to
                         AllowedScopes = {  
                        IdentityServerConstants.StandardScopes.OpenId, IdentityServerConstants.StandardScopes.Profile,
                        "api1"
                    }
                },
                new Client
                         {
                             ClientId = "js",
                             ClientName = "JavaScript Client",
                             AllowedGrantTypes = GrantTypes.Code,
                             RequireClientSecret = false,
                         
                             RedirectUris =           { "https://localhost:5003/callback.html" },
                             PostLogoutRedirectUris = { "https://localhost:5003/index.html" },
                             AllowedCorsOrigins =     { "https://localhost:5003" },
                         
                             AllowedScopes =
                             {
                                 IdentityServerConstants.StandardScopes.OpenId,
                                 IdentityServerConstants.StandardScopes.Profile,
                                 "api1"
                             }
                         }
                         
            };
    }
}
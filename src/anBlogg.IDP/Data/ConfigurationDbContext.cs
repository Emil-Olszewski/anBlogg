using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Extensions;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using System;

namespace anBlogg.IDP.Data
{
    public class ConfigurationDbContext : IdentityServer4.EntityFramework.DbContexts.ConfigurationDbContext
    {
        private readonly ConfigurationStoreOptions storeOptions;

        public ConfigurationDbContext(DbContextOptions<IdentityServer4.EntityFramework.DbContexts.ConfigurationDbContext> options, ConfigurationStoreOptions storeOptions) : base(options, storeOptions)
        {
            this.storeOptions = storeOptions;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureClientContext(storeOptions);
            modelBuilder.ConfigureResourcesContext(storeOptions);

            base.OnModelCreating(modelBuilder);

            ClientSeed(modelBuilder);
        }

        private void ClientSeed(ModelBuilder builder)
        {
            builder.Entity<ApiResource>().HasData(
                new ApiResource
                {
                    Id = 1,
                    Name = "anBloggApi",
                    DisplayName = "anBlogg API",

                });

            builder.Entity<ApiScope>().HasData(
                new ApiScope
                {
                    Id = 1,
                    Name = "anBloggApi",
                    DisplayName = "anBlogg API",
                    Description = null,
                    Required = false,
                    Emphasize = false,
                    ShowInDiscoveryDocument = true,
                    ApiResourceId = 1
                });

            builder.Entity<IdentityResource>().HasData(
                new IdentityResource
                {
                    Id = 1,
                    Enabled = true,
                    Name = "openid",
                    DisplayName = "Your user identifier",
                    Description = null,
                    Required = true,
                    Emphasize = false,
                    ShowInDiscoveryDocument = true,
                    Created = DateTime.UtcNow,
                    Updated = null,
                    NonEditable = false

                },
                new IdentityResource
                {
                    Id = 2,
                    Enabled = true,
                    Name = "profile",
                    DisplayName = "User profile",
                    Description = "Your user profile information",
                    Required = false,
                    Emphasize = true,
                    ShowInDiscoveryDocument = true,
                    Created = DateTime.UtcNow,
                    Updated = null,
                    NonEditable = false

                });

            builder.Entity<IdentityClaim>().HasData(
                new IdentityClaim
                {
                    Id = 1,
                    IdentityResourceId = 1,
                    Type = "sub"
                },
                new IdentityClaim
                {
                    Id = 2,
                    IdentityResourceId = 2,
                    Type = "email"
                },
                new IdentityClaim
                {
                    Id = 3,
                    IdentityResourceId = 2,
                    Type = "website"
                },
                new IdentityClaim
                {
                    Id = 4,
                    IdentityResourceId = 2,
                    Type = "given_name"
                },
                new IdentityClaim
                {
                    Id = 5,
                    IdentityResourceId = 2,
                    Type = "family_name"
                },
                new IdentityClaim
                {
                    Id = 6,
                    IdentityResourceId = 2,
                    Type = "name"
                });

            builder.Entity<Client>().HasData(
                new Client
                {
                    Id = 1,
                    Enabled = true,
                    ClientId = "spa-client",
                    ClientName = "anBlogg web app",
                    ProtocolType = "oidc",
                    RequireClientSecret = false,
                    RequirePkce = true,
                    RequireConsent = true,
                    Description = null,
                    AllowRememberConsent = true,
                    AlwaysIncludeUserClaimsInIdToken = false,
                    AllowAccessTokensViaBrowser = true,
                    AllowOfflineAccess = false
                });

            builder.Entity<ClientGrantType>().HasData(
                new ClientGrantType
                {
                    Id = 1,
                    GrantType = "authorization_code",
                    ClientId = 1
                });

            builder.Entity<ClientScope>().HasData(
                new ClientScope
                {
                    Id = 1,
                    Scope = "profile",
                    ClientId = 1
                },
                new ClientScope
                {
                    Id = 2,
                    Scope = "openid",
                    ClientId = 1
                },
                new ClientScope
                {
                    Id = 3,
                    Scope = "anBloggApi",
                    ClientId = 1
                });

            builder.Entity<ClientPostLogoutRedirectUri>().HasData(
                new ClientPostLogoutRedirectUri 
                {
                    Id = 1,
                    ClientId = 1,
                    PostLogoutRedirectUri = "http://localhost:4200/signout-callback"
                });

            builder.Entity<ClientRedirectUri>().HasData(
                new ClientRedirectUri
                {
                    Id = 1,
                    ClientId = 1,
                    RedirectUri = "http://localhost:4200/signin-callback"
                });

            builder.Entity<ClientCorsOrigin>().HasData(
                new ClientCorsOrigin
                {
                    Id = 1,
                    ClientId = 1,
                    Origin = "http://localhost:4200"
                });
        }
    }
}

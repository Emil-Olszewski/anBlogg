using anBlogg.IDP.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace anBlogg.IDP.Data
{
    public class anBloggIDPContext : IdentityDbContext<ApplicationUser>
    {
        public anBloggIDPContext(DbContextOptions<anBloggIDPContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            UserSeed(builder);
        }

        private void UserSeed(ModelBuilder builder)
        {
            var password = "Long123$Password";

            var alice = new ApplicationUser
            {
                Id = "1",
                UserName = "Alice",
                NormalizedUserName = "ALICE",
                Email = "alice.smith@outlook.com",
                NormalizedEmail = "alice.smith@outlook.com".ToUpper(),
                EmailConfirmed = true
            };

            alice.PasswordHash = new PasswordHasher<ApplicationUser>()
                .HashPassword(alice, password);

            builder.Entity<ApplicationUser>().HasData(alice);

            builder.Entity<IdentityUserClaim<string>>().HasData(
                new IdentityUserClaim<string>
                {
                    Id = 1,
                    UserId = "1",
                    ClaimType = "name",
                    ClaimValue = "Alice Smith"
                },
                new IdentityUserClaim<string>
                {
                    Id = 2,
                    UserId = "1",
                    ClaimType = "given_name",
                    ClaimValue = "Alice"
                },
                new IdentityUserClaim<string>
                {
                    Id = 3,
                    UserId = "1",
                    ClaimType = "family_name",
                    ClaimValue = "Smith"
                },
                new IdentityUserClaim<string>
                {
                    Id = 4,
                    UserId = "1",
                    ClaimType = "email",
                    ClaimValue = "alice.smith@outlook.com"
                },
                new IdentityUserClaim<string>
                {
                    Id = 5,
                    UserId = "1",
                    ClaimType = "website",
                    ClaimValue = "http://alicesmith.com"
                },
                new IdentityUserClaim<string>
                {
                    Id = 6,
                    UserId = "1",
                    ClaimType = "email_verified",
                    ClaimValue = true.ToString()
                },
                new IdentityUserClaim<string>
                {
                    Id = 7,
                    UserId = "1",
                    ClaimType = "address",
                    ClaimValue = @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg',
                        'postal_code': 69118, 'country': 'Germany' }"
                },
                new IdentityUserClaim<string>
                {
                    Id = 8,
                    UserId = "1",
                    ClaimType = "location",
                    ClaimValue = "somewhere"
                }
            );
        }
    }
}
using System.Reflection.Emit;
using ConfigurationLibrary.UI.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ConfigurationLibrary.UI.Data.Context
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppRole>().HasData(
                new AppRole { Id = 1, Name = "SuperAdmin", NormalizedName = "SUPERADMIN" },
                new AppRole { Id = 2, Name = "Admin", NormalizedName = "ADMIN" }
            );

            builder.Entity<AppUser>().HasData(
                new AppUser
                {
                    Id = 1,
                    FullName = "Selçuk ARSLAN",
                    UserImage = null,
                    RegisterDate = new DateTimeOffset(2024, 5, 6, 14, 6, 58, 107, TimeSpan.Zero),
                    IsActive = true,
                    UserName = "slckarslan93@gmail.com",
                    NormalizedUserName = "SLCKARSLAN93@GMAIL.COM",
                    Email = "slckarslan93@gmail.com",
                    NormalizedEmail = "SLCKARSLAN93@GMAIL.COM",
                    EmailConfirmed = false,
                    PasswordHash = "AQAAAAIAAYagAAAAEKF0ap/M3i1GsUVEIkYKBOd+Pa377Z2OJEV8htETgCV0+9Kj5ROgpGfViZYx6cW5Fg==",
                    SecurityStamp = "J7LERNAJ53RTILDXUQZRWMRVI4OOSMOC",
                    ConcurrencyStamp = "5fee1238-3e67-4d95-9486-a2b4809e3d1a",
                    PhoneNumber = "05315996173",
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0
                });

          
        }
    }
}

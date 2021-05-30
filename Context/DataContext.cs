using ApiSkeleton.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;
using System.Linq;

namespace ApiSkeleton.Context
{
    //https://docs.microsoft.com/ko-kr/ef/core/cli/dotnet
    public class DataContext : DbContext
    {

        public DbSet<User> UserDatas { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is BaseEntity &&
                            (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((BaseEntity)entityEntry.Entity).UpdatedDate = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseEntity)entityEntry.Entity).CreatedDate = DateTime.Now;
                }
            }

            return base.SaveChanges();
        }

        // ref : https://docs.microsoft.com/ko-kr/ef/core/modeling/relationships?tabs=fluent-api%2Cfluent-api-simple-key%2Csimple-key
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCharSet(charSet: CharSet.Utf8);

            modelBuilder.Entity<User>(e =>
            {
                e.HasKey(e => e.Id);
                e.Property(e => e.Email).IsRequired();
                e.Property(e => e.Password).IsRequired();

                e.Property(e => e.IsActive).HasDefaultValue(false);

                e.HasMany(usr => usr.Roles)
                    .WithMany(r => r.Users)
                    .UsingEntity<UserRole>(
                        j => j.HasOne(ur => ur.Role)
                        .WithMany(r => r.UserRoles)
                        .HasForeignKey(ur => ur.RoleId),
                        j => j.HasOne(ur => ur.User)
                        .WithMany(r => r.UserRoles)
                        .HasForeignKey(ur => ur.UserId),
                        j => j.HasKey(t => new { t.RoleId, t.UserId })
                        );
            });

            modelBuilder.Entity<UserRole>(e =>
            {
                e.HasKey(ur => new { ur.UserId, ur.RoleId });
            });


            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    Id = 1,
                    Name = "Admin"
                },
                new Role
                {
                    Id = 2,
                    Name = "Staff"
                },
                new Role
                {
                    Id = 3,
                    Name = "User"
                },
                new Role
                {
                    Id = 4,
                    Name = "Guest"
                });

            var hasher = new PasswordHasher<User>();

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    NickName = "관리자",
                    Email = "admin@skeleton.com",
                    Password = hasher.HashPassword(null, "admin"),
                    IsActive = true,
                },
                new User
                {
                    Id = 2,
                    NickName = "게스트",
                    Email = "guest@skeleton.com",
                    Password = hasher.HashPassword(null, "guest"),
                    IsActive = true
                }
            );

            modelBuilder.Entity<UserRole>().HasData(
                new UserRole
                {
                    RoleId = 1, // for admin username
                    UserId = 1  // for admin role
                },
                new UserRole
                {
                    RoleId = 4, // for staff username
                    UserId = 2  // for staff role
                }
            );

            base.OnModelCreating(modelBuilder);

        }
    }
}

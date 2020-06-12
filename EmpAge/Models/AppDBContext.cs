using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmpAge.Models
{
    public class AppDBContext : IdentityDbContext
    {
       /* public DbSet<IdentityRole> Roles { get; set; }
        public DbSet<IdentityUser> Users { get; set; }
        public DbSet<IdentityUserRole<string>> UserRoles { get; set; }*/
        public DbSet<Vacancy> Vacancies { get; set; }
        public DbSet<Summary> Summaries { get; set; }

        public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        /*protected override void OnModelCreating(ModelBuilder builder)
        {

            IdentityRole applicantRole = new IdentityRole("applicant");
            IdentityRole employerRole = new IdentityRole("employer");
            IdentityRole adminRole = new IdentityRole("admin");
            IdentityRole moderRole = new IdentityRole("moder");

            builder.Entity<IdentityRole>().HasData(new IdentityRole[]
            {
                applicantRole, employerRole, adminRole, moderRole
            });


            IdentityUser admin = new IdentityUser { Email = "admin@mail.ru", UserName = "admin@mail.ru" };
            IdentityUser moder = new IdentityUser { Email = "moder@mail.ru", UserName = "moder@mail.ru" };
            var _passwordHasher = new PasswordHasher<IdentityUser>();
            string adminHashPas = _passwordHasher.HashPassword(admin, "Mnbvcxz1@");
            string moderHashPas = _passwordHasher.HashPassword(moder, "Mnbvcxz1@");

            admin.PasswordHash = adminHashPas;
            moder.PasswordHash = moderHashPas;

            IdentityUserRole<string> userRoleAdmin = new IdentityUserRole<string> { RoleId = adminRole.Id, UserId = admin.Id };
            IdentityUserRole<string> userRoleModer = new IdentityUserRole<string> { RoleId = moderRole.Id, UserId = moder.Id };

            builder.Entity<IdentityUser>().HasData(new IdentityUser[] { admin, moder });
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>[] { userRoleAdmin, userRoleModer });

            base.OnModelCreating(builder);
        }*/
    }
}

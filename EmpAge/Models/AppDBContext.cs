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
        public DbSet<Vacancy> Vacancies { get; set; }
        public DbSet<Summary> Summaries { get; set; }
        public DbSet<Proposal> Proposals { get; set; }

        public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}

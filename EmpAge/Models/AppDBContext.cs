using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmpAge.Controllers;
using Microsoft.Extensions.Logging;
using System.IO;

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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(LogFactory);
        }

        public static readonly ILoggerFactory LogFactory = LoggerFactory.Create(builder =>
        {
            builder.AddFilter((category, level) =>
                category == DbLoggerCategory.Database.Command.Name)
            .AddProvider(new FileLoggerProvider(Path.Combine(Directory.GetCurrentDirectory(), "DBlogger.txt")));
        });
    }
}

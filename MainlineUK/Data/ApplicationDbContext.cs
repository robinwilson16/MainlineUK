using System;
using System.Collections.Generic;
using System.Text;
using MainlineUK.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MainlineUK.Data
{
    //public class ApplicationDbContext : IdentityDbContext
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationRole> ApplicationRole { get; set; }
        public DbSet<Photo> Photo { get; set; }
        public DbSet<StocklistImport> StocklistImport { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Needed to set default value as current date
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<StocklistImport>()
                .Property(s => s.DateImported)
                .HasDefaultValueSql("GETDATE()");

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Photo>()
                .Property(p => p.DateImported)
                .HasDefaultValueSql("GETDATE()");
        }

        public DbSet<MainlineUK.Models.CarRequestSell> CarRequestSell { get; set; }
    }
}

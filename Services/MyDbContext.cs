using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//...
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GrandTravelPackages.Models;

namespace GrandTravelPackages.Services
{
    public class MyDbContext:IdentityDbContext
    {
        public DbSet<Customer> CustomerTbl { get; set; }
        public DbSet<TravelProvider> TravelProviderTbl { get; set; }
        public DbSet<Package> PackageTbl { get; set; }
        public DbSet<Order> OrderTbl { get; set; }
        public DbSet<Feedback> FeedbackTbl { get; set; }


        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //    modelBuilder.Entity<CustomerProvider>()
        //        .HasKey(t => new { t.CustomerId, t.TravelProviderId });
        //}
        protected override void OnConfiguring(DbContextOptionsBuilder option)
        {
            option.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB ; Database=DbGTP; Trusted_Connection=True");
        }
       
    }
}

using EmployeeManagement2.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { 
        }
          public  DbSet<Employee> Employees{ get; set; }
          public DbSet<ApplicationUser> applicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Seed();
            ////    modelBuilder.Entity<Employee>().HasData(
            ////        new Employee()
            ////        {
            ////            Id = 1,
            ////            Name = "Chris",
            ////            Department = Dept.IT,
            ////            Email = "amaeme5873@yahoo.com",
            ////            PhotoPath = "car.jpg"
            ////        },

            ////        new Employee()
            ////        {
            ////            Id = 2,
            ////            Name = "Christo",
            ////            Department = Dept.HR,
            ////            Email = "amaeme5873@yahoo.com",
            ////            PhotoPath = "car.jpg"
            ////        }
            ////        );
        }

    }
}

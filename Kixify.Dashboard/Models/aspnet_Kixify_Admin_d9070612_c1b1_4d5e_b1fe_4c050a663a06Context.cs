using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Kixify.Dashboard.Models
{
    public partial class aspnet_Kixify_Admin_d9070612_c1b1_4d5e_b1fe_4c050a663a06Context : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            //optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=aspnet-Kixify.Admin-d9070612-c1b1-4d5e-b1fe-4c050a663a06;Trusted_Connection=True;MultipleActiveResultSets=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
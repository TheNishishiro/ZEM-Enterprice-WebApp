using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using ZEM_Enterprice_WebApp.Data.Tables;

namespace ZEM_Enterprice_WebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<MyUser>
    {
        public DbSet<Technical> Technical { get; set; }
        public DbSet<Dostawa> Dostawa { get; set; }
        public DbSet<VTMagazyn> VTMagazyn { get; set; }
        public DbSet<PendingChangesTechnical> PendingChangesTechnical { get; set; }
        public DbSet<MissingFromTech> MissingFromTech { get; set; }
        public DbSet<VtToDostawa> VtToDostawa { get; set; }
        public DbSet<PendingDostawa> PendingDostawa { get; set; }
        public DbSet<ScanCache> ScanCache { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<VTMagazyn>()
                .HasOne(d => d.Technical)
                .WithMany(c => c.VTMagazyns)
                .IsRequired();

            builder.Entity<Dostawa>()
                .HasOne(d => d.Technical)
                .WithMany(c => c.Dostawas)
                .IsRequired();

            builder.Entity<VtToDostawa>().HasKey(c => new { c.DostawaId, c.VTMagazynId });
            builder.Entity<VtToDostawa>()
                .HasOne(c => c.Dostawa)
                .WithMany(c => c.Skany).OnDelete(DeleteBehavior.NoAction)
                .HasForeignKey(c => c.DostawaId);
            builder.Entity<VtToDostawa>()
                .HasOne(c => c.VTMagazyn)
                .WithMany(c => c.Dostawy).OnDelete(DeleteBehavior.NoAction)
                .HasForeignKey(c => c.VTMagazynId);

            builder.Entity<Technical>().HasQueryFilter(c => c.Deleted == false);

            base.OnModelCreating(builder);
        }


    }
}

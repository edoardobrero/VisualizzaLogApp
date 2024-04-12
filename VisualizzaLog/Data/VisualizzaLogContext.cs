using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VisualizzaLog.Models;

namespace VisualizzaLog.Data
{
    public class VisualizzaLogContext : DbContext
    {
        public VisualizzaLogContext (DbContextOptions<VisualizzaLogContext> options)
            : base(options)
        {
        }

        public DbSet<VisualizzaLog.Models.Connection> Connections { get; set; } = default!;
        public DbSet<VisualizzaLog.Models.Arplog> Arplogs { get; set; } = default!;
        public DbSet<VisualizzaLog.Models.Rule> Rules { get; set; } = default!;
        public DbSet<VisualizzaLog.Models.Violation> Violations { get; set; }
        public DbSet<VisualizzaLog.Models.FileHash> FileHashes { get; set; }
        public DbSet<VisualizzaLog.Models.ArplogViolation> ArplogViolation { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Connection>().Property(x => x.ConnectionTimestamp).HasDefaultValueSql("GETDATE()");
            //modelBuilder.Entity<Arplog>().Property(x => x.ArplogTimestamp).HasDefaultValueSql("GETDATE()");
        }
        
    }
}

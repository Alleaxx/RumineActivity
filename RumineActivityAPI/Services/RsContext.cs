using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using RumineActivityAPI.Models;
using RumineActivity.Core;
using RumineActivity.Core.Models;

namespace RumineActivityAPI.Services
{
    public class RsContext : DbContext
    {
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Post> Posts { get; set; }

        public RsContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Topic>()
                .HasMany(t => t.Messages)
                .WithOne(m => m.Topic)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
    public class RsContextConfig : IDesignTimeDbContextFactory<RsContext>
    {
        public RsContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<RsContext>();

            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");
            IConfigurationRoot config = builder.Build();

            string connectionString = config.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlite(connectionString,
                opts => opts.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds));
            return new RsContext(optionsBuilder.Options);
        }
    }
}

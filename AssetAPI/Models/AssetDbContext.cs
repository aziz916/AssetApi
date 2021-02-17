using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetAPI.Models
{
    public class AssetDbContext : DbContext
    {
        public AssetDbContext() { }
        public AssetDbContext(DbContextOptions<AssetDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder.LogTo(Console.WriteLine);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Asset>()
                .HasAlternateKey(asset => asset.name)
                .HasName("AlternateKey_AssetName");

            //modelBuilder.Entity<AssetProperty>()
            //   .HasAlternateKey(asset_property => asset_property.name)
            //   .HasName("AlternateKey_AssetPropertyName");

            modelBuilder.Entity<AssetProperty>()
               .Property(asset_property => asset_property.time_stamp)
               .HasDefaultValue(DateTime.MinValue);
        }
        public DbSet<AssetProperty> asset_property { get; set; }
        public DbSet<Asset> asset { get; set; }

    }
}

using Microsoft.EntityFrameworkCore;
using SamuraiApp.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SamuraiApp.Data
{
    public class SamuraiContext : DbContext
    {
        public DbSet<Samurai> Samurais { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Clan> Clans { get; set; }
        public DbSet<Battle> Battles { get; set; }
        //dont create Horses dbset,because we done want to change that from business logic.Horse table will be crated automatically

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source= (localdb)\\MSSQLLocalDB; Initial Catalog = SamuraiAppData");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)//fluent mapping API
        {
            modelBuilder.Entity<SamuraiBattle>().HasKey(s => new { s.SamuraiId, s.BattleId });//Many to many relationship
            modelBuilder.Entity<Horse>().ToTable("Horses");//to keep the table name Horses insted of Horse (class name)
        }
    }
}

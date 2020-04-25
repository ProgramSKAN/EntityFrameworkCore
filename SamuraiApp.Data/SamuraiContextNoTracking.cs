using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SamuraiApp.Domain;

namespace SamuraiApp.Data
{
    public class SamuraiContextNoTracking : DbContext
    {
        public SamuraiContextNoTracking()
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            //since change tracking is expensive, we can avoid tracking if we need
            //improves performance
        }
        public DbSet<Samurai> Samurais { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Clan> Clans { get; set; }
        public DbSet<Battle> Battles { get; set; }
        //dont create Horses dbset,because we done want to change that from business logic.Horse table will be crated automatically

        //use below logger factory instead of sql profiler for logging. but in in actual asp.net core app it has inbuilt.so dont use here.its just demo
        public static readonly ILoggerFactory ConsoleLoggerFactory
          = LoggerFactory.Create(builder =>
          {
              builder
                .AddFilter((category, level) =>
                    category == DbLoggerCategory.Database.Command.Name &&
                    level == LogLevel.Information)
                .AddConsole();
          });

        /// //////////////////////////////////////////////////////////////////////


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLoggerFactory(ConsoleLoggerFactory)
                .UseSqlServer("Data Source= (localdb)\\MSSQLLocalDB; Initial Catalog = SamuraiAppData"/*,options=>options.MaxBatchSize(150)*/)//sql server has its limitations under bulk transactions like Defaultnetworkpacketsizebytes=4096,maximumscriptlength=65536*Defaultnetworkpacketsizebytes/2,maxparametercount=2100,maxrowcount=1000;so we can set using options
                 .EnableSensitiveDataLogging(true);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)//fluent mapping API
        {
            modelBuilder.Entity<SamuraiBattle>().HasKey(s => new { s.SamuraiId, s.BattleId });//Many to many relationship
            modelBuilder.Entity<Horse>().ToTable("Horses");//to keep the table name Horses insted of Horse (class name)
        }
    }
}


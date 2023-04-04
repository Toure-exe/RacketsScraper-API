using Microsoft.EntityFrameworkCore;
using RacketsScrapper.Domain;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RacketsScrapper.Infrastructure
{
    public class RacketDbContext : DbContext
    {
        public DbSet<Racket> Rackets { get; set; }
        public DbSet<FilteredRacket> FilteredRackets { get; set; }
        /*
         *  se faccio Add-Migration o update-database con il costruttore mi esce un errore (console app)
         */
       // private readonly IConfiguration _configuration;
        public RacketDbContext(DbContextOptions<RacketDbContext> options) : base(options)
        {

        }

        public RacketDbContext()
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<FilteredRacket>(rck =>
            {
                rck.HasNoKey();
                rck.ToView("Filtered_racket");
            });
        }

    }
}

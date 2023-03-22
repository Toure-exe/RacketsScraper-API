using Microsoft.EntityFrameworkCore;
using RacketsScrapper.Domain;
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
        /*
         *  faccio Add-Migration o update-database con il costruttore mi esce un errore
         */

       /* public RacketDbContext(DbContextOptions<RacketDbContext> options) : base(options)
        {

        }

        public RacketDbContext()
        {
            
        }*/

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source = AK24730\\MSSQLSERVER01; Initial Catalog= db_racchette; Integrated Security=true; TrustServerCertificate=True");
        }

    }
}

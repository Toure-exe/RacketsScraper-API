using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RacketsScrapper.Infrastructure
{
    internal class RolesConfig : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
            
                new IdentityRole
                {
                    Name = "Utente Normale",
                    NormalizedName = "UTENTE NORMALE"
                },
                new IdentityRole
                {
                    Name = "Amministratore",
                    NormalizedName = "AMMINISTRATORE"
                }, 
                new IdentityRole
                {
                    Name = "Utente con privileggi",
                    NormalizedName = "UTENTE CON PRIVILEGGI"
                }

            );
        }
    }
}

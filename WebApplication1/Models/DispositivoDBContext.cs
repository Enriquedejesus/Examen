using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.Controllers;


namespace WebApplication1.Models
{
    public class DispositivoDBContext : DbContext
    {
        
        public DispositivoDBContext(DbContextOptions<DispositivoDBContext>
                                     options):base(options)
                            {

                            }

        public DbSet<Dispositivo>?Dispositivos{get;set;}
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Usuarios> Usuarios { get; set; }
        }
}
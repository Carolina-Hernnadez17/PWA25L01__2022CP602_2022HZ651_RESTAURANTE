using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace L01_2022CP602_2022HZ651.Models
{
    public class RestauranteContext : DbContext
    {
        public RestauranteContext(DbContextOptions<RestauranteContext> options) : base(options)
        {

        }
        public DbSet<clientes> clientes { get; set; }
        public DbSet<motoristas> motoristas { get; set; }
        public DbSet<pedidos> pedidos { get; set; }
        public DbSet<platos> platos { get; set; }

    }
}

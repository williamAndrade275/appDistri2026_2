using app.microCliente.entities.models;
using Microsoft.EntityFrameworkCore;

namespace app.microCliente.dataAccess.context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
    }
}
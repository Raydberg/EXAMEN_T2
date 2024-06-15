using EXAMEN_T2.Models;
using Microsoft.EntityFrameworkCore;

namespace EXAMEN_T2.Datos
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions options):base(options){}
        public DbSet<Usuario> Usuarios { get; set;}
        public DbSet<Producto>Productos { get; set;}
    
    }

}

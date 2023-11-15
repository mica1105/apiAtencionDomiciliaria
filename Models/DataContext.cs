using Microsoft.EntityFrameworkCore;
namespace ApiAtencionDomiciliaria;
public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<Enfermero> Enfermero { get; set; }
        public DbSet<Paciente> Paciente { get; set; }
        public DbSet<HC> HC { get; set; }
        public DbSet<Visita> Visita { get; set; }
    }

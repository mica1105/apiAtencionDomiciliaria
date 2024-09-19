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
        public DbSet<HigieneyConfort> HigieneyConfort { get; set; }
        public DbSet<Curacion> Curacion { get; set; }
        public DbSet<Csv> Csv { get; set; }
        public DbSet<AdmDeFarmacos> AdmDeFarmacos { get; set; }
    }

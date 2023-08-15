using GameZone.Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameZone.Blog.Infra.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Noticia> Noticias { get; set; }
        public DbSet<Comentarios> Comentarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Noticia>()
                .HasOne(c => c.AspNetUsers)
                .WithOne()
                .HasForeignKey<Noticia>(n => n.AspNetUsersId);

            modelBuilder.Entity<Comentarios>()
                .HasOne(c => c.Noticia)
                .WithMany(n => n.Comentarios)
                .HasForeignKey(c => c.NoticiaId);

            modelBuilder.Entity<Comentarios>()
                .HasOne(c => c.AspNetUsers)
                .WithOne()
                .HasForeignKey<Comentarios>(n => n.AspNetUsersId)
                .OnDelete(DeleteBehavior.NoAction);

            // Ignorar a entidade Usuario para que não seja criada a tabela correspondente no banco de dados
            modelBuilder.Ignore<Usuario>();
        }
    }
}

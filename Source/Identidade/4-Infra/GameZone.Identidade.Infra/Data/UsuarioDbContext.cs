using GameZone.Identidade.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetDevPack.Security.Jwt.Core.Model;
using NetDevPack.Security.Jwt.Store.EntityFrameworkCore;

namespace GameZone.Identidade.Infra
{
    public class UsuarioDbContext : IdentityDbContext<Usuario>, ISecurityKeyContext
    {
        public UsuarioDbContext(DbContextOptions<UsuarioDbContext> opts) : base(opts){ }

        public DbSet<KeyMaterial> SecurityKeys { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Criar papéis iniciais
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "Administrador", NormalizedName = "ADMINISTRADOR" },
                new IdentityRole { Id = "2", Name = "Usuário", NormalizedName = "USUÁRIO" }
            );

            // Criar reivindicações iniciais
            modelBuilder.Entity<IdentityRoleClaim<string>>().HasData(
                new IdentityRoleClaim<string> { Id = 1, RoleId = "1", ClaimType = "PodeInserirUsuario", ClaimValue = "true" },
                new IdentityRoleClaim<string> { Id = 2, RoleId = "1", ClaimType = "PodeEditarUsuario", ClaimValue = "true" },
                new IdentityRoleClaim<string> { Id = 3, RoleId = "1", ClaimType = "PodeExcluirUsuario", ClaimValue = "true" },
                new IdentityRoleClaim<string> { Id = 4, RoleId = "1", ClaimType = "PodeInserirNoticia", ClaimValue = "true" },
                new IdentityRoleClaim<string> { Id = 5, RoleId = "1", ClaimType = "PodeEditarNoticia", ClaimValue = "true" },
                new IdentityRoleClaim<string> { Id = 6, RoleId = "1", ClaimType = "PodeExcluirNoticia", ClaimValue = "true" },
                new IdentityRoleClaim<string> { Id = 7, RoleId = "1", ClaimType = "PodeInserirComentario", ClaimValue = "true" },
                new IdentityRoleClaim<string> { Id = 8, RoleId = "1", ClaimType = "PodeEditarComentario", ClaimValue = "true" },
                new IdentityRoleClaim<string> { Id = 9, RoleId = "1", ClaimType = "PodeExcluirComentario", ClaimValue = "true" },

                new IdentityRoleClaim<string> { Id = 10, RoleId = "2", ClaimType = "PodeInserirUsuario", ClaimValue = "false" },
                new IdentityRoleClaim<string> { Id = 11, RoleId = "2", ClaimType = "PodeEditarUsuario", ClaimValue = "false" },
                new IdentityRoleClaim<string> { Id = 12, RoleId = "2", ClaimType = "PodeExcluirUsuario", ClaimValue = "false" },
                new IdentityRoleClaim<string> { Id = 13, RoleId = "2", ClaimType = "PodeInserirNoticia", ClaimValue = "false" },
                new IdentityRoleClaim<string> { Id = 14, RoleId = "2", ClaimType = "PodeEditarNoticia", ClaimValue = "false" },
                new IdentityRoleClaim<string> { Id = 15, RoleId = "2", ClaimType = "PodeExcluirNoticia", ClaimValue = "false" },
                new IdentityRoleClaim<string> { Id = 16, RoleId = "2", ClaimType = "PodeInserirComentario", ClaimValue = "true" },
                new IdentityRoleClaim<string> { Id = 17, RoleId = "2", ClaimType = "PodeEditarComentario", ClaimValue = "true" },
                new IdentityRoleClaim<string> { Id = 18, RoleId = "2", ClaimType = "PodeExcluirComentario", ClaimValue = "true" }
            );
        }
    }
}
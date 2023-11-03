using Bogus;
using GameZone.Core.Utils;
using GameZone.Identidade.API.Authorization;
using GameZone.Identidade.API.Configurations;
using GameZone.Identidade.API.Configurations.Interfaces;
using GameZone.Identidade.Application.Interfaces;
using GameZone.Identidade.Application;
using GameZone.Identidade.Domain.Entities;
using GameZone.Identidade.Infra;
using GameZone.Identidade.Infra.Interfaces;
using GameZone.Identidade.Infra.Repository;
using GameZone.Identidade.Services.Interfaces;
using GameZone.Identidade.Services;
using GameZone.Identidade.Tests.Api.Fixtures;
using GameZone.Identidade.Tests.Api.Infra;
using GameZone.WebAPI.Core.Usuario;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using GameZone.Identidade.API.Extensions;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace GameZone.Identidade.Tests.Api.Services
{
    public class UsuarioServicesTests : IClassFixture<DockerFixture>
    {
        private readonly Faker _faker;
        private readonly DockerFixture _dockerFixture;
        private readonly CreateUsuarioTestsFixture _usuarioTestsFixture;
        private readonly IIdentidadeRepository _identidadeRepository;

        public UsuarioServicesTests(DockerFixture dockerFixture)
        {
            _dockerFixture = dockerFixture;

            while (!_dockerFixture.VerificarContainerAtivo()) ;

            if (!ValidaDataBase.CheckIfDatabaseExists(_dockerFixture.GetConnectionString().Replace("GameZoneDB", "Master"), "GameZoneDB"))
            {
                using (var connection = new SqlConnection(_dockerFixture.GetConnectionString().Replace("GameZoneDB", "Master")))
                {
                    connection.Open();

                    string createDataBaseQuery = @"CREATE DATABASE [GameZoneDB]";
                    using (var command = new SqlCommand(createDataBaseQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            using (var connection = new SqlConnection(_dockerFixture.GetConnectionString()))
            {
                connection.Open();

                string createTablesQuery = @"
                    IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetRoleClaims]') AND type in (N'U'))
                    BEGIN
                    CREATE TABLE [dbo].[AspNetRoleClaims](
	                    [Id] [int] IDENTITY(1,1) NOT NULL,
	                    [RoleId] [nvarchar](450) NOT NULL,
	                    [ClaimType] [nvarchar](max) NULL,
	                    [ClaimValue] [nvarchar](max) NULL,
                     CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
                    (
	                    [Id] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
                    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
                    END

                    IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetRoles]') AND type in (N'U'))
                    BEGIN
                    CREATE TABLE [dbo].[AspNetRoles](
	                    [Id] [nvarchar](450) NOT NULL,
	                    [Name] [nvarchar](256) NULL,
	                    [NormalizedName] [nvarchar](256) NULL,
	                    [ConcurrencyStamp] [nvarchar](max) NULL,
                     CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
                    (
	                    [Id] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
                    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
                    END

                    IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUserClaims]') AND type in (N'U'))
                    BEGIN
                    CREATE TABLE [dbo].[AspNetUserClaims](
	                    [Id] [int] IDENTITY(1,1) NOT NULL,
	                    [UserId] [nvarchar](450) NOT NULL,
	                    [ClaimType] [nvarchar](max) NULL,
	                    [ClaimValue] [nvarchar](max) NULL,
                     CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
                    (
	                    [Id] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
                    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
                    END

                    IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUserLogins]') AND type in (N'U'))
                    BEGIN
                    CREATE TABLE [dbo].[AspNetUserLogins](
	                    [LoginProvider] [nvarchar](450) NOT NULL,
	                    [ProviderKey] [nvarchar](450) NOT NULL,
	                    [ProviderDisplayName] [nvarchar](max) NULL,
	                    [UserId] [nvarchar](450) NOT NULL,
                     CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
                    (
	                    [LoginProvider] ASC,
	                    [ProviderKey] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
                    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
                    END

                    IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUserRoles]') AND type in (N'U'))
                    BEGIN
                    CREATE TABLE [dbo].[AspNetUserRoles](
	                    [UserId] [nvarchar](450) NOT NULL,
	                    [RoleId] [nvarchar](450) NOT NULL,
                     CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
                    (
	                    [UserId] ASC,
	                    [RoleId] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
                    ) ON [PRIMARY]
                    END

                    IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUsers]') AND type in (N'U'))
                    BEGIN
                    CREATE TABLE [dbo].[AspNetUsers](
	                    [Id] [nvarchar](450) NOT NULL,
	                    [Name] [nvarchar](256) NOT NULL,
	                    [CpfCnpj] [nvarchar](15) NOT NULL,
	                    [DataNascimento] [datetime2](7) NOT NULL,
	                    [IsAdministrator] [bit] NOT NULL,
	                    [IdUsuarioInclusao] [nvarchar](450) NULL,
	                    [UserName] [nvarchar](256) NULL,
	                    [NormalizedUserName] [nvarchar](256) NULL,
	                    [Email] [nvarchar](256) NULL,
	                    [NormalizedEmail] [nvarchar](256) NULL,
	                    [EmailConfirmed] [bit] NOT NULL,
	                    [PasswordHash] [nvarchar](max) NULL,
	                    [SecurityStamp] [nvarchar](max) NULL,
	                    [ConcurrencyStamp] [nvarchar](max) NULL,
	                    [PhoneNumber] [nvarchar](max) NULL,
	                    [PhoneNumberConfirmed] [bit] NOT NULL,
	                    [TwoFactorEnabled] [bit] NOT NULL,
	                    [LockoutEnd] [datetimeoffset](7) NULL,
	                    [LockoutEnabled] [bit] NOT NULL,
	                    [AccessFailedCount] [int] NOT NULL,
                     CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
                    (
	                    [Id] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
                    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
                    END

                    IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUserTokens]') AND type in (N'U'))
                    BEGIN
                    CREATE TABLE [dbo].[AspNetUserTokens](
	                    [UserId] [nvarchar](450) NOT NULL,
	                    [LoginProvider] [nvarchar](450) NOT NULL,
	                    [Name] [nvarchar](450) NOT NULL,
	                    [Value] [nvarchar](max) NULL,
                     CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
                    (
	                    [UserId] ASC,
	                    [LoginProvider] ASC,
	                    [Name] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
                    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
                    END

                    IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RefreshTokens]') AND type in (N'U'))
                    BEGIN
                    CREATE TABLE [dbo].[RefreshTokens](
	                    [Id] [uniqueidentifier] NOT NULL,
	                    [Username] [nvarchar](max) NOT NULL,
	                    [Token] [uniqueidentifier] NOT NULL,
	                    [ExpirationDate] [datetime2](7) NOT NULL,
                     CONSTRAINT [PK_RefreshTokens] PRIMARY KEY CLUSTERED 
                    (
	                    [Id] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
                    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
                    END

                    IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SecurityKeys]') AND type in (N'U'))
                    BEGIN
                    CREATE TABLE [dbo].[SecurityKeys](
	                    [Id] [uniqueidentifier] NOT NULL,
	                    [KeyId] [nvarchar](max) NULL,
	                    [Type] [nvarchar](max) NULL,
	                    [Parameters] [nvarchar](max) NULL,
	                    [IsRevoked] [bit] NOT NULL,
	                    [RevokedReason] [nvarchar](max) NULL,
	                    [CreationDate] [datetime2](7) NOT NULL,
	                    [ExpiredAt] [datetime2](7) NULL,
                     CONSTRAINT [PK_SecurityKeys] PRIMARY KEY CLUSTERED 
                    (
	                    [Id] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
                    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
                    END

                    IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[AspNetRoleClaims]') AND name = N'IX_AspNetRoleClaims_RoleId')
                    BEGIN
                        CREATE NONCLUSTERED INDEX [IX_AspNetRoleClaims_RoleId] ON [dbo].[AspNetRoleClaims]
                        (
	                        [RoleId] ASC
                        )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
                    END

                    IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[AspNetRoles]') AND name = N'RoleNameIndex')
                    BEGIN
                        CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex] ON [dbo].[AspNetRoles]
                        (
	                        [NormalizedName] ASC
                        )
                        WHERE ([NormalizedName] IS NOT NULL)
                        WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
                    END

                    IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUserClaims]') AND name = N'IX_AspNetUserClaims_UserId')
                    BEGIN
                        CREATE NONCLUSTERED INDEX [IX_AspNetUserClaims_UserId] ON [dbo].[AspNetUserClaims]
                        (
	                        [UserId] ASC
                        )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
                    END

                    IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUserLogins]') AND name = N'IX_AspNetUserLogins_UserId')
                    BEGIN
                        CREATE NONCLUSTERED INDEX [IX_AspNetUserLogins_UserId] ON [dbo].[AspNetUserLogins]
                        (
	                        [UserId] ASC
                        )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
                    END

                    IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUserRoles]') AND name = N'IX_AspNetUserRoles_RoleId')
                    BEGIN
                        CREATE NONCLUSTERED INDEX [IX_AspNetUserRoles_RoleId] ON [dbo].[AspNetUserRoles]
                        (
	                        [RoleId] ASC
                        )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
                    END

                    IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUsers]') AND name = N'EmailIndex')
                    BEGIN
                        CREATE NONCLUSTERED INDEX [EmailIndex] ON [dbo].[AspNetUsers]
                        (
	                        [NormalizedEmail] ASC
                        )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
                    END

                    IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUsers]') AND name = N'UserNameIndex')
                    BEGIN
                        CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex] ON [dbo].[AspNetUsers]
                        (
	                        [NormalizedUserName] ASC
                        )
                        WHERE ([NormalizedUserName] IS NOT NULL)
                        WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
                    END

                    IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AspNetRoleClaims_AspNetRoles_RoleId]') AND parent_object_id = OBJECT_ID(N'[dbo].[AspNetRoleClaims]'))
                    BEGIN
                        ALTER TABLE [dbo].[AspNetRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
                        REFERENCES [dbo].[AspNetRoles] ([Id])
                        ON DELETE CASCADE
                    END

                    IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AspNetRoleClaims_AspNetRoles_RoleId]') AND parent_object_id = OBJECT_ID(N'[dbo].[AspNetRoleClaims]'))
                    BEGIN
                        ALTER TABLE [dbo].[AspNetRoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
                    END

                    IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AspNetUserClaims_AspNetUsers_UserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[AspNetUserClaims]'))
                    BEGIN    
                        ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId])
                        REFERENCES [dbo].[AspNetUsers] ([Id])
                        ON DELETE CASCADE
                    END

                    IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AspNetUserClaims_AspNetUsers_UserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[AspNetUserClaims]'))
                    BEGIN
                        ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
                    END

                    IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AspNetUserLogins_AspNetUsers_UserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[AspNetUserLogins]'))
                    BEGIN
                        ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId])
                        REFERENCES [dbo].[AspNetUsers] ([Id])
                        ON DELETE CASCADE
                    END

                    IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AspNetUserLogins_AspNetUsers_UserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[AspNetUserLogins]'))
                    BEGIN
                        ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
                    END

                    IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AspNetUserRoles_AspNetRoles_RoleId]') AND parent_object_id = OBJECT_ID(N'[dbo].[AspNetUserRoles]'))
                    BEGIN
                        ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
                        REFERENCES [dbo].[AspNetRoles] ([Id])
                        ON DELETE CASCADE
                    END

                    IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AspNetUserRoles_AspNetRoles_RoleId]') AND parent_object_id = OBJECT_ID(N'[dbo].[AspNetUserRoles]'))
                    BEGIN
                        ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
                    END

                    IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AspNetUserRoles_AspNetUsers_UserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[AspNetUserRoles]'))
                    BEGIN
                        ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId])
                        REFERENCES [dbo].[AspNetUsers] ([Id])
                    ON DELETE CASCADE
                    END

                    IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AspNetUserRoles_AspNetUsers_UserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[AspNetUserRoles]'))
                    BEGIN
                        ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
                    END
                    IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AspNetUserTokens_AspNetUsers_UserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[AspNetUserTokens]'))
                    BEGIN
                        ALTER TABLE [dbo].[AspNetUserTokens]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId])
                        REFERENCES [dbo].[AspNetUsers] ([Id])
                        ON DELETE CASCADE
                    END
                    IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AspNetUserTokens_AspNetUsers_UserId]') AND parent_object_id = OBJECT_ID(N'[dbo].[AspNetUserTokens]'))
                    BEGIN
                        ALTER TABLE [dbo].[AspNetUserTokens] CHECK CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
                    END
                ";
                using (var command = new SqlCommand(createTablesQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }

            _faker = new Faker();
            _usuarioTestsFixture = new CreateUsuarioTestsFixture();

            string connectionString = _dockerFixture.GetConnectionString();

            var options = new DbContextOptionsBuilder<UsuarioDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            var services = new ServiceCollection();

            services.AddDbContext<UsuarioDbContext>(options => options.UseSqlServer(connectionString, b => b.MigrationsAssembly("GameZone.Identidade.Infra")));
            services.AddDbContext<UsuarioDbContext>(options => options.UseInMemoryDatabase("TestDatabase"));

            services.AddIdentity<Usuario, IdentityRole>()
                            .AddErrorDescriber<IdentityMensagensPortugues>()
                            .AddEntityFrameworkStores<UsuarioDbContext>()
                            .AddDefaultTokenProviders();

            services.AddLogging();

            services.AddScoped<IIdentidadeRepository, IdentidadeRepository>();

            var serviceProvider = services.BuildServiceProvider();

            var usuarioDbContext = serviceProvider.GetRequiredService<UsuarioDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<Usuario>>();

            _identidadeRepository = serviceProvider.GetRequiredService<IIdentidadeRepository>();
        }


        [Fact]
        public async void Should_Insert_Usuario_With_Success()
        {
            // Arrange
            var usuarioDTO = _usuarioTestsFixture.CreateUserPF();
            Usuario usuario = new Usuario()
            {
                CpfCnpj = usuarioDTO.CpfCnpj,
                Email = usuarioDTO.Email,
                DataNascimento = usuarioDTO.DataNascimento,
                EmailConfirmed = true,
                IsAdministrator = _faker.Random.Bool(),
                IdUsuarioInclusao = usuarioDTO.IdUsuarioInclusao,
                Name = usuarioDTO.Name,
                UserName = usuarioDTO.Email
            };
            var password = "123@Mudar";

            var expectedResult = IdentityResult.Success;

            // Act
            var result = await _identidadeRepository.CadastrarUsuario(usuario, password);

            // Assert
            Assert.Equal(expectedResult, result);
        }
    }
}

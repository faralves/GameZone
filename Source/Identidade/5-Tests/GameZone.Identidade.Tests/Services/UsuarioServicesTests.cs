using Bogus;
using GameZone.Core.Utils;
using GameZone.Identidade.API.Configurations;
using GameZone.Identidade.API.Configurations.Interfaces;
using GameZone.Identidade.API.Controllers;
using GameZone.Identidade.Domain.Entities;
using GameZone.Identidade.Infra;
using GameZone.Identidade.Infra.Interfaces;
using GameZone.Identidade.Infra.Repository;
using GameZone.Identidade.Tests.Api.Fixtures;
using GameZone.Identidade.Tests.Api.Infra;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameZone.Identidade.Tests.Api.Services
{
    public class UsuarioServicesTests : IClassFixture<DockerFixture>
    {
        private readonly Faker _faker;
        private readonly DockerFixture _dockerFixture;
        private readonly CreateUsuarioTestsFixture _usuarioTestsFixture;
        private readonly Mock<IIdentidadeRepository> _identidadeRepositoryMock;
        private Mock<ILogger<IdentidadeRepository>> _loggerMock;
        private UserManager<Usuario> _userManager;
        private SignInManager<Usuario> _signInManager;
        private RoleManager<IdentityRole> _roleManager;
        private readonly IServiceProvider _serviceProvider;


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


            _loggerMock = new Mock<ILogger<IdentidadeRepository>>();
            _identidadeRepositoryMock = new Mock<IIdentidadeRepository>();
            _faker = new Faker();
            _usuarioTestsFixture = new CreateUsuarioTestsFixture();



            //new StartupTests(_dockerFixture).ConfigureServices(services);

            // Configurar o DbContext em memória
            //var serviceProvider = new ServiceCollection()
            //    .AddDbContext<UsuarioDbContext>(options => options.UseInMemoryDatabase("InMemoryDb"))
            //    .AddIdentity<Usuario, IdentityRole>()
            //    .AddEntityFrameworkStores<UsuarioDbContext>()
            //    .AddDefaultTokenProviders()
            //    .Services
            //    .BuildServiceProvider();

            //var context = serviceProvider.GetRequiredService<UsuarioDbContext>();

            //var services = new ServiceCollection();
            //services.AddIdentity<Usuario, IdentityRole>()
            //    .AddEntityFrameworkStores<UsuarioDbContext>()
            //    .AddDefaultTokenProviders();

            // Adicione outros serviços necessários, como IHttpContextAccessor e ILogger

            var services = new ServiceCollection();
            services.AddIdentity<Usuario, IdentityRole>()
                      .AddEntityFrameworkStores<UsuarioDbContext>()
                      .AddDefaultTokenProviders();

            var options = new DbContextOptionsBuilder<UsuarioDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            var dbContext = new UsuarioDbContext(options);

            services.AddSingleton<UsuarioDbContext>(dbContext);
            var logger = new LoggerFactory().CreateLogger<UserManager<Usuario>>();
            services.AddSingleton<ILogger<UserManager<Usuario>>>(logger);

            var loggerRole = new LoggerFactory().CreateLogger<RoleManager<IdentityRole>>();
            services.AddSingleton<ILogger<RoleManager<IdentityRole>>>(loggerRole);

            var loggerSignIn = new LoggerFactory().CreateLogger<SignInManager<Usuario>>();
            services.AddSingleton<ILogger<SignInManager<Usuario>>>(loggerSignIn);

            var loggerProtector = new LoggerFactory().CreateLogger<DataProtectorTokenProvider<Usuario>>();
            services.AddSingleton<ILogger<DataProtectorTokenProvider<Usuario>>>(loggerProtector);

            _serviceProvider = services.BuildServiceProvider();

            _userManager = _serviceProvider.GetRequiredService<UserManager<Usuario>>();
            _signInManager = _serviceProvider.GetRequiredService<SignInManager<Usuario>>();
        }

        //protected UsuarioDbContext CreateContext()
        //{
        //    var serviceProvider = new ServiceCollection()
        //        .AddEntityFrameworkInMemoryDatabase()
        //        .BuildServiceProvider();

        //    var options = new DbContextOptionsBuilder<UsuarioDbContext>()
        //        .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
        //        .UseInternalServiceProvider(serviceProvider)
        //        .Options;

        //    return new UsuarioDbContext(options);
        //}

        //[Fact]
        //public async void Should_Insert_Usuario_With_Success()
        //{
        //    var _connectionString = _dockerFixture.GetConnectionString();

        //    var configValues = new Dictionary<string, string>
        //    {
        //        { "ConnectionStrings:Connection", _connectionString }
        //    };

        //    var configuration = new ConfigurationBuilder()
        //        .AddInMemoryCollection(configValues)
        //        .Build();

        //    // Arrange
        //    var usuarioDTO = _usuarioTestsFixture.CreateUserPF();
        //    Usuario usuario = new Usuario()
        //    {
        //        CpfCnpj = usuarioDTO.CpfCnpj,
        //        Email = usuarioDTO.Email,
        //        DataNascimento = usuarioDTO.DataNascimento,
        //        EmailConfirmed = true,
        //        IsAdministrator = _faker.Random.Bool(),
        //        IdUsuarioInclusao = usuarioDTO.IdUsuarioInclusao,
        //        Name = usuarioDTO.Name,
        //        UserName = usuarioDTO.Email                                
        //    };
        //    var password = usuarioDTO.Password;

        //    var expectedResult = IdentityResult.Success;

        //    // Usar o _serviceProvider para obter as dependências necessárias, incluindo o UserManager
        //    using (var context = CreateContext())
        //    {

        //        //// Configurar e inicializar o UserManager com a configuração correta
        //        var dbContextOptions = new DbContextOptionsBuilder<UsuarioDbContext>()
        //            .UseSqlServer(_dockerFixture.GetConnectionString())
        //            .Options;

        //        var userStore = new UserStore<Usuario>(new UsuarioDbContext(dbContextOptions));
        //        _userManager = new UserManager<Usuario>(userStore, null, null, null, null, null, null, null, null);

        //        _identidadeRepositoryMock.Setup(r => r.CadastrarUsuario(usuario, password)).ReturnsAsync(expectedResult);

        //        var usuarioServices = new IdentidadeRepository(_userManager, _signInManager, _loggerMock.Object, _roleManager);

        //        // Act
        //        var result = await usuarioServices.CadastrarUsuario(usuario, password);

        //        // Assert
        //        Assert.Equal(expectedResult, result);
        //    }
        //}

        [Fact]
        public async Task CadastrarUsuario_ShouldSucceed_WhenValidUser()
        {
            // Arrange
            //var userManager = new Mock<UserManager<Usuario>>(
            //    new Mock<IUserStore<Usuario>>().Object,
            //    null, null, null, null, null, null, null, null);

            //userManager.Setup(u => u.CreateAsync(It.IsAny<Usuario>(), It.IsAny<string>()))
            //    .ReturnsAsync(IdentityResult.Success);

            //var signInManager = new Mock<SignInManager<Usuario>>(
            //    userManager.Object,
            //    new Mock<IHttpContextAccessor>().Object,
            //    new Mock<IUserClaimsPrincipalFactory<Usuario>>().Object,
            //    null, null, null);

            //signInManager
            //    .Setup(s => s.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
            //    .ReturnsAsync(SignInResult.Success);

            var userManager = new Mock<UserManager<Usuario>>(
                new Mock<IUserStore<Usuario>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<Usuario>>().Object,
                new IUserValidator<Usuario>[0],
                new IPasswordValidator<Usuario>[0],
                new Mock<ILookupNormalizer>().Object,
                new IdentityErrorDescriber(),
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<Usuario>>>().Object
            );

            var contextAccessor = new Mock<IHttpContextAccessor>();
            var claimsFactory = new Mock<IUserClaimsPrincipalFactory<Usuario>>();
            var schemes = new Mock<IAuthenticationSchemeProvider>();

            var signInManager = new SignInManager<Usuario>(userManager.Object, contextAccessor.Object, claimsFactory.Object, new Mock<IOptions<IdentityOptions>>().Object, new Mock<ILogger<SignInManager<Usuario>>>().Object, schemes.Object, new Mock<IUserConfirmation<Usuario>>().Object);

            var roleManager = new Mock<RoleManager<IdentityRole>>(
                new Mock<IRoleStore<IdentityRole>>().Object,
                null, null, null, null);

            var loggerMock = new Mock<ILogger<IdentidadeRepository>>();

            var identidadeRepository = new IdentidadeRepository(
                userManager.Object,
                signInManager,
                loggerMock.Object,
                roleManager.Object
            );

            var usuario = new Usuario
            {
                UserName = "testuser@example.com",
                Email = "testuser@example.com",
                IsAdministrator = false 
            };

            var password = "TestPassword123"; // Defina a senha conforme necessário

            // Act
            var result = await identidadeRepository.CadastrarUsuario(usuario, password);

            // Assert
            Assert.True(result.Succeeded);
        }
    }
}

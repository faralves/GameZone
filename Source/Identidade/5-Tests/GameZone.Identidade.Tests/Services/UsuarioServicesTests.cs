using Bogus;
using GameZone.Core.Utils;
using GameZone.Identidade.Domain.Entities;
using GameZone.Identidade.Infra;
using GameZone.Identidade.Infra.Interfaces;
using GameZone.Identidade.Infra.Repository;
using GameZone.Identidade.Tests.Api.Fixtures;
using GameZone.Identidade.Tests.Api.Infra;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using GameZone.Identidade.API.Extensions;

namespace GameZone.Identidade.Tests.Api.Services
{
    public class UsuarioServicesTests : IClassFixture<DockerFixture>
    {
        private readonly Faker _faker;
        private readonly Faker<Usuario> _fakerUsuario;
        private readonly DockerFixture _dockerFixture;
        private readonly CreateUsuarioTestsFixture _usuarioTestsFixture;
        private readonly IIdentidadeRepository _identidadeRepository;

        public UsuarioServicesTests(DockerFixture dockerFixture)
        {
            _dockerFixture = dockerFixture;

            //while (!_dockerFixture.VerificarContainerAtivo()) ;

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

            using (var connection = new SqlConnection(_dockerFixture.GetConnectionString()))
            {
                connection.Open();

                //string InsertRoles = @"INSERT INTO AspNetRoles (Id, [Name], NormalizedName)
                //                                VALUES ('1', 'Administrador', 'ADMINISTRADOR'),
                //                                       ('2', 'Usuário', 'USUÁRIO');
                //                                ";

                string InsertRoles = @"
                                    IF NOT EXISTS (SELECT 1 FROM AspNetRoles WHERE Id = '1' AND [Name] = 'Administrador' AND NormalizedName = 'ADMINISTRADOR')    
                                    INSERT INTO AspNetRoles (Id, [Name], NormalizedName) VALUES ('1', 'Administrador', 'ADMINISTRADOR')

                                    IF NOT EXISTS (SELECT 1 FROM AspNetRoles WHERE Id = '2' AND [Name] = 'Usuário' AND NormalizedName = 'USUÁRIO')
                                    INSERT INTO AspNetRoles (Id, [Name], NormalizedName) VALUES ('2', 'Usuário', 'USUÁRIO')
                                ";

                using (var command = new SqlCommand(InsertRoles, connection))
                {
                    command.ExecuteNonQuery();
                }

                //string InsertClaims = @"
                //                        INSERT INTO AspNetRoleClaims (RoleId, ClaimType, ClaimValue)
                //                        VALUES ('1', 'PodeInserirUsuario', 'true'),
                //                               ('1', 'PodeEditarUsuario', 'true'),
                //                               ('1', 'PodeExcluirUsuario', 'true'),
                //                               ('1', 'PodeInserirNoticia', 'true'),
                //                               ('1', 'PodeEditarNoticia', 'true'),
                //                               ('1', 'PodeExcluirNoticia', 'true'),
                //                               ('1', 'PodeInserirComentario', 'true'),
                //                               ('1', 'PodeEditarComentario', 'true'),
                //                               ('1', 'PodeExcluirComentario', 'true'),
                //                               ( '2', 'PodeInserirUsuario', 'false'),
                //                               ( '2', 'PodeEditarUsuario', 'false'),
                //                               ( '2', 'PodeExcluirUsuario', 'false'),
                //                               ( '2', 'PodeInserirNoticia', 'false'),
                //                               ( '2', 'PodeEditarNoticia', 'false'),
                //                               ( '2', 'PodeExcluirNoticia', 'false'),
                //                               ( '2', 'PodeInserirComentario', 'true'),
                //                               ( '2', 'PodeEditarComentario', 'true'),
                //                               ( '2', 'PodeExcluirComentario', 'true');
                //                                ";


                string InsertClaims = @"
                                IF NOT EXISTS (SELECT 1 FROM AspNetRoleClaims WHERE RoleId = '1' AND ClaimType = 'PodeInserirUsuario' AND ClaimValue = 'true')
                                    INSERT INTO AspNetRoleClaims (RoleId, ClaimType, ClaimValue) SELECT '1', 'PodeInserirUsuario', 'true'

                                IF NOT EXISTS (SELECT 1 FROM AspNetRoleClaims WHERE RoleId = '1' AND ClaimType = 'PodeEditarUsuario' AND ClaimValue = 'true')
                                    INSERT INTO AspNetRoleClaims (RoleId, ClaimType, ClaimValue) SELECT '1', 'PodeEditarUsuario', 'true'

                                IF NOT EXISTS (SELECT 1 FROM AspNetRoleClaims WHERE RoleId = '1' AND ClaimType = 'PodeExcluirUsuario' AND ClaimValue = 'true')
                                    INSERT INTO AspNetRoleClaims (RoleId, ClaimType, ClaimValue) SELECT '1', 'PodeExcluirUsuario', 'true'

    
                                IF NOT EXISTS (SELECT 1 FROM AspNetRoleClaims WHERE RoleId = '2' AND ClaimType = 'PodeInserirUsuario' AND ClaimValue = 'false')
                                    INSERT INTO AspNetRoleClaims (RoleId, ClaimType, ClaimValue) SELECT '2', 'PodeInserirUsuario', 'false'

                                IF NOT EXISTS (SELECT 1 FROM AspNetRoleClaims WHERE RoleId = '2' AND ClaimType = 'PodeEditarUsuario' AND ClaimValue = 'false')
                                    INSERT INTO AspNetRoleClaims (RoleId, ClaimType, ClaimValue) SELECT '2', 'PodeEditarUsuario', 'false'

                                IF NOT EXISTS (SELECT 1 FROM AspNetRoleClaims WHERE RoleId = '2' AND ClaimType = 'PodeExcluirUsuario' AND ClaimValue = 'false')
                                    INSERT INTO AspNetRoleClaims (RoleId, ClaimType, ClaimValue) SELECT '2', 'PodeExcluirUsuario', 'false'
                            ";

                using (var command = new SqlCommand(InsertClaims, connection))
                {
                    command.ExecuteNonQuery();
                }

                string InsertUserTest = @"
                                            INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount, Name, CpfCnpj, DataNascimento, IsAdministrator, IdUsuarioInclusao)
                                            SELECT 
                                                'ead52cf9-2155-4d47-b2cd-a1968201c19c', 'email@email.teste.com.br', 'EMAIL@EMAIL.TESTE.COM.BR', 'email@email.teste.com.br', 'EMAIL@EMAIL.TESTE.COM.BR', 1, 'AQAAAAEAACcQAAAAEDQdDUPatf69cATUNuYRJ4I3fgQJtlqwhcAHuvDnXNsMtAgSQt8jJmAsdvHRhlhqQw==', 'TCCKNYEVCJ2G6F37B7FMRUCEETTXCGJG', 'aec4a511-964f-43a7-b3aa-0a1df38880a4', NULL, 0, 0, NULL, 1, 0, 'Fulano de Teste', '326.769.068-44', '1988-01-29 14:07:58.2366363', 0, '81e12e21-2052-74eb-126a-c54e375c6be3'
                                            WHERE NOT EXISTS (SELECT 1 FROM AspNetUsers WHERE Id = 'ead52cf9-2155-4d47-b2cd-a1968201c19c');
                                        ";

                using (var command = new SqlCommand(InsertUserTest, connection))
                {
                    command.ExecuteNonQuery();
                }

                string InsertAspNetUserRoles = @"
                                                    INSERT INTO AspNetUserRoles (UserId, RoleId)
                                                    SELECT 'ead52cf9-2155-4d47-b2cd-a1968201c19c', 2
                                                    WHERE NOT EXISTS (
                                                        SELECT 1
                                                        FROM AspNetUserRoles
                                                        WHERE UserId = 'ead52cf9-2155-4d47-b2cd-a1968201c19c'
                                                        AND RoleId = 2
                                                    );
                                                ";

                using (var command = new SqlCommand(InsertAspNetUserRoles, connection))
                {
                    command.ExecuteNonQuery();
                }
            }


            _faker = new Faker();
            _fakerUsuario = new Faker<Usuario>();
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


        [Fact(DisplayName = "Validando se Cadastro de Usuário")]
        [Trait("Categoria", "Validando Teste Integrado")]
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

        [Fact(DisplayName = "Validando se Autenticação de Login")]
        [Trait("Categoria", "Validando Teste Integrado")]
        public async void Should_Auth_Usuario_With_Success()
        {
            // Arrange
            var usuarioDTO = _usuarioTestsFixture.CreateUserPF();
            LoginUsuario usuario = new LoginUsuario()
            {
                Email = "email@email.teste.com.br",
                Password = "123@Mudar"
            };

            var expectedResult =  _fakerUsuario.Generate();
            expectedResult = new Usuario()
            {
                Id = "ead52cf9-2155-4d47-b2cd-a1968201c19c",
                UserName = "email@email.teste.com.br",
                NormalizedUserName = "EMAIL@EMAIL.TESTE.COM.BR",
                Email = "email@email.teste.com.br",
                NormalizedEmail = "EMAIL@EMAIL.TESTE.COM.BR",
                EmailConfirmed = true,
                PasswordHash = "AQAAAAEAACcQAAAAEDQdDUPatf69cATUNuYRJ4I3fgQJtlqwhcAHuvDnXNsMtAgSQt8jJmAsdvHRhlhqQw==",
                SecurityStamp = "TCCKNYEVCJ2G6F37B7FMRUCEETTXCGJG",
                ConcurrencyStamp = "aec4a511-964f-43a7-b3aa-0a1df38880a4",
                PhoneNumber = null,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnd = null,
                LockoutEnabled = true,
                AccessFailedCount = 0,
                Name = "Fulano de Teste",
                CpfCnpj = "326.769.068-44",
                DataNascimento = new DateTime(1988, 01, 29, 14, 7, 58, 236),
                IsAdministrator = false,
                IdUsuarioInclusao = "81e12e21-2052-74eb-126a-c54e375c6be3"
            };

            // Act
            var result = await _identidadeRepository.ObterUsuarioPorEmail(usuario.Email);

            // Assert
            Assert.Equal(expectedResult.Id, result.Id);
            Assert.Equal(expectedResult.UserName, result.UserName);
            Assert.Equal(expectedResult.NormalizedUserName, result.NormalizedUserName);
            Assert.Equal(expectedResult.Email, result.Email);
            Assert.Equal(expectedResult.NormalizedEmail, result.NormalizedEmail);
            Assert.Equal(expectedResult.EmailConfirmed, result.EmailConfirmed);
            Assert.Equal(expectedResult.PasswordHash, result.PasswordHash);
            Assert.Equal(expectedResult.SecurityStamp, result.SecurityStamp);
            Assert.Equal(expectedResult.ConcurrencyStamp, result.ConcurrencyStamp);
            Assert.Equal(expectedResult.PhoneNumber, result.PhoneNumber);
            Assert.Equal(expectedResult.PhoneNumberConfirmed, result.PhoneNumberConfirmed);
            Assert.Equal(expectedResult.TwoFactorEnabled, result.TwoFactorEnabled);
            Assert.Equal(expectedResult.LockoutEnd, result.LockoutEnd);
            Assert.Equal(expectedResult.LockoutEnabled, result.LockoutEnabled);
            Assert.Equal(expectedResult.AccessFailedCount, result.AccessFailedCount);
            Assert.Equal(expectedResult.Name, result.Name);
            Assert.Equal(expectedResult.CpfCnpj, result.CpfCnpj);
            Assert.Equal(expectedResult.DataNascimento.Date, result.DataNascimento.Date);
            Assert.Equal(expectedResult.IsAdministrator, result.IsAdministrator);
            Assert.Equal(expectedResult.IdUsuarioInclusao, result.IdUsuarioInclusao);
        }
    }
}

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace GameZone.Identidade.API.Configurations
{
    public static class SwaggerConfig
    {
        public static void ConfigureSwagger(WebApplicationBuilder builder)
        {
            // Configurar o Swagger
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API de Usuários", Version = "v1" });

                // Adicionar o esquema de segurança JWT
                //c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Description = "Cabeçalho de autorização JWT usando o esquema Bearer",
                    Type = SecuritySchemeType.Http,
                    //Scheme = "bearer",
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    BearerFormat = "JWT"
                });

                // Adicionar o requisito de segurança para todas as operações do Swagger
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            }
                        },
                        new string[] {}
                    }
                });
            });
        }
    }
}
using GameZone.Identidade.API.Configurations;
using GameZone.Identidade.API.Configurations.Interfaces;
using GameZone.Identidade.Infra;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

ConfigureServices.Configure(builder);

var app = builder.Build();

// Create database
using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
var db = scope.ServiceProvider.GetRequiredService<UsuarioDbContext>();
await db.Database.MigrateAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseJwksDiscovery();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

var localizationOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(localizationOptions.Value);


using (var scop = app.Services.CreateScope())
{
    var services = scop.ServiceProvider;

    var seed = services.GetRequiredService<ISeed>();
    await seed.UsuarioAdm();
}

app.Run();


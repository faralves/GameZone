using GameZone.Identidade.API.Configurations;
using GameZone.Identidade.API.Configurations.Interfaces;
using GameZone.Identidade.Infra;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

ConfigureServices.Configure(builder);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Create database
    using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<UsuarioDbContext>();
    await db.Database.EnsureCreatedAsync();
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


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var seed = services.GetRequiredService<ISeed>();
    await seed.UsuarioAdm();
}

app.Run();


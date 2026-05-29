using Microsoft.EntityFrameworkCore;
using StellarGear.Application.Interfaces;
using StellarGear.Infrastructure.Persistence;
using StellarGear.Infrastructure.Persistence.Repositories;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<StellarGearContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));

builder.Services.AddScoped<IPassageiroRepository, PassageiroRepository>();
builder.Services.AddScoped<IMedicoRepository, MedicoRepository>();
builder.Services.AddScoped<ITrajeRepository, TrajeRepository>();
builder.Services.AddScoped<ILeituraSensorRepository, LeituraSensorRepository>();
builder.Services.AddScoped<IAlertaEmergenciaRepository, AlertaEmergenciaRepository>(); 
builder.Services.AddScoped<IHistoricoMedicoRepository, HistoricoMedicoRepository>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "🚀 StellarGear API 🛰️", 
        Version = "v1",
        Description = "Interface de controle para a Global Solution 2026/1."
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
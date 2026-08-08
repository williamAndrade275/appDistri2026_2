using app.microCliente.common.EventMQ;
using app.microCliente.dataAccess.context;
using app.microCliente.dataAccess.repositories;
using app.microCliente.services.EventMQ;
using app.microCliente.services.Implementations;
using app.microCliente.services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Cadena de conexión SQL Server
var conSqlServer = builder.Configuration.GetConnectionString("BDDSqlServer")!;

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(conSqlServer);
    options.LogTo(Console.WriteLine, LogLevel.Information)
           .EnableSensitiveDataLogging();
});

// RabbitMQ
builder.Services.Configure<RabbitMQSettings>(
    builder.Configuration.GetSection("rabbitmq")
);

// Servicios y repositorios
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IClienteService, ClienteService>();

builder.Services.AddScoped<IDireccionClienteRepository, DireccionClienteRepository>();
builder.Services.AddScoped<IDireccionClienteService, DireccionClienteService>();

builder.Services.AddSingleton<IRabbitMQService, RabbitMQService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsEnvironment("Docker"))
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
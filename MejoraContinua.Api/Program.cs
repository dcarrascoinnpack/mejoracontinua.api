using MejoraContinua.Api.Data;
using MejoraContinua.Api.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<DbConnectionFactory>();
builder.Services.AddScoped<NoConformidadRepository>();
builder.Services.AddScoped<AnalisisCausaRaizRepository>();
builder.Services.AddScoped<AccionCorrectivaRepository>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

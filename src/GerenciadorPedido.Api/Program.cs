using GerenciadorPedido.Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Adicionando o middleware para gerar a documentação OpenAPI (Swagger)
    app.MapOpenApi();

    // Adicionando o Swagger UI para visualização da documentação
    app.UseSwaggerUI(options =>
    {
        // Configurando o endpoint do Swagger UI para acessar a documentação da API
        options.SwaggerEndpoint("/openapi/v1.json", "Gerenciador de Pedidos API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

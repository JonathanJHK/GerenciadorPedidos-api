using GerenciadorPedido.Api.Data;
using GerenciadorPedido.Api.Handlers;
using GerenciadorPedido.Api.Interfaces;
using GerenciadorPedido.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Adicionando o contexto do banco de dados ao contêiner de serviços
builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Configurando a conexão com o banco de dados PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

// Registrando o serviço de produtos no contêiner de serviços
builder.Services.AddScoped<IProdutoService, ProdutoService>();
// Registrando o serviço de pedidos no contêiner de serviços
builder.Services.AddScoped<IPedidoService, PedidoService>();

// Adicionando o middleware para lidar com exceções globais
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddProblemDetails();

var app = builder.Build();

// Adicionando o middleware para lidar com exceções globais
app.UseExceptionHandler();

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

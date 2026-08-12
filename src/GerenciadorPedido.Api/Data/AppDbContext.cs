using Microsoft.EntityFrameworkCore;

// Essa classe será responsável por criar o banco de dados
namespace GerenciadorPedido.Api.Data
{
    public class AppDbContext: DbContext    
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
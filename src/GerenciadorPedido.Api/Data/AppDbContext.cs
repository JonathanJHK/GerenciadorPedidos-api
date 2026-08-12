using GerenciadorPedido.Api.Entities;
using Microsoft.EntityFrameworkCore;

// Essa classe é responsável por criar o banco de dados
namespace GerenciadorPedido.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Define as propriedades DbSet para as entidades Produto e Pedido
        public DbSet<Produto> Products => Set<Produto>();
        public DbSet<Pedido> Pedidos => Set<Pedido>();

        // Configura o modelo de dados usando o Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configura a entidade Produto
            modelBuilder.Entity<Produto>(entity =>
            {
                // Configura a propriedade Nome como obrigatória e define o tamanho máximo como 150 caracteres
                entity.Property(p => p.Nome)
                    .IsRequired()
                    .HasMaxLength(150);

                // Configura a propriedade Categoria como obrigatória e define o tamanho máximo como 100 caracteres
                entity.Property(p => p.Categoria)
                    .IsRequired()
                    .HasMaxLength(100);

                // Configura a propriedade Preco como obrigatória
                entity.Property(p => p.Preco)
                    .HasPrecision(18, 2);

                // Configura a propriedade QuantidadeEstoque como obrigatória
                entity.Property(p => p.QuantidadeEmEstoque)
                    .IsRequired();

                // Configura a propriedade DataDeCadastro como obrigatória
                entity.Property(p => p.DataDeCadastro)
                    .IsRequired();
            });

            // Configura a entidade Pedido
            modelBuilder.Entity<Pedido>(entity =>
            {
                // Configura a propriedade ProdutoId como obrigatória
                entity.Property(o => o.ProdutoId)
                  .IsRequired();

                // Configura a propriedade Quantidade como obrigatória
                entity.Property(o => o.Quantidade)
                    .IsRequired();

                // Configura a propriedade ValorTotal com precisão de 18 dígitos e 2 casas decimais
                entity.Property(o => o.ValorTotal)
                    .HasPrecision(18, 2);

                // Configura a propriedade DataDoPedido como obrigatória
                entity.Property(o => o.DataDoPedido)
                    .IsRequired();

                // Configura o relacionamento entre Pedido e Produto
                entity.HasOne(o => o.Produto)
                    .WithMany()
                    .HasForeignKey(o => o.ProdutoId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
namespace GerenciadorPedido.Api.Entities
{
    public class Pedido
    {
        public int Id { get; set; }

        public int ProdutoId { get; set; }

        public Produto Produto { get; set; } = null!;

        public int Quantidade { get; set; }

        public decimal ValorTotal { get; set; }

        public DateTime DataDoPedido { get; set; } = DateTime.UtcNow;
    }
}
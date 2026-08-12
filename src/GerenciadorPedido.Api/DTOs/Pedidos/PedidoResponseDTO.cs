using GerenciadorPedido.Api.DTOs.Produtos;

namespace GerenciadorPedido.Api.DTOs.Pedidos
{
    public class PedidoResponseDTO
    {
        public int Id { get; set; }

        public ProdutoResponseDTO Produto { get; set; } = null!;

        public int Quantidade { get; set; }

        public decimal ValorTotal { get; set; }

        public DateTime DataDoPedido { get; set; }
    }
}
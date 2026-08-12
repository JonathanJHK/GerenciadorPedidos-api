using System.ComponentModel.DataAnnotations;

namespace GerenciadorPedido.Api.DTOs.Pedidos
{
    public class CriarPedidoDTO
    {
        [Range(
        1,
        int.MaxValue,
        ErrorMessage = "O produto deve ser informado."
        )]
        public int ProdutoId { get; set; }

        [Range(
            1,
            int.MaxValue,
            ErrorMessage = "A quantidade deve ser maior que zero."
        )]
        public int Quantidade { get; set; }
    }
}
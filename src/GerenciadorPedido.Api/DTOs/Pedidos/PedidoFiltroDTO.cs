using System.ComponentModel.DataAnnotations;

namespace GerenciadorPedido.Api.DTOs.Pedidos
{
    public class PedidoFiltroDTO
    {

        [Range(1, int.MaxValue, ErrorMessage = "O número da página deve ser maior que 0.")]
        public int Pagina { get; set; } = 1;

        [Range(1, 100, ErrorMessage = "Itens por página deve ser maior que 0.")]
        public int itensPorPagina { get; set; } = 20;
    }
}
using GerenciadorPedido.Api.DTOs.Comum;
using GerenciadorPedido.Api.DTOs.Pedidos;

namespace GerenciadorPedido.Api.Interfaces
{
    public interface IPedidoService
    {
        Task<PedidoResponseDTO> Criar(CriarPedidoDTO novoPedido);

        Task<PaginacaoResponseDTO<PedidoResponseDTO>> Listar(PedidoFiltroDTO filtro);
    }
}
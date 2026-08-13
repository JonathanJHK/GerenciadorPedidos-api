using GerenciadorPedido.Api.DTOs.Comum;
using GerenciadorPedido.Api.DTOs.Produtos;

namespace GerenciadorPedido.Api.Interfaces
{
    public interface IProdutoService
    {
        Task<ProdutoResponseDTO?> Criar(CriarProdutoDTO novoProduto);

        Task<ProdutoResponseDTO?> BuscarPorId(int id);

        Task<PaginacaoResponseDTO<ProdutoResponseDTO>> Listar(
            ProdutoFiltroDTO filtro
        );

        Task<ProdutoResponseDTO?> Atualizar(
            int id,
            AtualizarProdutoDTO dadosAtualizados
        );

        Task<bool> Excluir(int id);
    }
}
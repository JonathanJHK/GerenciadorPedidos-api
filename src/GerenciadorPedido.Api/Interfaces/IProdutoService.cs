using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GerenciadorPedido.Api.DTOs.Produtos;

namespace GerenciadorPedido.Api.Interfaces
{
    public interface IProdutoService
    {
        Task<ProdutoResponseDTO?> Criar(CriarProdutoDTO novoProduto);

        Task<ProdutoResponseDTO?> BuscarPorId(int id);

        Task<List<ProdutoResponseDTO>> Listar();

        Task<ProdutoResponseDTO?> Atualizar(
            int id,
            AtualizarProdutoDTO dadosAtualizados
        );

        Task<bool> Excluir(int id);
    }
}
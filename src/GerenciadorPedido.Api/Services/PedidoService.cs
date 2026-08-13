using GerenciadorPedido.Api.Data;
using GerenciadorPedido.Api.DTOs.Comum;
using GerenciadorPedido.Api.DTOs.Pedidos;
using GerenciadorPedido.Api.DTOs.Produtos;
using GerenciadorPedido.Api.Entities;
using GerenciadorPedido.Api.Exceptions;
using GerenciadorPedido.Api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorPedido.Api.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly AppDbContext _appDbContext;

        public PedidoService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<PedidoResponseDTO> Criar(CriarPedidoDTO novoPedido)
        {
            // Busca o produto no banco de dados pelo ID
            var produto = await _appDbContext.Produtos.FirstOrDefaultAsync<Produto>(p => p.Id == novoPedido.ProdutoId);

            // Se o produto nao for encontrado, retorna produto não encontrado
            if (produto == null)
            {
                throw new ProdutoNaoEncontradoException();
            }

            // Se a quantidade em estoque do produto for menor que a quantidade do pedido, retorna estoque insuficiente
            if (produto.QuantidadeEmEstoque < novoPedido.Quantidade)
            {
                throw new EstoqueInsuficienteException();
            }

            // Calcula o valor total do pedido com base no preço do produto e na quantidade solicitada
            var valorTotal = produto.Preco * novoPedido.Quantidade;

            // Atualiza a quantidade em estoque do produto subtraindo a quantidade do pedido
            produto.QuantidadeEmEstoque -= novoPedido.Quantidade;

            var pedido = new Pedido
            {
                ProdutoId = produto.Id,
                Quantidade = novoPedido.Quantidade,
                ValorTotal = valorTotal
            };

            _appDbContext.Pedidos.Add(pedido);

            await _appDbContext.SaveChangesAsync();

            // Retorna o pedido criado
            return new PedidoResponseDTO
            {
                Id = pedido.Id,
                Produto = new ProdutoResponseDTO
                {
                    Id = produto.Id,
                    Nome = produto.Nome,
                    Categoria = produto.Categoria,
                    Preco = produto.Preco,
                    QuantidadeEmEstoque = produto.QuantidadeEmEstoque
                },
                Quantidade = pedido.Quantidade,
                ValorTotal = pedido.ValorTotal,
                DataDoPedido = pedido.DataDoPedido
            };
        }

        public async Task<PaginacaoResponseDTO<PedidoResponseDTO>> Listar(PedidoFiltroDTO filtro)
        {
            var pedidos = await _appDbContext.Pedidos
                .AsNoTracking()
                .Include(p => p.Produto) // Inclui o produto relacionado ao pedido
                .OrderByDescending(p => p.DataDoPedido)
                .Skip((filtro.Pagina - 1) * filtro.itensPorPagina)
                .Take(filtro.itensPorPagina)
                .ToListAsync();

            // Calcula o total de itens que atendem aos filtros aplicados
            var totalPedidos = await _appDbContext.Pedidos.CountAsync();

            // Retorna a lista de pedidos
            return new PaginacaoResponseDTO<PedidoResponseDTO>
            {
                Itens = pedidos.Select(pedido => new PedidoResponseDTO
                {
                    Id = pedido.Id,
                    Produto = new ProdutoResponseDTO
                    {
                        Id = pedido.Produto.Id,
                        Nome = pedido.Produto.Nome,
                        Categoria = pedido.Produto.Categoria,
                        Preco = pedido.Produto.Preco,
                        QuantidadeEmEstoque = pedido.Produto.QuantidadeEmEstoque
                    },
                    Quantidade = pedido.Quantidade,
                    ValorTotal = pedido.ValorTotal,
                    DataDoPedido = pedido.DataDoPedido
                }).ToList(),
                itensPorPagina = filtro.itensPorPagina,
                Pagina = filtro.Pagina,
                TotalItens = totalPedidos,
                TotalPaginas = (int)Math.Ceiling((double)totalPedidos / filtro.itensPorPagina),
            };

        }

    }
}
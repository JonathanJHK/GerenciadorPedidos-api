using GerenciadorPedido.Api.Data;
using GerenciadorPedido.Api.DTOs.Comum;
using GerenciadorPedido.Api.DTOs.Produtos;
using GerenciadorPedido.Api.Entities;
using GerenciadorPedido.Api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorPedido.Api.Services
{
    public class ProdutoService : IProdutoService
    {
        // Atributo para armazenar o contexto do banco de dados
        private readonly AppDbContext _appDbContext;

        public ProdutoService(AppDbContext appDbContext)
        {
            // Injeção de dependência do banco de dados
            _appDbContext = appDbContext;
        }

        public async Task<ProdutoResponseDTO?> Criar(CriarProdutoDTO novoProduto)
        {
            // Pergunta ao banco se já existe algum produto com o mesmo nome (ignorando maiúsculas e minúsculas) | EF.Functions.ILike(p.Nome, novoProduto.Nome)
            bool nomeProdutoJaExiste = await _appDbContext.Produtos.AnyAsync(p => p.Nome.ToLower() == novoProduto.Nome.ToLower());

            if (nomeProdutoJaExiste)
            {
                // Se já existir, retorna null
                return null;
            }

            // Cria uma nova instância da entidade Produto
            var produto = new Produto
            {
                Nome = novoProduto.Nome,
                Categoria = novoProduto.Categoria,
                Preco = novoProduto.Preco,
                QuantidadeEmEstoque = novoProduto.QuantidadeEmEstoque
            };

            // Adiciona o produto ao banco de dados
            _appDbContext.Produtos.Add(produto);
            // Salva as alterações no banco de dados
            await _appDbContext.SaveChangesAsync();

            // Retorna o produto criado
            return new ProdutoResponseDTO
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Categoria = produto.Categoria,
                Preco = produto.Preco,
                QuantidadeEmEstoque = produto.QuantidadeEmEstoque
            };
        }

        public async Task<ProdutoResponseDTO?> BuscarPorId(int id)
        {
            // Busca o produto no banco de dados pelo ID
            var produto = await _appDbContext.Produtos
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            // Se o produto não for encontrado, retorna null
            if (produto is null)
            {
                return null;
            }

            // Retorna o produto encontrado
            return new ProdutoResponseDTO
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Categoria = produto.Categoria,
                Preco = produto.Preco,
                QuantidadeEmEstoque = produto.QuantidadeEmEstoque
            };
        }

        public async Task<PaginacaoResponseDTO<ProdutoResponseDTO>> Listar(ProdutoFiltroDTO filtro)
        {
            // Cria uma consulta de produtos usado para construir a consulta de forma dinâmica
            var produtosQuery = _appDbContext.Produtos.AsNoTracking().AsQueryable();

            // Se o filtro de nome for informado, filtra os produtos pelo nome
            if (!string.IsNullOrEmpty(filtro.Nome))
            {
                produtosQuery = produtosQuery.Where(p => p.Nome.ToLower().Contains(filtro.Nome.ToLower()));
            }

            // Se o filtro de categoria for informado, filtra os produtos pela categoria
            if (!string.IsNullOrEmpty(filtro.Categoria))
            {
                produtosQuery = produtosQuery.Where(p => p.Categoria.ToLower().Contains(filtro.Categoria.ToLower()));
            }

            // Se o filtro de preço mínimo for informado, filtra os produtos pelo preço mínimo
            if (filtro.PrecoMinimo.HasValue)
            {
                produtosQuery = produtosQuery.Where(p => p.Preco >= filtro.PrecoMinimo.Value);
            }

            // Se o filtro de preço máximo for informado, filtra os produtos pelo preço máximo
            if (filtro.PrecoMaximo.HasValue)
            {
                produtosQuery = produtosQuery.Where(p => p.Preco <= filtro.PrecoMaximo.Value);
            }

            // Calcula o total de itens que atendem aos filtros aplicados
            var totalItens = await produtosQuery.CountAsync();

            var produtos = await produtosQuery
            .AsNoTracking()
            .OrderBy(p => p.Id)
            .Skip((filtro.Pagina - 1) * filtro.TamanhoPagina) // Calcula a quantidade de itens a serem pulados com base na página atual e no tamanho da página
            .Take(filtro.TamanhoPagina) // Limita a quantidade de itens retornados com base no tamanho da página
            .ToListAsync();

            // Retorna a lista de produtos mapeada para ProdutoResponseDTO
            return new PaginacaoResponseDTO<ProdutoResponseDTO>
            {
                Itens = produtos.Select(p => new ProdutoResponseDTO
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    Categoria = p.Categoria,
                    Preco = p.Preco,
                    QuantidadeEmEstoque = p.QuantidadeEmEstoque,
                    DataDeCadastro = p.DataDeCadastro
                }).ToList(),
                Pagina = filtro.Pagina,
                TamanhoPagina = filtro.TamanhoPagina,
                TotalItens = totalItens,
                TotalPaginas = (int)Math.Ceiling((double)totalItens / filtro.TamanhoPagina) // Calcula o total de páginas com base no total de itens e no tamanho da página
            };

        }

        public async Task<ProdutoResponseDTO?> Atualizar(int id, AtualizarProdutoDTO dadosAtualizados)
        {
            // Busca o produto no banco de dados pelo ID
            var produto = await _appDbContext.Produtos
            .FirstOrDefaultAsync(p => p.Id == id);

            // Se o produto nao for encontrado, retorna null
            if (produto is null)
            {
                return null;
            }

            // Verifica se já existe outro produto com o mesmo nome (ignorando maiúsculas e minúsculas) e com ID diferente do produto que está sendo atualizado
            bool nomeProdutoJaExiste = await _appDbContext.Produtos
                .AnyAsync(p => p.Nome.ToLower() == dadosAtualizados.Nome.ToLower() && p.Id != id);

            // Se já existir, retorna null
            if (nomeProdutoJaExiste)
            {
                return null;
            }

            // Atualiza os dados do produto
            produto.Nome = dadosAtualizados.Nome;
            produto.Categoria = dadosAtualizados.Categoria;
            produto.Preco = dadosAtualizados.Preco;
            produto.QuantidadeEmEstoque = dadosAtualizados.QuantidadeEmEstoque;

            // Salva as alterações no banco de dados
            await _appDbContext.SaveChangesAsync();

            return new ProdutoResponseDTO
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Categoria = produto.Categoria,
                Preco = produto.Preco,
                QuantidadeEmEstoque = produto.QuantidadeEmEstoque,
                DataDeCadastro = produto.DataDeCadastro
            };

        }

        public async Task<bool> Excluir(int id)
        {
            // Busca o produto no banco de dados pelo ID
            var produto = await _appDbContext.Produtos
                .FirstOrDefaultAsync(p => p.Id == id);

            // Se o produto não for encontrado, retorna false
            if (produto is null)
            {
                return false;
            }

            // Remove o produto do banco de dados
            _appDbContext.Produtos.Remove(produto);

            // Salva as alterações no banco de dados
            await _appDbContext.SaveChangesAsync();

            return true;
        }
    }
}
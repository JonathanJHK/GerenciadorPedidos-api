using GerenciadorPedido.Api.DTOs.Comum;
using GerenciadorPedido.Api.DTOs.Produtos;
using GerenciadorPedido.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GerenciadorPedido.Api.Controllers
{
    [ApiController]
    [Route("api/produtos")]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoService _produtoService;

        public ProdutosController(IProdutoService produtoService)
        {
            _produtoService = produtoService;
        }

        [HttpPost]
        public async Task<ActionResult<ProdutoResponseDTO>> Criar([FromBody] CriarProdutoDTO novoProduto)
        {
            // Chama o serviço para criar um novo produto
            var produto = await _produtoService.Criar(novoProduto);

            // Verifica se o produto foi criado
            if (produto == null)
            {
                return BadRequest(
                    new
                    {
                        mensagem = "Já existe um produto com o mesmo nome."
                    }
                );
            }

            // Retorna o produto criado com o status HTTP 201 (Created)
            return Created("Produto criado com sucesso.", produto);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProdutoResponseDTO>> BuscarPorId(int id)
        {
            // Chama o serviço para buscar o produto pelo ID
            var produto = await _produtoService.BuscarPorId(id);

            // Verifica se o produto foi encontrado
            if (produto == null)
            {
                return NotFound(new
                {
                    mensagem = "Produto não encontrado."
                });
            }

            return StatusCode(200, produto);
        }

        [HttpGet]
        public async Task<ActionResult<PaginacaoResponseDTO<ProdutoResponseDTO>>> Listar([FromQuery] ProdutoFiltroDTO filtro)
        {
            var produtos = await _produtoService.Listar(filtro);

            return StatusCode(200, produtos);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ProdutoResponseDTO>> Atualizar(
            int id,
            AtualizarProdutoDTO dadosAtualizados
        )
        {
            // Chama o serviço para atualizar o produto
            var produto = await _produtoService.Atualizar(id, dadosAtualizados);

            // Verifica se o produto foi atualizado
            if (produto == null)
            {
                return NotFound(new
                {
                    mensagem = "Produto não encontrado ou já existe outro produto com o mesmo nome."
                });
            }

            return StatusCode(201, produto);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Excluir(int id)
        {
            var excluido = await _produtoService.Excluir(id);

            if (!excluido)
            {
                return NotFound(new
                {
                    mensagem = "Produto não encontrado."
                });
            }

            return StatusCode(200, new
            {
                mensagem = "Produto excluido com sucesso."
            });
        }
    }
}
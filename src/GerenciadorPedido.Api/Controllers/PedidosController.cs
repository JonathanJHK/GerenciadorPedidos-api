using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GerenciadorPedido.Api.DTOs.Comum;
using GerenciadorPedido.Api.DTOs.Pedidos;
using GerenciadorPedido.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GerenciadorPedido.Api.Controllers
{
    [ApiController]
    [Route("api/pedidos")]
    public class PedidosController : ControllerBase
    {
        private readonly IPedidoService _pedidoService;

        public PedidosController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        [HttpPost]
        public async Task<ActionResult<PedidoResponseDTO>> Criar([FromBody] CriarPedidoDTO novoPedido)
        {
            var pedido = await _pedidoService.Criar(novoPedido);

            return StatusCode(201, new
            {
                mensagem = "Pedido criado com sucesso.",
                pedido
            });
        }

        [HttpGet]
        public async Task<ActionResult<PaginacaoResponseDTO<PedidoResponseDTO>>> Listar([FromQuery] PedidoFiltroDTO filtro)
        {
            var pedidos = await _pedidoService.Listar(filtro);

            return StatusCode(200, pedidos);
        }
    }
}
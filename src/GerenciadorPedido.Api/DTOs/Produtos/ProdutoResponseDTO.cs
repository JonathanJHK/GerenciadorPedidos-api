using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciadorPedido.Api.DTOs.Produtos
{
    public class ProdutoResponseDTO
    {
        public int Id { get; set; }

        public string Nome { get; set; } = string.Empty;

        public string Categoria { get; set; } = string.Empty;

        public decimal Preco { get; set; }

        public int QuantidadeEmEstoque { get; set; }

        public DateTime DataDeCadastro { get; set; }
    }
}
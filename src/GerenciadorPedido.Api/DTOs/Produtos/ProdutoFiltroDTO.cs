using System.ComponentModel.DataAnnotations;

namespace GerenciadorPedido.Api.DTOs.Produtos
{
    public class ProdutoFiltroDTO
    {
        public string? Nome { get; set; }

        public string? Categoria { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "O preço mínimo não pode ser negativo.")]
        public decimal? PrecoMinimo { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "O preço máximo não pode ser negativo.")]
        public decimal? PrecoMaximo { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "O tamanho da pagina deve ser maior que 0.")]
        public int Pagina { get; set; } = 1;

        [Range(1, 100, ErrorMessage = "O tamanho da pagina deve ser maior que 0.")]
        public int TamanhoPagina { get; set; } = 20;
    }
}
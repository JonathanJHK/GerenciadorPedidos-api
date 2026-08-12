using System.ComponentModel.DataAnnotations;
namespace GerenciadorPedido.Api.DTOs.Produtos
{
    // DTO para criar um novo produto
    public class CriarProdutoDTO
    {
        [Required(ErrorMessage = "O nome do produto é obrigatório.")]
        [StringLength(
        150,
        MinimumLength = 2,
        ErrorMessage = "O nome do produto deve possuir entre 2 e 150 caracteres."
        )]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "A categoria do produto é obrigatória.")]
        [StringLength(
            100,
            MinimumLength = 2,
            ErrorMessage = "A categoria deve possuir entre 2 e 100 caracteres."
        )]
        public string Categoria { get; set; } = string.Empty;

        [Range(
            0.01,
            double.MaxValue,
            ErrorMessage = "O preço deve ser maior que zero."
        )]
        public decimal Preco { get; set; }

        [Range(
            0,
            int.MaxValue,
            ErrorMessage = "A quantidade em estoque não pode ser negativa."
        )]
        public int QuantidadeEmEstoque { get; set; }
    }
}
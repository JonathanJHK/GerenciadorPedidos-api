namespace GerenciadorPedido.Api.Entities
{
    public class Produto
    {
        public int Id { get; set; }

        public string Nome { get; set; } = string.Empty;

        public string Categoria { get; set; } = string.Empty;

        public decimal Preco { get; set; }

        public int QuantidadeEmEstoque { get; set; }

        public DateTime DataDeCadastro { get; set; } = DateTime.UtcNow;
    }
}
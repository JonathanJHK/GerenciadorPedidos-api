namespace GerenciadorPedido.Api.Exceptions
{
    public class ProdutoNaoEncontradoException : Exception
    {
        // Constructor padrão
        public ProdutoNaoEncontradoException()
        : base("Produto não encontrado.")
        {
        }

        // Constructor que recebe uma mensagem personalizada
        public ProdutoNaoEncontradoException(string mensagem)
            : base(mensagem)
        {
        }
    }
}
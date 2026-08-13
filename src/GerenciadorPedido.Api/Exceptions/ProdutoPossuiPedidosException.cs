namespace GerenciadorPedido.Api.Exceptions;

public class ProdutoPossuiPedidosException : Exception
{
    public ProdutoPossuiPedidosException()
        : base("Não é possível excluir um produto que possui pedidos associados.")
    {
    }

    // Constructor que recebe uma mensagem personalizada
    public ProdutoPossuiPedidosException(string mensagem)
        : base(mensagem)
    {
    }
}
namespace GerenciadorPedido.Api.Exceptions
{
    public class EstoqueInsuficienteException : Exception
    {
        // Constructor padrão
        public EstoqueInsuficienteException()
            : base("Estoque insuficiente.")
        {
        }

        // Constructor que recebe uma mensagem personalizada
        public EstoqueInsuficienteException(string mensagem)
            : base(mensagem)
        {
        }
    }
}
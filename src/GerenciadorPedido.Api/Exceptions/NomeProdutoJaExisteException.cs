using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciadorPedido.Api.Exceptions
{
    public class NomeProdutoJaExisteException : Exception
    {
        public NomeProdutoJaExisteException()
        : base("Já existe um produto com esse nome.")
        {
        }

        // Constructor que recebe uma mensagem personalizada
        public NomeProdutoJaExisteException(string mensagem)
        : base(mensagem)
        {
        }
    }

}
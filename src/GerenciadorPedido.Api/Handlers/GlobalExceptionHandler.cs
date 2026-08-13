using GerenciadorPedido.Api.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace GerenciadorPedido.Api.Handlers;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger
    )
    {
        _logger = logger;
    }

    //Handler para lidar com exceções globais (recebe o contexto HTTP, a exceção e o token de cancelamento asíncrono)
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        // Verifica o tipo da exceção e define o status code correspondente
        var statusCode = exception switch
        {
            ProdutoNaoEncontradoException
                => StatusCodes.Status404NotFound,

            EstoqueInsuficienteException
                => StatusCodes.Status400BadRequest,

            ProdutoPossuiPedidosException
                => StatusCodes.Status409Conflict,

            NomeProdutoJaExisteException
                => StatusCodes.Status409Conflict,

            _
                => StatusCodes.Status500InternalServerError
        };

        // Define a mensagem de erro baseado no tipo da exceção
        var mensagem = exception switch
        {
            ProdutoNaoEncontradoException => exception.Message,

            EstoqueInsuficienteException => exception.Message,

            ProdutoPossuiPedidosException => exception.Message,

            NomeProdutoJaExisteException => exception.Message,

            _ => "Ocorreu um erro interno no servidor."
        };

        // Se o status code for 500 (Internal Server Error), registra o erro no log
        if (statusCode == StatusCodes.Status500InternalServerError)
        {
            _logger.LogError(
                exception,
                "Erro inesperado durante a requisição."
            );
        }

        // Define o status code e escreve a mensagem de erro no corpo da resposta
        httpContext.Response.StatusCode = statusCode;

        // Escreve a mensagem de erro no corpo da resposta
        await httpContext.Response.WriteAsJsonAsync(
            new
            {
                mensagem
            },
            cancellationToken
        );

        return true;
    }
}
namespace GerenciadorPedido.Api.DTOs.Comum
{
    public class PaginacaoResponseDTO<T>
    {
        // Lista de itens da página atual
        public List<T> Itens { get; set; } = [];

        // Número da página atual
        public int Pagina { get; set; }

        // Tamanho da página (quantidade de itens por página)
        public int TamanhoPagina { get; set; }

        // Total de itens disponíveis no total
        public int TotalItens { get; set; }

        // Total de páginas disponíveis no total
        public int TotalPaginas { get; set; }
    }
}
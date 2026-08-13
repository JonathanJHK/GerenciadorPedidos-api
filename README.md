# Gerenciador de Pedidos

API REST desenvolvida em **ASP.NET Core** para gerenciamento de produtos e pedidos.

O projeto foi desenvolvido como parte de um desafio técnico para uma vaga de Desenvolvedor .NET Júnior e contempla cadastro de produtos, filtros, paginação, criação de pedidos, controle de estoque e tratamento centralizado de erros.

## Tecnologias utilizadas

* .NET 10
* ASP.NET Core Web API
* Entity Framework Core
* PostgreSQL 17
* Docker
* Docker Compose
* OpenAPI / Swagger
* Npgsql

## Funcionalidades

### Produtos

A API permite:

* Cadastrar um produto
* Buscar um produto por ID
* Listar produtos
* Atualizar um produto
* Excluir um produto
* Filtrar produtos por:

  * Nome
  * Categoria
  * Preço mínimo
  * Preço máximo
* Paginar a listagem de produtos

### Pedidos

A API permite:

* Realizar um pedido
* Listar pedidos realizados

Ao realizar um pedido, a aplicação:

1. Verifica se o produto existe.
2. Verifica se há estoque suficiente.
3. Calcula o valor total utilizando o preço do produto.
4. Atualiza a quantidade disponível em estoque.
5. Registra o pedido no banco de dados.

## Estrutura do projeto

A aplicação foi organizada buscando manter uma separação simples de responsabilidades.

```text
GerenciadorPedido.Api/
├── Controllers/                  # Recebe as requisições HTTP e retorna as respostas da API
│   ├── ProdutosController.cs     
│   └── PedidosController.cs     
│
├── Data/                         # Configuração de acesso e comunicação com o banco de dados
│   └── AppDbContext.cs           
│
├── DTOs/                         # Objetos utilizados para entrada e saída de dados da API
│   ├── Comum/                    
│   ├── Produtos/                 
│   └── Pedidos/                 
│
├── Entities/                     # Entidades que representam as tabelas do banco de dados
│   ├── Produto.cs                
│   └── Pedido.cs                 
│
├── Exceptions/                   # Exceções personalizadas utilizadas nas regras de negócio
│
├── Handlers/                     # Tratamento centralizado de exceções da aplicação
│   └── GlobalExceptionHandler.cs 
│
├── Interfaces/                   # Contratos implementados pelos Services
│   ├── IProdutoService.cs        
│   └── IPedidoService.cs         
│
├── Services/                     # Regras de negócio e acesso ao banco por meio do DbContext
│   ├── ProdutoService.cs         
│   └── PedidoService.cs          
│
├── Migrations/                   # Histórico de criação e atualização da estrutura do banco
│
├── Program.cs                    # Configuração da aplicação, DI, banco, handlers e pipeline HTTP
└── appsettings.json              # Configurações da aplicação e string de conexão com o banco
```

## Decisões de implementação

### DTOs

As entidades persistidas pelo Entity Framework Core não são utilizadas diretamente como entrada e saída dos endpoints.

Foram criados DTOs específicos para controlar os dados recebidos e retornados pela API, além de centralizar as validações de entrada.

Essa separação permite:

* Evitar exposição direta das entidades
* Controlar quais campos podem ser enviados pelo cliente
* Aplicar validações específicas para cada operação
* Manter separado o modelo de persistência do contrato HTTP da API

### Services

As regras de negócio ficam concentradas nos Services, evitando que os Controllers acumulem responsabilidades.

### Tratamento global de exceções

A aplicação utiliza `IExceptionHandler` para centralizar o tratamento de exceções.

Dessa forma, os Controllers não precisam possuir blocos `try/catch` repetidos.

Exemplos de respostas:

```text
Produto não encontrado -> 404 Not Found
Estoque insuficiente   -> 400 Bad Request
Erro inesperado        -> 500 Internal Server Error
```

Erros inesperados são registrados nos logs da aplicação e uma mensagem genérica é retornada ao cliente para evitar exposição de detalhes internos.

## Banco de dados

Foi utilizado **PostgreSQL 17** como banco de dados relacional.

O banco é executado através do Docker Compose.

Configuração utilizada:

```text
Database: gerenciador_pedido
Username: root
Password: root
```

A porta `5433` é utilizada no host para evitar conflitos com possíveis instalações locais do PostgreSQL.

```text
Host             Container
localhost:5433 -> PostgreSQL:5432
```

## Pré-requisitos

Para executar o projeto, é necessário possuir:

* .NET SDK 10
* Docker
* Docker Compose

## Como executar o projeto

### 1. Clonar o repositório

```bash
git clone <https://github.com/JonathanJHK/GerenciadorPedidos-api.git>
```

Entre na pasta do projeto:

```bash
cd GerenciadorPedido
```

### 2. Subir o PostgreSQL

Na raiz do projeto, execute:

```bash
docker compose up -d
```

Para verificar se o container está rodando:

```bash
docker ps
```

Deve existir um container chamado:

```text
gerenciador-pedido-db
```

### 3. Restaurar as dependências

Entre na pasta da API:

```bash
cd src/GerenciadorPedido.Api
```

Execute:

```bash
dotnet restore
```

### 4. Aplicar as migrations

Com o PostgreSQL em execução, execute:

```bash
dotnet ef database update
```

Esse comando cria ou atualiza a estrutura do banco utilizando as migrations do Entity Framework Core.

Para visualizar as migrations existentes:

```bash
dotnet ef migrations list
```

### 5. Executar a API

Execute:

```bash
dotnet run
```

A URL da aplicação será exibida no terminal.

## Swagger

Com a aplicação em execução no ambiente de desenvolvimento, acesse a URL informada no terminal seguida de:

```text
/swagger
```

Exemplo:

```text
http://localhost:5059/swagger
```

O Swagger permite testar os endpoints da aplicação sem necessidade de uma interface gráfica.

## Paginação

A listagem de produtos retorna os dados paginados juntamente com informações da paginação.

Exemplo:

```json
{
  "itens": [
    {
      "id": 1,
      "nome": "Mouse Logitech G203",
      "categoria": "Periféricos",
      "preco": 149.90,
      "quantidadeEmEstoque": 10,
      "dataDeCadastro": "2026-08-13T00:00:00Z"
    }
  ],
  "pagina": 1,
  "tamanhoPagina": 20,
  "totalItens": 1,
  "totalPaginas": 1
}
```

## Validações

Os DTOs utilizam Data Annotations para validar os dados recebidos pela API.

Entre as validações estão:

* Nome obrigatório
* Categoria obrigatória
* Preço válido
* Quantidade em estoque válida
* Produto obrigatório no pedido
* Quantidade do pedido maior que zero

Quando uma validação falha, a API retorna:

```text
400 Bad Request
```

## Migrations

As alterações da estrutura do banco são controladas através das migrations do Entity Framework Core.

Para criar uma nova migration:

```bash
dotnet ef migrations add NomeDaMigration
```

Para aplicar as migrations:

```bash
dotnet ef database update
```

Dessa forma, a estrutura do banco pode ser recriada sem necessidade de scripts SQL manuais.

## Docker

O PostgreSQL é executado utilizando Docker Compose.

Para iniciar:

```bash
docker compose up -d
```

Para visualizar os containers:

```bash
docker ps
```

Para visualizar os logs do banco:

```bash
docker logs gerenciador-pedido-db
```

Para parar os containers:

```bash
docker compose down
```

Para remover também o volume do banco:

```bash
docker compose down -v
```

> **Atenção:** esse comando também remove os dados armazenados no volume do PostgreSQL.

## Autor

Desenvolvido por Jonathan Kinjo.

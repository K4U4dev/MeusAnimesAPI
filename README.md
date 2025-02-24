# Meus Animes - Desafio Técnico

Este repositório contém a implementação de uma Web API para gerenciamento de animes e concorrer diretamente com a Crunchyroll. A API foi desenvolvida utilizando as tecnologias e padrões especificados no desafio, como .NET, Clean Architecture, Entity Framework, MediatR, e Docker.

## Funcionalidades da API

A Web API oferece as seguintes funcionalidades relacionadas a animes:

- **Obter todos os animes**
- **Obter anime por ID**
- **Cadastrar um novo anime**
- **Alterar os dados de um anime existente**
- **Excluir um anime**
- **Obter anime por nome**
- **Obter anime por nome do diretor**

### Dados dos Animes

Cada anime possui as seguintes propriedades:

- **ID**: Identificador único
- **Nome**: Nome do anime
- **Diretor**: Nome do diretor
- **Resumo**: Resumo breve sobre o anime

## Tecnologias Utilizadas

- **.NET (versão mais recente)**: Framework utilizado para o desenvolvimento da API
- **Clean Architecture**: Estrutura do projeto para garantir código bem organizado e escalável
- **Entity Framework**: ORM para interação com o banco de dados
- **Banco de Dados SQL Server ou MongoDB**: Para armazenamento dos dados dos animes
- **RESTful API**: Construção de endpoints seguindo os princípios REST
- **MediatR**: Biblioteca para implementação de mediadores, desacoplando os componentes do sistema
- **xUnit**: Framework de testes de unidade
- **Docker**: Suporte para deploy da aplicação em containers

## Endpoints da API

A API segue o padrão REST com versionamento nas rotas.

### Rotas disponíveis:

- **`GET /api/v1/Anime`**: Retorna todos os animes
- **`POST /api/v1/Anime`**: Cadastra um novo anime
- **`GET /api/v1/Anime/{id}`**: Retorna um anime específico pelo ID
- **`PUT /api/v1/Anime/{id}`**: Atualiza os dados de um anime
- **`DELETE /api/v1/Anime/{id}`**: Exclui um anime
- **`GET /api/v1/Anime/name/{name}`**: Retorna animes filtrados pelo nome
- **`GET /api/v1/Anime/director/{nameDirector}`**: Retorna animes filtrados pelo nome do diretor

## Como Executar o Projeto

Para executar o projeto, siga os passos abaixo:

1. **Clone o repositório**  
   Clone o repositório para o seu ambiente local:  
   ```bash
   git clone https://github.com/K4U4dev/MeusAnimesAPI.git
   cd meus-animes-api
   ```

2. **Restaure os pacotes NuGet**  
   No diretório raiz do projeto, execute o comando para restaurar os pacotes necessários:  
   ```bash
   dotnet restore
   ```

3. **Execute a API**  
   Acesse a pasta do projeto `MeusAnimesAPI` e execute o seguinte comando para rodar a API:  
   ```bash
   cd MeusAnimesAPI
   dotnet run
   ```


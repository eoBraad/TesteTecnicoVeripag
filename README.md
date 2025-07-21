# Weather Forecast API

## Descrição

Esta aplicação fornece informações meteorológicas e histórico de buscas utilizando serviços externos como SQL Server para armazenamento e cache. A API foi desenvolvida em C# utilizando o framework ASP.NET Core.

## Tecnologias Utilizadas

- **C#**
- **ASP.NET Core**: Framework robusto e rapido, permite configuração e desenvolvimento agil.
- **SQL Server** (armazenamento de dados): Banco de dados SQL robusto e feito para o uso empresarial, permite facil integração com dinversas tecnologias microsoft e também possui uma configuração simples.
- **Docker** (containerização): Facilita a configuração e o compartilhamento do projeto
- **Entity Framework**: ORM de alto nivel, facilitando o acesso aos dados e tratamento dos mesmos, contem diversas camadas de conversão de dados permitindo com que você trabalhe com fluides e velocidade.

## Notas de desenvolvimento

Este projeto foi desenvolvido seguindo os princípios da Arquitetura Limpa e aplicando conceitos essenciais do SOLID, visando garantir um código de fácil manutenção e entendimento.

Para facilitar o uso e a execução, a aplicação está configurada com Docker, incluindo um container para o SQL Server, tornando o ambiente de desenvolvimento e testes mais simples e portátil.

A documentação da API é disponibilizada via Swagger, e complementada com um arquivo Api.http que permite uma visão rápida e prática de todas as rotas disponíveis, facilitando o consumo e os testes da API.

No gerenciamento do banco de dados, utilizamos o Entity Framework Core com Migrations, o que auxilia na evolução estruturada do banco e proporciona melhor compreensão do modelo de dados para novos desenvolvedores ou durante alterações no esquema.

Todas as decisões técnicas e arquiteturais foram tomadas com o objetivo de promover um desenvolvimento de alta qualidade e tornar a aplicação mais prática para manutenção e evolução futura.

## Coverage

![Teste Covarage Result](test-coverage.png)

## Configuração

### Pré-requisitos

- Docker e Docker Compose instalados.
- .NET SDK 9.0 ou superior.

### Passos para execução

1. Clone o repositório:

   ```bash
   git clone https://github.com/eoBraad/TesteTecnicoVeripag.git
   cd TesteTecnicoVeripag
   ```

2. Crie os containers docker

   ```bash
   docker-compose up --build
   ```

3. De update no database

   ```bash
   dotnet ef database update --project src/Infrastructure --startup-project src/Api
   ```

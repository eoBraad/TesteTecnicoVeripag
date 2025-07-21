# Weather Forecast API

## Descrição

Esta aplicação fornece informações meteorológicas e histórico de buscas utilizando serviços externos como SQL Server para armazenamento e cache. A API foi desenvolvida em C# utilizando o framework ASP.NET Core.

## Tecnologias Utilizadas

- **C#**
- **ASP.NET Core**: Framework robusto e rapido, permite configuração e desenvolvimento agil.
- **SQL Server** (armazenamento de dados): Banco de dados SQL robusto e feito para o uso empresarial, permite facil integração com dinversas tecnologias microsoft e também possui uma configuração simples.
- **Docker** (containerização): Facilita a configuração e o compartilhamento do projeto
- **Entity Framework**: ORM de alto nivel, facilitando o acesso aos dados e tratamento dos mesmos, contem diversas camadas de conversão de dados permitindo com que você trabalhe com fluides e velocidade.

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

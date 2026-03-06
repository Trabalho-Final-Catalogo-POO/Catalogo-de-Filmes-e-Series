# Catálogo de Filmes e Séries

Projeto final da disciplina de **Programação Orientada a Objetos (POO)**, desenvolvido por estudantes de **Sistemas de Informação - PUC Minas Betim**.

## Descrição

Este sistema simula um catálogo de filmes e séries, permitindo:

- Cadastro de mídias (filmes e séries)
- Cadastro de usuários
- Favoritar mídias
- Listagem e busca
- Armazenamento de dados com **MySQL**
- Interface gráfica (separada)  com **Windows Forms**

## Funcionalidades:

- Cadastro de usuários

- Cadastro e listagem de filmes e séries

- Associação de mídias aos usuários como "favoritos"

- Busca de mídias por nome

- Favoritar Midia

- Exclusão de mídias (em desenvolvimento)

## Tecnologias Utilizadas

- C# (.NET)
- Windows Forms
- MySQL
- MySQL Connector/NET
- Programação Orientada a Objetos (POO)
- Diagrama de classe
- Modelos Conceituais

## Requisitos

- MySQL instalado e rodando em `localhost`
- Visual Studio com suporte a projetos Windows Forms
- [.NET SDK] instalado, link abaixo:
```bash
https://dotnet.microsoft.com/download
```
- **Pacote `MySql.Data` adicionado ao projeto (veja abaixo)**
  

## Instalação do pacote MySQL

Para que o código se conecte ao banco de dados corretamente, é necessário instalar o pacote de integração com o MySQL. Execute no terminal, dentro da pasta do projeto:

```bash
dotnet add package MySql.Data
```


## Configuração do Banco de Dados

O código SQL necessário para criação do banco de dados e suas tabelas está disponível no arquivo `DatabaseCatalogo.sql`, localizado na raiz do projeto.

Você pode executá-lo diretamente no MySQL Workbench, DBeaver ou outro cliente de sua preferência.

## Conexão com o banco:

No arquivo Conexao.cs, atualize a string de conexão conforme sua configuração local:
```csharp
 string conexao = "server=localhost;user=root;password=Root190406@;database=Catalogo_Filmes_Series";
```

## Como Executar

Clone o repositório:

```bash 
 git clone https://github.com/Trabalho-Final-Catalogo-POO/Catalogo-de-Filmes-e-Series.git
```

- Execute o script DATABASECatalogo.sql no MySQL

- Abra o projeto no Visual Studio

- Verifique a string de conexão em Conexao.cs

- No terminal, adicione o pacote MySQL:

```bash
dotnet add package MySql.Data
```

- Compile e execute o projeto (F5 ou dotnet run no terminal)

## Autores

- Arthur Henry Martins Brito
  
- João Marcos Moreira Laudares

- Vladimir Valentim Vieira da Costa


## Licença

Este projeto é de uso educacional e não possui fins comerciais.






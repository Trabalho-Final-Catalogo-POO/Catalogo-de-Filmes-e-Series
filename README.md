# 🎬 Catálogo de Filmes e Séries

Projeto final da disciplina de **Programação Orientada a Objetos (POO)**, desenvolvido por estudantes de **Sistemas de Informação - PUC Minas Betim**.

## 📌 Descrição

Este sistema simula um catálogo de filmes e séries, permitindo:

- Cadastro de mídias (filmes e séries)
- Cadastro de usuários
- Favoritar mídias
- Listagem e busca
- Armazenamento de dados com **MySQL**
- Interface gráfica (separada)  com **Windows Forms**

## 🚀 Tecnologias Utilizadas

- C# (.NET)
- Windows Forms
- MySQL
- MySQL Connector/NET
- Programação Orientada a Objetos (POO)

  ## 🧩 Requisitos

- MySQL instalado e rodando em `localhost`
- Visual Studio com suporte a projetos Windows Forms
- [.NET SDK](
```bash
https://dotnet.microsoft.com/download
```
) instalado
- **Pacote `MySql.Data` adicionado ao projeto (veja abaixo)**

### 📦 Instalação do pacote MySQL

Para que o código se conecte ao banco de dados corretamente, é necessário instalar o pacote de integração com o MySQL. Execute no terminal, dentro da pasta do projeto:

```bash
dotnet add package MySql.Data
```

## 🛠️ Configuração do Banco de Dados

O código SQL necessário para criação do banco de dados e suas tabelas está disponível no arquivo `DATABASECatalogo.sql`, localizado na raiz do projeto.

Você pode executá-lo diretamente no MySQL Workbench, DBeaver ou outro cliente de sua preferência.

### Exemplo de conteúdo do `DATABASECatalogo.sql`:

```sql
CREATE DATABASE Catalogo_Filmes_Series;
USE Catalogo_Filmes_Series;

CREATE TABLE Usuarios (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    NomeUsuario VARCHAR(25) NOT NULL,
    Senha VARCHAR(10) NOT NULL
);

CREATE TABLE Midias (
    id INT AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(25) NOT NULL,
    Genero VARCHAR(25) NOT NULL, 
    Tipo VARCHAR(10) NOT NULL,
    AnoLancamento INT
);

CREATE TABLE Filmes (
    MidiaId INT PRIMARY KEY,
    Duracao DOUBLE NOT NULL,
    Diretor VARCHAR(25),
    FOREIGN KEY (MidiaId)
        REFERENCES Midias (id)
        ON UPDATE CASCADE ON DELETE CASCADE
);

CREATE TABLE Series (
    MidiaId INT PRIMARY KEY,
    Duracao DOUBLE NOT NULL,
    Temporadas INT NOT NULL,
    QntEpisodios INT NOT NULL,
    FOREIGN KEY (MidiaId)
        REFERENCES Midias (id)
        ON UPDATE CASCADE ON DELETE CASCADE
);

CREATE TABLE Favoritos (
    MidiaId INT,
    UsuarioId INT,
    PRIMARY KEY (MidiaId, UsuarioId),
    FOREIGN KEY Fk_Midia (MidiaId)
        REFERENCES Midias (Id)
        ON UPDATE CASCADE ON DELETE CASCADE,
    FOREIGN KEY Fk_Usuarios (UsuarioId)
        REFERENCES Usuarios (Id)
        ON UPDATE CASCADE ON DELETE CASCADE
);

-- Configuração opcional de senha:
ALTER USER 'root'@'localhost' IDENTIFIED WITH mysql_native_password BY 'Root190406@';
FLUSH PRIVILEGES;
```
## 🔐 Conexão com o banco:

No arquivo Conexao.cs, atualize a string de conexão conforme sua configuração local:
```csharp
 string conexao = "server=localhost;user=root;password=Root190406@;database=Catalogo_Filmes_Series";
```

## 📸 Funcionalidades:

- ✅ Cadastro de usuários

- ✅ Cadastro e listagem de filmes e séries

- ✅ Associação de mídias aos usuários como "favoritos"

- ✅ Busca de mídias por nome

- 🚧 Edição e exclusão de mídias (em desenvolvimento)

## 📋 Como Executar

Clone o repositório:

```bash 
 git clone https://github.com/Trabalho-Final-Catalogo-POO/Catalogo-de-Filmes-e-Series.git
```

- Execute o script DATABASECatalogo.sql no MySQL

- Abra o projeto no Visual Studio

- Verifique a string de conexão em Conexao.cs

- Compile e execute o projeto (F5 ou dotnet run no terminal)

## 👨‍💻 Autores

- Arthur Henry Martins Brito
  
- João Marcos Moreira Laudares


## 📄 Licença

Este projeto é de uso educacional e não possui fins comerciais.





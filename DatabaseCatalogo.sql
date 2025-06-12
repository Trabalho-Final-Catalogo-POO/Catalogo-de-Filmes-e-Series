create database Catalogo_Filmes_Series;
use Catalogo_Filmes_Series;

CREATE TABLE Usuarios (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    NomeUsuario VARCHAR(25) NOT NULL,
    Senha VARCHAR(10) NOT NULL
);

CREATE TABLE Midias (
    id INT AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(25) NOT NULL ,
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
    PRIMARY KEY (MidiaId , UsuarioId),
    FOREIGN KEY Fk_Midia (MidiaId)
        REFERENCES Midias (Id)
		ON UPDATE CASCADE ON DELETE CASCADE,
    FOREIGN KEY Fk_Usuarios (UsuarioId)
        REFERENCES Usuarios (Id)
        ON UPDATE CASCADE ON DELETE CASCADE
);

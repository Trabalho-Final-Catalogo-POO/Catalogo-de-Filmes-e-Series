using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace backend
{
    public class BancoDeDadosService
    {
        private readonly BancoDeDados _banco;

        public BancoDeDadosService()
        {
            _banco = new BancoDeDados();
        }

        // Carrega todas as mídias do banco de dados
        public (List<Filme> filmes, List<Serie> series) CarregarMidiasBD()
        {
            var filmes = new List<Filme>();
            var series = new List<Serie>();

            ExecutarComConexao(conexao =>
            {
                string query = @"SELECT m.id, m.nome, m.genero, m.tipo, m.anoLancamento,
                        f.duracao AS filme_duracao, f.diretor,
                        s.duracao AS serie_duracao, s.temporadas, s.qntEpisodios
                    FROM Midias m
                    LEFT JOIN Filmes f ON m.id = f.MidiaId
                    LEFT JOIN Series s ON m.id = s.MidiaId";

                using (var cmd = new MySqlCommand(query, conexao))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32("id");
                        string nome = reader.GetString("nome");
                        string genero = reader.GetString("genero");
                        int ano = reader.GetInt32("anoLancamento");
                        string tipo = reader.GetString("tipo");

                        if (tipo == "filme")
                        {
                            double duracao = reader.GetDouble("filme_duracao");
                            string diretor = reader.GetString("diretor");
                            filmes.Add(new Filme(nome, genero, ano, id, duracao, diretor));
                        }
                        else if (tipo == "serie")
                        {
                            double duracao = reader.GetDouble("serie_duracao");
                            int temporadas = reader.GetInt32("temporadas");
                            int episodios = reader.GetInt32("qntEpisodios");
                            series.Add(new Serie(nome, genero, ano, id, duracao, temporadas, episodios));
                        }
                    }
                }
            });

            return (filmes, series);
        }

        // Carrega todos os usuários do banco de dados
        public List<Usuario> PreencherUsuariosComBD()
        {
            var usuarios = new List<Usuario>();

            ExecutarComConexao(conexao =>
            {
                string query = "SELECT Id, NomeUsuario, Senha FROM Usuarios";
                using (var cmd = new MySqlCommand(query, conexao))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32("Id");
                        string nome = reader.GetString("NomeUsuario");
                        string senha = reader.GetString("Senha");
                        usuarios.Add(new Usuario(nome, senha, id));
                    }
                }
            });

            return usuarios;
        }

        // Cadastra um novo usuário no banco
        public long CadastrarUsuarioBD(Usuario usuario)
        {
            return ExecutarComConexao(conexao =>
            {
                string query = "INSERT INTO Usuarios (NomeUsuario, Senha) VALUES (@nome, @senha)";
                using (var cmd = new MySqlCommand(query, conexao))
                {
                    cmd.Parameters.AddWithValue("@nome", usuario._Nome);
                    cmd.Parameters.AddWithValue("@senha", usuario._Senha);
                    cmd.ExecuteNonQuery();
                    return cmd.LastInsertedId;
                }
            });
        }

        // Salva uma mídia no banco (filme ou série)
        public long SalvarMidiaNoBanco(Midia midia)
        {
            return ExecutarComConexao(conexao =>
            {
                // Inserir na tabela principal de mídias
                string queryMidia = @"INSERT INTO Midias (nome, genero, tipo, anoLancamento) 
                                    VALUES (@nome, @genero, @tipo, @ano)";
                
                using (var cmdMidia = new MySqlCommand(queryMidia, conexao))
                {
                    cmdMidia.Parameters.AddWithValue("@nome", midia.Nome);
                    cmdMidia.Parameters.AddWithValue("@genero", midia.Genero);
                    cmdMidia.Parameters.AddWithValue("@ano", midia.AnoLancamento);

                    if (midia is Filme)
                        cmdMidia.Parameters.AddWithValue("@tipo", "filme");
                    else if (midia is Serie)
                        cmdMidia.Parameters.AddWithValue("@tipo", "serie");
                    else
                        throw new ArgumentException("Tipo de mídia desconhecido");

                    cmdMidia.ExecuteNonQuery();
                    long midiaId = cmdMidia.LastInsertedId;

                    // Inserir na tabela específica
                    if (midia is Filme filme)
                    {
                        string queryFilme = @"INSERT INTO Filmes (MidiaId, duracao, diretor) 
                                            VALUES (@id, @duracao, @diretor)";
                        
                        using (var cmdFilme = new MySqlCommand(queryFilme, conexao))
                        {
                            cmdFilme.Parameters.AddWithValue("@id", midiaId);
                            cmdFilme.Parameters.AddWithValue("@duracao", filme.Duracao);
                            cmdFilme.Parameters.AddWithValue("@diretor", filme.Diretor);
                            cmdFilme.ExecuteNonQuery();
                        }
                    }
                    else if (midia is Serie serie)
                    {
                        string querySerie = @"INSERT INTO Series (MidiaId, duracao, temporadas, qntEpisodios) 
                                            VALUES (@id, @duracao, @temporadas, @episodios)";
                        
                        using (var cmdSerie = new MySqlCommand(querySerie, conexao))
                        {
                            cmdSerie.Parameters.AddWithValue("@id", midiaId);
                            cmdSerie.Parameters.AddWithValue("@duracao", serie.Duracao);
                            cmdSerie.Parameters.AddWithValue("@temporadas", serie.Temporadas);
                            cmdSerie.Parameters.AddWithValue("@episodios", serie.QntEpisodios);
                            cmdSerie.ExecuteNonQuery();
                        }
                    }

                    return midiaId;
                }
            });
        }

        // Método auxiliar para gerenciar a conexão
        private void ExecutarComConexao(Action<MySqlConnection> acao)
        {
            bool conexaoAbertaLocalmente = false;
            
            try
            {
                if (_banco.Conexao.State != System.Data.ConnectionState.Open)
                {
                    _banco.Conexao.Open();
                    conexaoAbertaLocalmente = true;
                }

                acao(_banco.Conexao);
            }
            finally
            {
                if (conexaoAbertaLocalmente)
                    _banco.Conexao.Close();
            }
        }

        // Método genérico para operações que retornam valor
        private T ExecutarComConexao<T>(Func<MySqlConnection, T> acao)
        {
            bool conexaoAbertaLocalmente = false;
            
            try
            {
                if (_banco.Conexao.State != System.Data.ConnectionState.Open)
                {
                    _banco.Conexao.Open();
                    conexaoAbertaLocalmente = true;
                }

                return acao(_banco.Conexao);
            }
            finally
            {
                if (conexaoAbertaLocalmente)
                    _banco.Conexao.Close();
            }
        }
    }
}

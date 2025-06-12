using System;
using System.Runtime.CompilerServices;
using MySql.Data.MySqlClient;

public class Usuario
{
    public long Id;
    private string Nome;
    public string _Nome
    {
        get { return Nome; }
        set // Impede a modificação de um usuário existente sem um metódo
        {
            if (Nome == null)
                Nome = value;
        }
    }
    private string Senha;
    public string _Senha
    {
        get { return Senha; }
        set // Impede a modificação de uma senha existente sem um metódo
        {
            if (Senha == null)
                Senha = value;
        }
    }
    BancoDeDados bancoDeDados = new();
    private List<Midia> listaFavoritos = new();
    public Usuario() { }
    public Usuario(string Nome, string Senha, long Id)
    {
        this.Senha = Senha;
        this.Nome = Nome;
        this.Id = Id;
        AtualizaFavoritosComBD();
    }
    public void AtualizaFavoritosComBD()
    {
        listaFavoritos.Clear(); // Limpa a lista para recarrega-la com o banco de dados

        if (bancoDeDados.Conexao.State != System.Data.ConnectionState.Open)
            bancoDeDados.Conexao.Open();

        string query = @" SELECT m.id, m.nome, m.genero, m.tipo, m.anoLancamento,
                f.duracao AS filme_duracao, f.diretor,
                s.duracao AS serie_duracao, s.temporadas, s.qntEpisodios
                    FROM Favoritos fav
                    JOIN Midias m ON fav.MidiaId = m.id
                    LEFT JOIN Filmes f ON m.id = f.MidiaId
                    LEFT JOIN Series s ON m.id = s.MidiaId
                WHERE fav.UsuarioId = @UsuarioId";

        using MySqlCommand ComandoFavoritos = new(query, bancoDeDados.Conexao);
        ComandoFavoritos.Parameters.AddWithValue("@UsuarioId", Id);

        using MySqlDataReader LeitorFavoritos = ComandoFavoritos.ExecuteReader();

        while (LeitorFavoritos.Read())
        {
            string nome = LeitorFavoritos.GetString("Nome");
            string genero = LeitorFavoritos.GetString("Genero");
            int anolancamento = LeitorFavoritos.GetInt32("AnoLancamento");
            string tipo = LeitorFavoritos.GetString("tipo");
            int Id = LeitorFavoritos.GetInt32("Id");

            Midia nova = null;

            if (tipo.ToLower() == "filme")
            {
                double duracao = LeitorFavoritos.GetDouble("filme_duracao");
                string diretor = LeitorFavoritos.GetString("diretor");

                nova = new Filme(nome, genero, anolancamento, Id, duracao, diretor);
            }
            else if (tipo.ToLower() == "serie")
            {
                double duracao = LeitorFavoritos.GetDouble("serie_duracao");
                int QtdTemporadas = LeitorFavoritos.GetInt32("temporadas");
                int QtdEpisodios = LeitorFavoritos.GetInt32("QntEpisodios");

                nova = new Serie(nome, genero, anolancamento, Id, duracao, QtdTemporadas, QtdEpisodios);
            }
            if (BuscarFavoritoPorNome(nova.Nome) == null)
                listaFavoritos.Add(nova);
        }
        bancoDeDados.Conexao.Close();
    }

    // Manipulação de atributos de Usuario
    public void NovoNome(string Nome)
    {
        this.Nome = Nome;
    }
    
    public bool VerificaNome(string Nome) // Impede que o nome do usuário esteja vazio ou que se repita
    {
        if (Nome == "")
        {
            Console.WriteLine("\nO usuário não pode ficar vazio.");
            return false;
        }

        

        return true;
    }

    public bool VerificaSenha(string Senha)
    {
        if (string.IsNullOrWhiteSpace(Senha))
            Console.WriteLine("A senha não pode ser nula");
        else if (Senha.Length < 6)
            Console.WriteLine("Senha muito curta");
        else
            return true;

        return false;
    }
    public void AtualizarSenha(string Senha) { this.Senha = Senha; }

    // Manipulação de favoritos
    public void AdicionarFavorito(Midia midiaNova)
    {
        bool MidiaExiste = false;

        // Verificação da existencia da midia na lista de favoritos
        foreach (Midia m in listaFavoritos)
            if (m.Nome == midiaNova.Nome)
                MidiaExiste = true;

        if (!MidiaExiste)
        {
            AdicionarFavoritoBD(midiaNova);
            listaFavoritos.Add(midiaNova);
            Console.WriteLine($"'{midiaNova.Nome}' foi adicionado aos favoritos.");
        }
        else
            Console.WriteLine($"{midiaNova} \nJÁ EXISTE NOS FAVORITOS!!");
    }
    private void AdicionarFavoritoBD(Midia midia)
    {
        if (bancoDeDados.Conexao.State != System.Data.ConnectionState.Open)
            bancoDeDados.Conexao.Open();


        using MySqlCommand Comando = new($"INSERT INTO Favoritos (UsuarioId,MidiaId) VALUES (@UsuarioId, @MidiaId)", bancoDeDados.Conexao);
        Comando.Parameters.AddWithValue("@UsuarioId", Id);
        Comando.Parameters.AddWithValue("@MidiaId", midia.Id);

        Comando.ExecuteNonQuery();

        bancoDeDados.Conexao.Close();
    }
    public void ExibirFavoritos()
    {
        Console.WriteLine("========== Lista de Favoritos ==========\n");
        foreach (Midia m in listaFavoritos)
            Console.WriteLine(m);
    }
    public bool RemoverFavorito(Midia midia)
    {
        foreach (Midia m in listaFavoritos)
            if (midia == m)
            {
                if (bancoDeDados.Conexao.State != System.Data.ConnectionState.Open)
                    bancoDeDados.Conexao.Open();


                using MySqlCommand Comando = new("DELETE FROM Favoritos WHERE UsuarioId = @UsuarioId AND MidiaId = @MidiaId", bancoDeDados.Conexao);
                Comando.Parameters.AddWithValue("@UsuarioId", Id);
                Comando.Parameters.AddWithValue("@MidiaId", midia.Id);

                Comando.ExecuteNonQuery();

                bancoDeDados.Conexao.Close();

                listaFavoritos.Remove(m);
                return true;
            }

        return false;
    }
    public void RemoverFavoritos()
    {
        if (bancoDeDados.Conexao.State != System.Data.ConnectionState.Open)
            bancoDeDados.Conexao.Open();

        using MySqlCommand Comando = new("DELETE FROM Favoritos Where UsuarioId = @UsuarioId", bancoDeDados.Conexao);
        Comando.Parameters.AddWithValue("@UsuarioId", Id);
        Comando.ExecuteNonQuery();

        bancoDeDados.Conexao.Close();

        listaFavoritos.Clear();
    }
    public Midia BuscarFavoritoPorNome(string nome)
    {
        foreach (Midia m in listaFavoritos)
            if (m.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase))
                return m;

        return null;
    }
    public int QtdFavoritos() { return listaFavoritos.Count; }
}
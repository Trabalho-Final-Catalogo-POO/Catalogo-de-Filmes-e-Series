public class Catalogo
{
    BancoDeDados bancoDeDados = new();
    private List<Filme> filmes = new List<Filme>();
    private List<Serie> series = new List<Serie>();
    public Catalogo()
    {
        CarregarMidiasBD();
    }
    private void CarregarMidiasBD()
    {
        if (bancoDeDados.Conexao.State != System.Data.ConnectionState.Open)
            bancoDeDados.Conexao.Open();

        string query = @"SELECT  m.id, m.nome, m.genero, m.tipo, m.anoLancamento,
                f.duracao AS filme_duracao, f.diretor,
                s.duracao AS serie_duracao, s.temporadas, s.qntEpisodios
                    FROM Midias m
                    LEFT JOIN Filmes f ON m.id = f.MidiaId
                    LEFT JOIN Series s ON m.id = s.MidiaId";

        using MySqlCommand Comando = new(query, bancoDeDados.Conexao);
        using MySqlDataReader Leitor = Comando.ExecuteReader();

        while (Leitor.Read())
        {
            string nome = Leitor.GetString("Nome");
            string genero = Leitor.GetString("Genero");
            int anolancamento = Leitor.GetInt32("AnoLancamento");
            string tipo = Leitor.GetString("tipo");
            int Id = Leitor.GetInt32("id");

            if (tipo.ToLower() == "filme")
            {
                double duracao = Leitor.GetDouble("filme_duracao");
                string diretor = Leitor.GetString("diretor");

                Filme novo = new(nome, genero, anolancamento, Id, duracao, diretor);
                filmes.Add(novo);
            }
            else if (tipo.ToLower() == "serie")
            {
                double duracao = Leitor.GetDouble("serie_duracao");
                int QtdTemporadas = Leitor.GetInt32("temporadas");
                int QtdEpisodios = Leitor.GetInt32("QntEpisodios");

                Serie nova = new(nome, genero, anolancamento, Id, duracao, QtdTemporadas, QtdEpisodios);
                series.Add(nova);
            }
        }
        bancoDeDados.Conexao.Close();
    }

    // Manipulação de Filmes
    public void cadastrarFilme()
    {
        Console.Clear();
        string nome, genero, diretor;
        int anoLancamento;
        double duracao;

        Console.WriteLine("\n   Cadastrar Filme");
        Console.WriteLine("-----------------------------------");
        Console.WriteLine("Digite os dados do filme.");

        Console.Write("\nNome: ");
        nome = Console.ReadLine();

        Console.Write("Gênero: ");
        genero = Console.ReadLine();

        Console.Write("Diretor: ");
        diretor = Console.ReadLine();

        Console.Write("Ano de lançamento: ");
        anoLancamento = int.Parse(Console.ReadLine());

        Console.Write("Duração (em minutos): ");
        duracao = double.Parse(Console.ReadLine());

        Filme novo = new Filme(nome, genero, anoLancamento, duracao, diretor);

        if (BuscarPorNome(novo.Nome) == null)
        {
            BDCadastrarFilme(novo);
            filmes.Add(novo);
            Console.WriteLine("\nFilme cadastrado com sucesso!");
            Console.WriteLine("-----------------------------------\n");
        }
        else
            Console.WriteLine("\nFilme ou Serie já cadastrado com mesmo nome.\n");
    }
    public void cadastrarFilme(string nome, string genero, string diretor, int anoLancamento, double duracao)
    {
        Filme novo = new Filme(nome, genero, anoLancamento, duracao, diretor);

        if (BuscarPorNome(novo.Nome) == null)
        {
            BDCadastrarFilme(novo);
            filmes.Add(novo);
            Console.WriteLine("\nFilme cadastrado com sucesso!");
        }
        else
            Console.WriteLine("\nFilme ou Serie já cadastrado com mesmo nome.\n");
    }
    private void BDCadastrarFilme(Filme filme)
    {
        if (bancoDeDados.Conexao.State != System.Data.ConnectionState.Open)
            bancoDeDados.Conexao.Open();

        // Insere a parte midia do filme na tabela midias
        using MySqlCommand ComandoMidia = new($"INSERT INTO Midias (nome,genero,tipo,anolancamento) values (@nome,@genero,'filme',@anoLancamento)", bancoDeDados.Conexao);
        ComandoMidia.Parameters.AddWithValue("@nome", filme.Nome);
        ComandoMidia.Parameters.AddWithValue("@genero", filme.Genero);
        ComandoMidia.Parameters.AddWithValue("@anoLancamento", filme.AnoLancamento);
        ComandoMidia.ExecuteNonQuery();

        long MidiaId = ComandoMidia.LastInsertedId; // id da midia para inserir os dados na tabela filmes
        filme.Id = MidiaId; 

        // Insere o filme na tabela filmes
        using MySqlCommand ComandoFilme = new($"INSERT INTO Filmes (MidiaId,Duracao,Diretor) values (@midiaId,@duracao,@diretor)", bancoDeDados.Conexao);
        ComandoFilme.Parameters.AddWithValue("@midiaId", filme.Id);
        ComandoFilme.Parameters.AddWithValue("@duracao", filme.Duracao);
        ComandoFilme.Parameters.AddWithValue("@diretor", filme.Diretor);
        ComandoFilme.ExecuteNonQuery();

        bancoDeDados.Conexao.Close();
    }
    public void exibirFilmes()
    {
        Console.WriteLine("================ FILMES ================\n");
        foreach (Filme f in filmes)
            Console.WriteLine(f);

        Console.WriteLine();
    }
    public void RemoverFilmes(OperaçoesUsuario operaçoes)
    {
        if (bancoDeDados.Conexao.State != System.Data.ConnectionState.Open)
            bancoDeDados.Conexao.Open();

        using MySqlCommand Comando = new("Delete FROM Midias WHERE tipo='filme'", bancoDeDados.Conexao);
        Comando.ExecuteNonQuery();
        bancoDeDados.Conexao.Close();

        operaçoes.AtualizarUsuarios();

        filmes.Clear();
    }

    // Manipulação de Series
    public void cadastrarSerie()
    {
        Console.Clear();
        string nome, genero;
        int anoLancamento, temporadas, qntEpisodios;
        double duracao;

        Console.WriteLine("\n   Cadastrar Série: ");
        Console.WriteLine("-----------------------------------");
        Console.WriteLine("Digite os dados da série.");

        Console.Write("\nNome: ");
        nome = Console.ReadLine();

        Console.Write("Gênero: ");
        genero = Console.ReadLine();

        Console.Write("Ano de lançamento: ");
        anoLancamento = int.Parse(Console.ReadLine());

        Console.Write("Tempo médio de episódio (em minutos): ");
        duracao = double.Parse(Console.ReadLine());

        Console.Write("Quantidade de Temporadas: ");
        temporadas = int.Parse(Console.ReadLine());

        Console.Write("Quantidade de Episódios (Total): ");
        qntEpisodios = int.Parse(Console.ReadLine());

        Serie nova = new Serie(nome, genero, anoLancamento, duracao, temporadas, qntEpisodios);

        if (BuscarPorNome(nova.Nome) == null)
        {
            BDCadastrarSerie(nova);
            series.Add(nova);
            Console.WriteLine("\nSérie cadastrada com sucesso!");
            Console.WriteLine("-----------------------------------\n");
        }
        else
            Console.WriteLine("\nFilme ou Serie já cadastrado com mesmo nome.\n");
    }
    public void cadastrarSerie(string nome, string genero, int anoLancamento, int temporadas, int qntEpisodios, double duracao)
    {
        Serie nova = new Serie(nome, genero, anoLancamento, duracao, temporadas, qntEpisodios);

        if (BuscarPorNome(nova.Nome) == null)
        {
            BDCadastrarSerie(nova);
            series.Add(nova);
            Console.WriteLine("\nSérie cadastrada com sucesso!");
        }
        else
            Console.WriteLine("\nFilme ou Serie já cadastrado com mesmo nome.\n");
    }
    private void BDCadastrarSerie(Serie serie)
    {
        if (bancoDeDados.Conexao.State != System.Data.ConnectionState.Open)
            bancoDeDados.Conexao.Open();

        // Insere a parte midia da serie na tabela midias
        using MySqlCommand ComandoMidia = new($"INSERT INTO Midias (nome,genero,tipo,anolancamento) values (@nome,@genero,'serie',@anoLancamento)", bancoDeDados.Conexao);
        ComandoMidia.Parameters.AddWithValue("@nome", serie.Nome);
        ComandoMidia.Parameters.AddWithValue("@genero", serie.Genero);
        ComandoMidia.Parameters.AddWithValue("@anoLancamento", serie.AnoLancamento);
        ComandoMidia.ExecuteNonQuery();

        long MidiaId = ComandoMidia.LastInsertedId; // id da midia para inserir os dados na tabela series
        serie.Id = MidiaId;

        // Insere a serie na tabela series
        using MySqlCommand ComandoSerie = new($"INSERT INTO Series (MidiaId,Duracao,Temporadas,QntEpisodios) values (@midiaId,@duracao,@temporadas,@QtdEps)", bancoDeDados.Conexao);
        ComandoSerie.Parameters.AddWithValue("@midiaId", serie.Id);
        ComandoSerie.Parameters.AddWithValue("@duracao", serie.Duracao);
        ComandoSerie.Parameters.AddWithValue("@temporadas", serie.Temporadas);
        ComandoSerie.Parameters.AddWithValue("@QtdEps", serie.QntEpisodios);

        ComandoSerie.ExecuteNonQuery();

        bancoDeDados.Conexao.Close();
    }
    public void exibirSeries()
    {
        Console.WriteLine("================ SÉRIES ================\n");
        foreach (Serie s in series)
            Console.WriteLine(s);

        Console.WriteLine();
    }
    public void RemoverSeries(OperaçoesUsuario operacoes)
    {
        if (bancoDeDados.Conexao.State != System.Data.ConnectionState.Open)
            bancoDeDados.Conexao.Open();

        using MySqlCommand Comando = new("Delete FROM Midias WHERE tipo = 'serie' ", bancoDeDados.Conexao);
        Comando.ExecuteNonQuery();

        operacoes.AtualizarUsuarios();

        series.Clear();
    }

    // Manipulção de midia
    public bool RemoveMidia(Midia midia, OperaçoesUsuario operacao)
    {
        bool Encontrado = false;

        if (midia is Filme)
        {
            foreach (Filme F in filmes)
                if (midia.Nome == F.Nome)
                {
                    Encontrado = true;
                    filmes.Remove(F);
                    break;
                }
        }
        else if (midia is Serie)
        {
            foreach (Serie S in series)
                if (midia.Nome == S.Nome)
                {
                    Encontrado = true;
                    series.Remove(S);
                    break;
                }
        }
        else
            Console.WriteLine("tipo inválido");

        if (Encontrado)
        {
            if (bancoDeDados.Conexao.State != System.Data.ConnectionState.Open)
                bancoDeDados.Conexao.Open();

            using MySqlCommand Comando = new("DELETE FROM Midias WHERE nome=@value", bancoDeDados.Conexao);
            Comando.Parameters.AddWithValue("@value", midia.Nome);
            Comando.ExecuteNonQuery();
            bancoDeDados.Conexao.Close();

            operacao.AtualizarUsuarios();// Garante que a listaFavoritos é igual o banco de dados

            return true;
        }

        return false;
    }
    public Midia BuscarPorNome(string nome)
    {
        foreach (Filme f in filmes)
            if (f.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase))
                return f;

        foreach (Serie s in series)
            if (s.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase))
                return s;

        return null;
    }

    // Funçoes count
    public int QtdFilmes() { return filmes.Count; }
    public int QtdSeries() { return series.Count; }
    public int QtdMidias() { return QtdFilmes() + QtdSeries(); }
}

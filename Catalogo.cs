class Catalogo
{
    protected List<Filme> filmes = new List<Filme>();
    protected List<Serie> series = new List<Serie>();

    public Catalogo()
    {
        // Construtor vazio
    }

    public void cadastrarFilme()
    {
        Console.Clear();
        string nome, genero, diretor;
        int anoLancamento;
        double duracao;

        Console.WriteLine("*****************************************************");
        Console.WriteLine("\nCadastrar Filme: ");
        Console.WriteLine("Digite os dados do filme:");

        Console.Write("Nome: ");
        nome = Console.ReadLine();

        Console.Write("Gênero: ");
        genero = Console.ReadLine();

        Console.Write("Diretor: ");
        diretor = Console.ReadLine();

        Console.Write("Ano de lançamento: ");
        anoLancamento = int.Parse(Console.ReadLine());

        Console.Write("Duração (em minutos): ");
        duracao = double.Parse(Console.ReadLine());

        Filme novoFilme = new Filme(nome, genero, anoLancamento, duracao, diretor);
        filmes.Add(novoFilme);

        Console.WriteLine("\nFilme cadastrado com sucesso!");
        Console.WriteLine("*****************************************************\n");
    }
   // Funçao cadastro de filme, sobrecarga passando parametros
    public void cadastrarFilme(string nome, string genero, string diretor, int anoLancamento, double duracao)
    {
        bool FilmeNovo = true;
        Filme novo = new Filme(nome, genero, anoLancamento, duracao, diretor);

        // Nova busca por midia antes da adiçao
        foreach (Filme f in filmes) // Procura na lista de filmes 
            if (f.Nome == novo.Nome)
                FilmeNovo = false;

        foreach (Serie s in series) // Procura na lista de series 
            if (s.Nome == novo.Nome)
                FilmeNovo = false;

        if (FilmeNovo)
        {
            filmes.Add(novo);
            Console.WriteLine("\nFilme cadastrado com sucesso!");
        }
        else
            Console.WriteLine("\nFilme ou Serie já cadastrado com mesmo nome.\n");
    }
    public void cadastrarSerie()
    {
        Console.Clear();
        string nome, genero;
        int anoLancamento, temporadas, qntEpisodios;
        double duracao;

        Console.WriteLine("*****************************************************");
        Console.WriteLine("\nCadastrar Série: ");
        Console.WriteLine("Digite os dados da série:");

        Console.Write("Nome: ");
        nome = Console.ReadLine();

        Console.Write("Gênero: ");
        genero = Console.ReadLine();

        Console.Write("Ano de lançamento: ");
        anoLancamento = int.Parse(Console.ReadLine());

        Console.Write("Duração média por episódio: ");
        duracao = double.Parse(Console.ReadLine());

        Console.Write("Quantidade de temporadas: ");
        temporadas = int.Parse(Console.ReadLine());

        Console.Write("Quantidade de episódios: ");
        qntEpisodios = int.Parse(Console.ReadLine());

        Serie novaSerie = new Serie(nome, genero, anoLancamento, duracao, temporadas, qntEpisodios);
        series.Add(novaSerie);

        Console.WriteLine("\nSérie cadastrada com sucesso!");
        Console.WriteLine("*****************************************************\n");
    }
     // Funçao cadastro de serie, sobrecarga passando parametros
    public void cadastrarSerie(string nome, string genero, string diretor, int anoLancamento, int temporadas, int qntEpisodios, double duracao)
    {
        bool SerieNova = true;
        Serie nova = new Serie(nome, genero, anoLancamento, duracao, temporadas, qntEpisodios);

        // Nova busca por midia antes da adiçao
        foreach (Serie s in series) // Procura na lista de series 
            if (s.Nome == nova.Nome)
                SerieNova = false;

        foreach (Filme f in filmes) // Procura na lista de filmes 
            if (f.Nome == nova.Nome)
                SerieNova = false;

        if (SerieNova)
        {
            series.Add(nova);
            Console.WriteLine("\nSérie cadastrada com sucesso!");
        }
        else
            Console.WriteLine("\nFilme ou Serie já cadastrado com mesmo nome.\n");
    }
    public void exibirFilmes()
    {
        Console.WriteLine("***************** FILMES *****************");
        foreach (Filme f in filmes)
        {
            Console.WriteLine(f);
        }
        Console.WriteLine();
    }

    public void exibirSeries()
    {
        Console.WriteLine("***************** SÉRIES *****************");
        foreach (Serie s in series)
        {
            Console.WriteLine(s);
        }
        Console.WriteLine();
    }

    public virtual Midia BuscarPorNome(string nome)
    {
        foreach (Filme f in filmes)
        {
            if (f.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase))
                return f;
        }

        foreach (Serie s in series)
        {
            if (s.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase))
                return s;
        }

        return null;
    }
    
    // Funçoes count
    public int QtdFilmes() { return filmes.Count; }
    public int QtdSeries() { return series.Count; }
    public int QtdMidias() { return QtdFilmes() + QtdSeries(); }

    public bool RemoveMidia(Midia midia)
    {
        List<Midia> Midias = new();
        Midias.AddRange(filmes);
        Midias.AddRange(series);

        foreach (Midia m in Midias)
            if (midia == m)
            {
                Midias.Remove(midia);
                return true;
            }

        return false;
    }

    public void RemoverFilmes()
    {
        filmes.Clear();
    }
     public void RemoverSeries()
    {
        series.Clear();
    }
}

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
}

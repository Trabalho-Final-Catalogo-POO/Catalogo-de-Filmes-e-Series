using System;
using System.Collections.Generic;


class Program
{
    static void Main(string[] args)
    {
        Catalogo catalogo = new Catalogo();
        Usuario usuario = new Usuario();
        catalogo.cadastrarFilme();
        catalogo.cadastrarSerie();

        catalogo.exibirFilmes();
        catalogo.exibirSeries();



        Console.Write("Digite o nome da mídia para adicionar aos favoritos: ");
        string nomeMidia = Console.ReadLine();

        Midia midiaEncontrada = catalogo.BuscarPorNome(nomeMidia);

        if (midiaEncontrada != null)
        {
            usuario.favoritos.AdicionarNaLista(midiaEncontrada);
            Console.WriteLine("Mídia adicionada aos favoritos!");
        }
        else
        {
            Console.WriteLine("Mídia não encontrada.");
        }

        usuario.favoritos.ExibirFavoritos();
    }
}

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

class Midia
{
    public string Nome { get; set; }
    public string Genero { get; set; }
    public int AnoLancamento { get; set; }

    public Midia(string nome, string genero, int anoLancamento)
    {
        Nome = nome;
        Genero = genero;
        AnoLancamento = anoLancamento;
    }

    public override string ToString()
    {
        return $"{Nome} ({AnoLancamento}) - Gênero: {Genero}";
    }


}

class Filme : Midia
{
    public double Duracao { get; set; }
    public string Diretor { get; set; }

    public Filme(string nome, string genero, int anoLancamento, double duracao, string diretor)
        : base(nome, genero, anoLancamento)
    {
        Duracao = duracao;
        Diretor = diretor;
    }

    public override string ToString()
    {
        return "[Filme] - " + base.ToString() + $", Duração: {Duracao} min, Diretor: {Diretor}";
    }
}

class Serie : Midia
{
    public double Duracao { get; set; }
    public int Temporadas { get; set; }
    public int QntEpisodios { get; set; }

    public Serie(string nome, string genero, int anoLancamento, double duracao, int temporadas, int qntEpisodios)
        : base(nome, genero, anoLancamento)
    {
        Duracao = duracao;
        Temporadas = temporadas;
        QntEpisodios = qntEpisodios;
    }

    public override string ToString()
    {
        return "[Serie] - " + base.ToString() + $", {Temporadas} temporadas, {QntEpisodios} episódios, Duração média: {Duracao} min";
    }
}

class Favoritos
{

    private List<Midia> listaFavoritos = new List<Midia>();

    public void AdicionarNaLista(Midia midia)
    {
        listaFavoritos.Add(midia);
        Console.WriteLine($"'{midia.Nome}' foi adicionado aos favoritos.");
    }

    public void ExibirFavoritos()
    {
        Console.WriteLine("***** Lista de Favoritos *****");
        foreach (Midia m in listaFavoritos)
        {
            Console.WriteLine(m);
        }
    }


}

class Usuario
{
    public string NomeUsuario { get; private set; }
    public string Senha { get; private set; }
    public bool Cadastrado { get; private set; } = false;

    public Favoritos favoritos { get; private set; } = new Favoritos();


    public void Cadastrar()
    {
        if (!Cadastrado)
        {
            Console.Write("Digite o nome de usuário: ");
            NomeUsuario = Console.ReadLine();
            Console.Write("Digite a senha: ");
            Senha = Console.ReadLine();
            Cadastrado = true;
            Console.WriteLine("Usuário cadastrado com sucesso!\n");
        }
        else
        {
            Console.WriteLine("Usuário já cadastrado.\n");
        }
    }

    public bool FazerLogin()
    {
        if (!Cadastrado)
        {
            Console.WriteLine("Nenhum usuário cadastrado. Cadastre primeiro.\n");
            return false;
        }

        Console.Write("Digite o nome de usuário: ");
        string nome = Console.ReadLine();
        Console.Write("Digite a senha: ");
        string senha = Console.ReadLine();

        if (nome == NomeUsuario && senha == Senha)
        {
            Console.WriteLine("Login bem-sucedido!\n");
            return true;
        }
        else
        {
            Console.WriteLine("Usuário ou senha incorretos.\n");
            return false;
        }
    }
}
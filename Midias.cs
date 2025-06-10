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

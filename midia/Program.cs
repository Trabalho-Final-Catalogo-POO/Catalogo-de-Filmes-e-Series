using System;

// Classe "Midia" é a classe pai de "Serie" e "Filme"
class Midia{
    // Atributos
    protected int ID_Midia;
    protected string Titulo;
    protected string Genero;
    protected int Ano_Lancamento;
    protected List<Avaliacao> Avaliacoes = new List<Avaliacao>();
    protected double Media_Avaliacoes;

    // Construtor
    public Midia(int id_midia, string titulo, string genero, int ano_lancamento, double media_avaliacoes){
        ID_Midia = id_midia;
        Titulo = titulo;
        Genero = genero;
        Ano_Lancamento = ano_lancamento;
        Media_Avaliacoes = calcularMediaAvaliacoes();   // A média é definida pelo retorno da função "calcularMediaAvaliacoes()"
    }

    // Método que calcula a média das avaliações
    public double calcularMediaAvaliacoes(){
        // Contador para ser o denominador do cálculo da média
        int c = 0;
        // Recebe a soma de todas as avaliações para calcular a média
        double soma_avaliacoes = 0;

        // Para cada avaliação na lista de avaliações, converte para double e 
        // soma o valor do atributo ".nota" na variável "soma_avaliações"
        foreach (Avaliacao avaliacao in Avaliacoes)
        {
            soma_avaliacoes += double.Parse(avaliacao.nota);
            c++;
        }
        
        // Retorna a média das avaliações, dividindo a soma total das notas pela quantidade de avaliações que a mídia recebeu
        return soma_avaliacoes / c;
    }
}

// Classe "Serie" herda de "Midia"
class Serie : Midia{
    // Atributos
    protected int Quantidade_Temporadas;
    protected int Quantidade_Episodios;
    protected List<Episodio> Lista_Episodios = new List<Episodio>();
    protected int Duracao_Total;

    // Construtor
    public Serie(int id_midia, string titulo, string genero, int ano_lancamento, double media_avaliacoes,
        int quantidade_temporadas, int quantidade_episodios)
        : base(id_midia, titulo, genero, ano_lancamento, media_avaliacoes)
    {
        Quantidade_Temporadas = quantidade_temporadas;
        Quantidade_Episodios = quantidade_episodios;
        Duracao_Total = calcularDuracaoTotal();
    }

    // Método para calcular a duração total dos episódios de uma série
    public int calcularDuracaoTotal()
    {
        // Recebe a soma da duração de todos os episódios da série
        int duracao_total = 0;

        // Para cada episódio da série, irá somar a duração do episódio ao total
        foreach (Episodio episodio in Lista_Episodios){
            duracao_total += episodio.Duracao_Episodio;
        }

        // Retorna a soma da duração de todos os episódios da série
        return duracao_total;
    }
}

// Classe "Episodio" herda de "Serie"
class Episodio : Serie{
    // Atributos
    protected int ID_Episodio;
    protected string Titulo_Episodio;
    public int Duracao_Episodio;

    // Construtor
    public Episodio(int id_midia, string titulo, string genero, int ano_lancamento, double media_avaliacoes,
        int quantidade_temporadas, int quantidade_episodios,
        int id_episodio, string titulo_episodio, int duracao_episodio)
        : base(id_midia, titulo, genero, ano_lancamento, media_avaliacoes, quantidade_temporadas, quantidade_episodios){
        ID_Episodio = id_episodio;
        Titulo_Episodio = titulo_episodio;
        Duracao_Episodio = duracao_episodio;
    }
}
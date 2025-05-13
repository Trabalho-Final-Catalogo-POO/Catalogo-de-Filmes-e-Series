using System;

// Classe mídia é a classe pai de Serie e Filme
class Midia{
    // Atributos
    protected int ID_Midia;
    protected string Titulo;
    protected string Genero;
    protected int Ano_Lancamento;
    // protected List<Avaliacao> Avaliacoes = new List<Avaliacao>(); !!! Sugestão de atributo para facilitar a visualização das avaliaçoes !!!
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
    // public int calcularMediaAvaliacoes(){
    //     // Contador para ser o denominador do cálculo da média
    //     int c;
    //     // Recebe a soma de todas as avaliações para calcular a média
    //     double soma_avaliacoes = 0;

    //     // Para cada avaliação na lista de avaliações, converte para double e 
    //     // soma o valor do atributo ".nota" na variável "soma_avaliações"
    //     foreach (Avaliacao avaliacao in Avaliacoes){
    //         soma_avaliacoes += double.Parse(avaliacao.nota);
    //     }
        
    //     // Retorna a média das avaliações, dividindo a soma total das notas pela quantidade de avaliações que a mídia recebeu
    //     return soma_avaliacoes / c;
    // }
}

// Classe "Série" herda de "Mídia"
class Serie : Midia{
    // Atributos
    private int Quantidade_Temporadas;
    private int Quantidade_Episodios;
    // private int Duracao_Episodio;    // Padronizado em minutos !!! sugiro a criação de uma classe "Episodio" para esse atributo !!!
    
    // Construtor
    public Serie(int id_midia, string titulo, string genero, int ano_lancamento, double media_avaliacoes, 
        int quantidade_temporadas, int quantidade_episodios, int duracao_episodio)
        : base(id_midia, titulo, genero, ano_lancamento, media_avaliacoes){
            Quantidade_Temporadas = quantidade_temporadas;
            Quantidade_Episodios = quantidade_episodios;
            // Duracao_Episodio = duracao_episodio;
    }

    // public double calcularDuracaoTotal(){
    //     fore
    // }
}
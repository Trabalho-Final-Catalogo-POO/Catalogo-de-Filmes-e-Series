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

public class Favoritos
{
    private List<Midia> listaFavoritos = new List<Midia>();

    public void AdicionarNaLista(Midia midiaNova)
    {
        bool MidiaExiste = false;
      
        // Verificação da existencia da midia na lista de favoritos
        foreach (Midia m in listaFavoritos)
            if (m.Nome == midiaNova.Nome)
                MidiaExiste = true;

        if (!MidiaExiste)
        {
            listaFavoritos.Add(midiaNova);
            Console.WriteLine($"'{midiaNova.Nome}' foi adicionado aos favoritos.");
        }
        else
            Console.WriteLine($"{midiaNova} \nJÁ EXISTE NOS FAVORITOS!!");
    }

    public void ExibirFavoritos()
    {
        Console.WriteLine("========== Lista de Favoritos ==========\n");
        foreach (Midia m in listaFavoritos)
            Console.WriteLine(m);
    }
  
    // Função count
    public int QtdFavoritos() { return listaFavoritos.Count; }

    public bool RemoverFavorito(Midia midia)
    {
        foreach (Midia m in listaFavoritos)
            if (midia == m)
            {
                listaFavoritos.Remove(m);
                return true;
            }

        return false;
    }
    
    public void RemoverFavoritos() { listaFavoritos.Clear(); }

    public Midia BuscarPorNome(string nome)
    {
        foreach (Midia m in listaFavoritos)
            if (m.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase))
                return m;

        return null;
    }
}
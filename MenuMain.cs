using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics.Arm;

class Program
{
    static void Main(string[] args)
    {
        Catalogo catalogo = new Catalogo();
        Usuario usuario = new Usuario();
        int opçao;

        do
        {
            opçao = Menu();
            switch (opçao)
            {
                case 1: // Cadastrar filme (DONE)
                    do
                    {
                        catalogo.cadastrarFilme();
                        Console.WriteLine($" {{{catalogo.QtdFilmes()}}} filme(s) cadastrado(s) no catalogo.");
                        Console.WriteLine("Deseja cadastrar outro filme?(Enter para confirmar)");
                    } while (Console.ReadKey(true).Key == ConsoleKey.Enter);
                    break;
                case 2: // Cadastrar serie (DONE)
                    do
                    {
                        catalogo.cadastrarSerie();
                        Console.WriteLine($" {{{catalogo.QtdSeries()}}} serie(s) cadastrada(s) no catalogo.");
                        Console.WriteLine("Deseja cadastrar outra serie?(Enter para confirmar)");
                    } while (Console.ReadKey(true).Key == ConsoleKey.Enter);
                    break;
                case 3: // Favoritar (DONE)
                    if (catalogo.QtdMidias() > 0) // Se existirem series ou filmes
                        if (usuario.favoritos.QtdFavoritos() < catalogo.QtdMidias()) // se ainda existirem series a serem favoritadas
                        {
                            Console.Clear();

                            Console.WriteLine($"\n {{{catalogo.QtdSeries() + catalogo.QtdFilmes()}}} midia(s) cadastrada(s) no catalogo,");
                            Console.WriteLine($"{{{usuario.favoritos.QtdFavoritos()}}} favoritado(s).");
                            Console.WriteLine("Deseja exibir as series e filmes para favoritar ?(Enter confirma)");

                            if (Console.ReadKey(true).Key == ConsoleKey.Enter) // Exibe series e fimes se houverem
                            {
                                Console.WriteLine("Sim");
                                Thread.Sleep(400);

                                Console.Clear();

                                if (catalogo.QtdFilmes() > 0)
                                {
                                    catalogo.exibirFilmes();
                                    Console.WriteLine("");
                                }
                                if (catalogo.QtdSeries() > 0)
                                {
                                    catalogo.exibirSeries();
                                    Console.WriteLine("");
                                }
                                if (usuario.favoritos.QtdFavoritos() > 0)
                                {
                                    usuario.favoritos.ExibirFavoritos();
                                    Console.WriteLine("");
                                }
                            }
                            else
                                Console.WriteLine("Não");

                            do
                            {
                                Console.Write("\nInforme o nome da midia a ser favoritada : ");
                                string MidiaNome = Console.ReadLine();
                                Console.WriteLine("");

                                Midia midiafavoritada = catalogo.BuscarPorNome(MidiaNome);

                                if (midiafavoritada != null)
                                {
                                    usuario.favoritos.AdicionarNaLista(midiafavoritada);

                                    if ((catalogo.QtdFilmes() + catalogo.QtdSeries()) <= usuario.favoritos.QtdFavoritos()) // Se todas a series foram favoritadas encerra o codigo
                                    {
                                        Console.WriteLine("Não há mais midias a serem favoritadas.");
                                        Console.ReadKey(true);
                                        break;
                                    }

                                    Console.WriteLine("\nAdicionar outra?(Enter confirma)");
                                }
                                else
                                {
                                    Console.WriteLine("Filme/Serie inexistênte.");
                                    Console.WriteLine("\nTentar novamente ?(Enter confirma)");
                                }
                            } while (Console.ReadKey(true).Key == ConsoleKey.Enter);
                        }
                        else // Todas a series foram favoritadas
                        {
                            Console.Clear();

                            Console.WriteLine($"\n {{{catalogo.QtdSeries() + catalogo.QtdFilmes()}}} midia(s) cadastrada(s) no catalogo,");
                            Console.WriteLine($"{{{usuario.favoritos.QtdFavoritos()}}} favoritado(s).");
                            Console.WriteLine("Não há series ou filmes a serem favoritadas.");

                            Console.WriteLine("Deseja exibir as series e filmes favoritados ?(Enter confirma)");

                            if (Console.ReadKey(true).Key == ConsoleKey.Enter) // Exibe series e fimes favoritados
                            {
                                usuario.favoritos.ExibirFavoritos();
                                Console.ReadKey(true);
                            }
                        }
                    else // Não existem series ou filmes
                    {
                        Console.WriteLine("\n Não há series ou filmes cadastrados.");
                        Console.ReadKey(true);
                    }
                    break;
                case 4: // Exibir (DONE)
                    if (catalogo.QtdMidias() > 0) // Se houver series ou filmes
                    {
                        int escolha;
                        bool Existem = false;
                        // Menu com opçoes dinamicas para exibição
                        do
                        {
                            Console.Clear();

                            Console.WriteLine("  Menu exibição");
                            Console.WriteLine("----------------------------");
                            Console.WriteLine(" 1 - Exibir filmes");
                            Console.WriteLine(" 2 - Exibir series");
                            if (catalogo.QtdFilmes() > 0 && catalogo.QtdSeries() > 0) // Existem series e filmes cadastrados
                            {
                                Existem = true;
                                Console.WriteLine(" 3 - Exibir todos");
                                Console.WriteLine(" 4 - Exibir favoritos");
                                Console.WriteLine(" 5 - voltar\n");
                            }
                            else // Não existem series e filmes cadastrados, logo menos 1 opçao
                            {
                                Console.WriteLine(" 3 - Exibir favoritos");
                                Console.WriteLine(" 4 - voltar\n");
                            }
                            escolha = int.Parse(Console.ReadLine());

                            if (escolha > 2 && !Existem) // Não existem series E filmes e foi escolhida uma opçao dinamica 
                                escolha++; // Aumenta 1 na opçao para enquadrar no if

                            if (escolha == 1)
                            {
                                if (catalogo.QtdFilmes() > 0)
                                {
                                    Console.Clear();
                                    catalogo.exibirFilmes();
                                }
                                else
                                    Console.WriteLine("Não há filmes cadastradas");

                                Console.ReadKey(true);
                            }
                            else if (escolha == 2)
                            {
                                if (catalogo.QtdSeries() > 0)
                                {
                                    Console.Clear();
                                    catalogo.exibirSeries();
                                }
                                else
                                    Console.WriteLine("Não há series cadastradas");

                                Console.ReadKey(true);
                            }
                            else if (escolha == 3 && Existem) // Exibe filmes e series se houver os dois
                            {
                                Console.Clear();
                                catalogo.exibirFilmes();
                                Console.WriteLine("");
                                catalogo.exibirSeries();
                                Console.ReadKey(true);
                            }
                            else if (escolha == 4)
                            {
                                if (usuario.favoritos.QtdFavoritos() > 0)
                                {
                                    Console.Clear();
                                    usuario.favoritos.ExibirFavoritos();
                                }
                                else
                                    Console.WriteLine("\n Não há series ou filmes favoritados.");

                                Console.ReadKey(true);
                            }
                            else if (escolha == 5)
                            {
                                Console.Write("\nVoltando.");
                                for (int i = 0; i < 3; i++)
                                {
                                    Thread.Sleep(200);
                                    Console.Write(".");
                                }
                            }
                            else
                            {
                                Console.WriteLine("\n OPÇÃO INVÁLIDA.");
                                Console.ReadKey(true);
                            }

                        } while (escolha != 5);
                    }
                    else // Não existem series ou filmes
                    {
                        Console.WriteLine("\n Não há series ou filmes cadastrados.");
                        Console.ReadKey(true);
                    }
                    break;
                case 5: // Remover (DONE?)
                    if (catalogo.QtdMidias() > 0) // Se houver series ou filmes
                    {
                        int escolha;

                        do
                        {
                            Console.Clear();

                            Console.WriteLine("\n  Menu remover");
                            Console.WriteLine("----------------------------");
                            Console.WriteLine(" 1 - Remover midia");
                            Console.WriteLine(" 2 - Remover todos os filmes");
                            Console.WriteLine(" 3 - Remover todas as series");
                            Console.WriteLine(" 4 - Remover todas midias");
                            Console.WriteLine(" 5 - Remover favorito");
                            Console.WriteLine(" 6 - Remover todos os favoritos");
                            Console.WriteLine(" 7 - voltar\n");
                            escolha = int.Parse(Console.ReadLine());

                            if (escolha == 1) // (DONE?)
                            {
                                if ((catalogo.QtdFilmes() + catalogo.QtdSeries()) > 0)
                                    do
                                    {
                                        Console.Clear();

                                        Console.Write("\nInforme o nome da midia a ser removida : ");
                                        string Midia = Console.ReadLine();

                                        Midia MidiaRemovida = catalogo.BuscarPorNome(Midia);

                                        if (MidiaRemovida != null)
                                        {
                                            Console.WriteLine("\nMidia Encontrada.");
                                            Console.Write("Removendo.");
                                            for (int i = 0; i < 3; i++)
                                            {
                                                Thread.Sleep(300);
                                                Console.Write(".");
                                            }

                                            if (catalogo.RemoveMidia(MidiaRemovida))
                                            {
                                                Console.WriteLine($"{MidiaRemovida} Removido(a) com sucesso.");
                                                Console.WriteLine("Remover outra?(Enter confima)");
                                            }
                                            else
                                                Console.WriteLine($"Erro ao remover {MidiaRemovida}.");
                                        }
                                        else
                                        {
                                            Console.WriteLine($"\n\"{Midia}\" não encontrada.");
                                            Console.WriteLine("Tentar novamente?(Enter confirma)");
                                        }
                                    } while (Console.ReadKey(true).Key == ConsoleKey.Enter);

                                else
                                {
                                    Console.WriteLine("Não há midias a remover.");
                                    Console.ReadKey(true);
                                }
                            }
                            else if (escolha == 2) // (DONE?)
                            {
                                if (catalogo.QtdFilmes() > 0)
                                {

                                    bool Repetir = false;
                                    do
                                    {
                                        Console.Clear();

                                        Console.WriteLine("\nRemover todos os filmes.");
                                        Console.WriteLine("Tem certeza? (digite SIM)");

                                        string confimaçao = Console.ReadLine().Trim().ToUpper();

                                        if (confimaçao == "SIM")
                                        {
                                            Console.Write("\nRemovendo todos os filmes.");
                                            for (int i = 0; i < 4; i++)
                                            {
                                                Thread.Sleep(400);
                                                Console.Write(".");
                                            }
                                            Console.WriteLine("");

                                            catalogo.RemoverFilmes();

                                            Console.WriteLine("Filmes removidos.");
                                            Console.ReadKey(true);

                                            Repetir = false;
                                        }
                                        else
                                        {
                                            Console.WriteLine("\nResposta inválida");
                                            Console.WriteLine("Tentar novamente ?(Enter confirma)");
                                            if (Console.ReadKey(true).Key == ConsoleKey.Enter)
                                            { Repetir = true; }
                                        }
                                    } while (Repetir);
                                }
                                else
                                {
                                    Console.WriteLine("Não há midias a remover.");
                                    Console.ReadKey(true);
                                }
                            }
                            else if (escolha == 3) // (DONE?)
                            {
                                if (catalogo.QtdSeries() > 0)
                                {
                                    bool Repetir = false;
                                    do
                                    {
                                        Console.Clear();

                                        Console.WriteLine("\nRemover todos as series.");
                                        Console.WriteLine("Tem certeza? (digite SIM)");

                                        string confimaçao = Console.ReadLine().Trim().ToUpper();

                                        if (confimaçao == "SIM")
                                        {
                                            Console.Write("\nRemovendo todas as series.");
                                            for (int i = 0; i < 4; i++)
                                            {
                                                Thread.Sleep(400);
                                                Console.Write(".");
                                            }
                                            Console.WriteLine("");

                                            catalogo.RemoverSeries();

                                            Console.WriteLine("Series removidas.");
                                            Console.ReadKey(true);

                                            Repetir = false;
                                        }
                                        else
                                        {
                                            Console.WriteLine("\nResposta inválida");
                                            Console.WriteLine("Tentar novamente ?(Enter confirma)");
                                            if (Console.ReadKey(true).Key == ConsoleKey.Enter)
                                            { Repetir = true; }
                                        }
                                    } while (Repetir);
                                }
                                else
                                {
                                    Console.WriteLine("Não há midias a remover.");
                                    Console.ReadKey(true);
                                }
                            }
                            else if (escolha == 4) // (DONE?)
                            {
                                if ((catalogo.QtdFilmes() + catalogo.QtdSeries()) > 0)
                                {
                                    bool Repetir = false;
                                    do
                                    {
                                        Console.Clear();

                                        Console.WriteLine("\nRemover todas as mídias.");
                                        Console.WriteLine("Tem certeza? (digite CONFIRMA)");

                                        string confimaçao = Console.ReadLine().Trim().ToUpper();

                                        if (confimaçao == "CONFIRMA")
                                        {
                                            Console.Write("\nRemovendo todas as Midias.");
                                            for (int i = 0; i < 4; i++)
                                            {
                                                Thread.Sleep(400);
                                                Console.Write(".");
                                            }
                                            Console.WriteLine("");

                                            catalogo.RemoverFilmes();
                                            catalogo.RemoverSeries();

                                            Console.WriteLine("Midias removidas.");
                                            Console.ReadKey(true);

                                            Repetir = false;
                                        }
                                        else
                                        {
                                            Console.WriteLine("\nResposta inválida");
                                            Console.WriteLine("Tentar novamente ?(Enter confirma)");
                                            if (Console.ReadKey(true).Key == ConsoleKey.Enter)
                                            { Repetir = true; }
                                        }
                                    } while (Repetir);
                                }
                                else
                                {
                                    Console.WriteLine("Não há midias a remover.");
                                    Console.ReadKey(true);
                                }
                            }
                            else if (escolha == 5) // (DONE?)
                            {
                                if (usuario.favoritos.QtdFavoritos() > 0)
                                    do
                                    {
                                        Console.Clear();

                                        Console.Write("\nInforme o nome da midia a ser removida dos favoritos: ");
                                        string Midia = Console.ReadLine();

                                        Midia FavoritoRemover = usuario.favoritos.BuscarPorNome(Midia);

                                        if (FavoritoRemover != null)
                                        {
                                            Console.WriteLine("\nMidia Encontrada.");
                                            Console.Write("Removendo.");
                                            for (int i = 0; i < 3; i++)
                                            {
                                                Thread.Sleep(300);
                                                Console.Write(".");
                                            }

                                            if (usuario.favoritos.RemoverFavorito(FavoritoRemover))
                                            {
                                                Console.WriteLine($"{FavoritoRemover} Removido(a) com sucesso.");
                                                Console.WriteLine("Remover outra?(Enter confima)");
                                            }
                                            else
                                                Console.WriteLine($"Erro ao remover {FavoritoRemover}.");
                                        }
                                        else
                                        {
                                            Console.WriteLine($"\n\"{Midia}\" não encontrada.");
                                            Console.WriteLine("Tentar novamente?(Enter confirma)");
                                        }
                                    } while (Console.ReadKey(true).Key == ConsoleKey.Enter);
                                else
                                {
                                    Console.WriteLine("Não há favoritos a remover.");
                                    Console.ReadKey(true);
                                }
                            }
                            else if (escolha == 6) // (DONE?)5
                            {
                                if (usuario.favoritos.QtdFavoritos() > 0)
                                {
                                    bool Repetir = false;
                                    do
                                    {
                                        Console.Clear();

                                        Console.WriteLine("\nRemover todos os favoritos.");
                                        Console.WriteLine("Tem certeza? (digite CONFIRMA)");

                                        string confimaçao = Console.ReadLine().Trim().ToUpper();

                                        if (confimaçao == "CONFIRMA")
                                        {
                                            Console.Write("\nRemovendo todas os favoritos.");
                                            for (int i = 0; i < 4; i++)
                                            {
                                                Thread.Sleep(400);
                                                Console.Write(".");
                                            }
                                            Console.WriteLine("");

                                            usuario.favoritos.RemoverFavoritos();

                                            Console.WriteLine("Favoritos removidos.");
                                            Console.ReadKey(true);

                                            Repetir = false;
                                        }
                                        else
                                        {
                                            Console.WriteLine("\nResposta inválida");
                                            Console.WriteLine("Tentar novamente ?(Enter confirma)");
                                            if (Console.ReadKey(true).Key == ConsoleKey.Enter)
                                            { Repetir = true; }
                                        }
                                    } while (Repetir);
                                }
                                else
                                {
                                    Console.WriteLine("Não há favoritos a remover.");
                                    Console.ReadKey(true);
                                }
                            }
                            else if (escolha == 7)
                            {
                                Console.Write("\nVoltando.");
                                for (int i = 0; i < 3; i++)
                                {
                                    Thread.Sleep(200);
                                    Console.Write(".");
                                }
                            }
                            else
                            {
                                Console.WriteLine("\n OPÇÃO INVÁLIDA.");
                                Console.ReadKey(true);
                            }

                        } while (escolha != 7);
                    }
                    else // Não existem series ou filmes
                    {
                        Console.WriteLine("\n Não há series ou filmes cadastrados.");
                        Console.ReadKey(true);
                    }
                    break;
                case 6: // Sair, animado (DONE)
                    Console.Write("\nFinalizando o programa.");
                    for (int i = 0; i < 4; i++)
                    {
                        Thread.Sleep(400);
                        Console.Write(".");
                    }
                    break;
                case 7: // Teste placeholder
                    catalogo.cadastrarFilme("a grande viagem", "comedia", "pedro", 1954, 154);
                    catalogo.cadastrarFilme("mochileiro", "suspense", "unknown", 2000, 210);
                    catalogo.cadastrarSerie("Doctor house", "Medica", "john albert", 1950, 5, 402, 40);
                    catalogo.cadastrarSerie("The flash", "ficçao", "Jefreey", 2011, 9, 345, 45);

                    catalogo.exibirFilmes();
                    catalogo.exibirSeries();

                    Console.WriteLine("Adicionando o primeiro filme e serie nos favoritos");

                    usuario.favoritos.AdicionarNaLista(catalogo.BuscarPorNome("a grande viagem"));
                    usuario.favoritos.AdicionarNaLista(catalogo.BuscarPorNome("Doctor house"));

                    usuario.favoritos.ExibirFavoritos();

                    Console.ReadKey(true);
                    break;
                default: // (DONE)
                    Console.WriteLine("\n Opção inválida!");
                    Console.ReadKey(true);
                    break;
            }
        } while (opçao != 6);
    }
    static public int Menu()
    {
        Console.Clear();

        Console.WriteLine("==============================");
        Console.WriteLine("  Catalogo Filmes e Series");
        Console.WriteLine("==============================");
        Console.WriteLine(" 1 - Cadastrar filme");
        Console.WriteLine(" 2 - Cadastrar serie");
        Console.WriteLine(" 3 - Favoritar");
        Console.WriteLine(" 4 - Exibir");
        Console.WriteLine(" 5 - Remover");
        Console.WriteLine(" 6 - Sair");
        Console.WriteLine(" 7 - Teste Placeholders\n"); // REMOVER

        return int.Parse(Console.ReadLine());
    }
}
class Favoritos
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

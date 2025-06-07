class Program
{
    static void Main(string[] args)
    {
        Menu menu = new Menu();

        menu.MenuFuncional();

    }
}
class Menu
{
    Catalogo catalogo = new Catalogo();
    Usuario usuario = new Usuario();
    public void MenuFuncional()
    {
        int opçao;

        do
        {
            opçao = MenuPrincipal();
            switch (opçao)
            {
                case 1: // Cadastrar filme (DONE)
                    do
                    {
                        catalogo.cadastrarFilme();
                        Console.WriteLine($" {{{catalogo.QtdFilmes()}}} filme(s) cadastrado(s) no catalogo.");
                        Console.WriteLine("\nDeseja cadastrar outro filme?(Enter para confirmar)");
                    } while (Console.ReadKey(true).Key == ConsoleKey.Enter);
                    break;
                case 2: // Cadastrar serie (DONE)
                    do
                    {
                        catalogo.cadastrarSerie();
                        Console.WriteLine($" {{{catalogo.QtdSeries()}}} serie(s) cadastrada(s) no catalogo.");
                        Console.WriteLine("\nDeseja cadastrar outra serie? (Enter para confirmar)");
                    } while (Console.ReadKey(true).Key == ConsoleKey.Enter);
                    break;
                case 3: // Favoritar (DONE)
                    if (catalogo.QtdFilmes() > 0 || catalogo.QtdSeries() > 0) // Se existirem series ou filmes
                        if (usuario.favoritos.QtdFavoritos() < (catalogo.QtdFilmes() + catalogo.QtdSeries())) // se ainda existirem series a serem favoritadas
                        {
                            Console.Clear();

                            Console.WriteLine($"\n {{{catalogo.QtdSeries() + catalogo.QtdFilmes()}}} midia(s) cadastrada(s) no catalogo,");
                            Console.WriteLine($"\n{{{usuario.favoritos.QtdFavoritos()}}} favoritado(s).");
                            Console.WriteLine("\nDeseja exibir as series e filmes para favoritar ? (Enter confirma)");

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

                                    Console.WriteLine("\nAdicionar outra? (Enter confirma)");
                                }
                                else
                                {
                                    Console.WriteLine("Filme/Serie inexistênte.");
                                    Console.WriteLine("\nTentar novamente ? (Enter confirma)");
                                }
                            } while (Console.ReadKey(true).Key == ConsoleKey.Enter);
                        }
                        else // Todas a series foram favoritadas
                        {
                            Console.Clear();

                            Console.WriteLine($"\n {{{catalogo.QtdSeries() + catalogo.QtdFilmes()}}} midia(s) cadastrada(s) no catalogo,");
                            Console.WriteLine($"\n{{{usuario.favoritos.QtdFavoritos()}}} favoritado(s).");
                            Console.WriteLine("\nNão há series ou filmes a serem favoritadas.");

                            Console.WriteLine("Deseja exibir as series e filmes favoritados ? (Enter confirma)");

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
                    if (catalogo.QtdFilmes() > 0 || catalogo.QtdSeries() > 0) // Se houver series ou filmes
                    {
                        int escolha;
                        bool Existem = false;
                        // Menu com opçoes dinamicas para exibição
                        do
                        {
                            Console.Clear();

                            Console.WriteLine("  Catalogo Filmes e Series");
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
                case 5: // Sair, animado (DONE)
                    Console.Write("\nFinalizando o programa.");
                    for (int i = 0; i < 4; i++)
                    {
                        Thread.Sleep(400);
                        Console.Write(".");
                    }
                    break;
                case 6: // Teste placeholder
                    catalogo.cadastrarFilme("a grande viagem", "comedia", "pedro", 1954, 154);
                    catalogo.cadastrarFilme("mochileiro", "suspense", "unknown", 2000, 210);
                    catalogo.cadastrarSerie("Doctor house", "Medica", "john albert", 1950, 5, 402, 40);
                    catalogo.cadastrarSerie("The flash", "ficçao", "Jefreey", 2011, 9, 345, 45);

                    catalogo.exibirFilmes();
                    catalogo.exibirSeries();

                    Console.Write("Adicionando o primeiro filme e serie nos favoritos");

                    usuario.favoritos.AdicionarNaLista(catalogo.BuscarPorNome("a grande viagem"));
                    usuario.favoritos.AdicionarNaLista(catalogo.BuscarPorNome("Doctor house"));
                    Console.WriteLine("Mídia adicionada aos favoritos!");

                    usuario.favoritos.ExibirFavoritos();

                    Console.ReadKey(true);
                    break;
                default: // (DONE)
                    Console.WriteLine("\n Opção inválida!");
                    Console.ReadKey(true);
                    break;
            }
        } while (opçao != 5);
    }
    public static int MenuPrincipal()
    {
        Console.Clear();

        Console.WriteLine("==============================");
        Console.WriteLine("  Catálogo de Filmes e Séries");
        Console.WriteLine("==============================");
        Console.WriteLine(" 1 - Cadastrar Filme");
        Console.WriteLine(" 2 - Cadastrar Série");
        Console.WriteLine(" 3 - Favoritar");
        Console.WriteLine(" 4 - Exibir");
        Console.WriteLine(" 5 - Sair");
        Console.Write("\nEscolha uma opção: ");

        if (int.TryParse(Console.ReadLine(), out int opcao))
            return opcao;
        else
            return -1;

    }
}

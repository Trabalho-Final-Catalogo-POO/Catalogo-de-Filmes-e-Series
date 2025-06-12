using System;
using System.Runtime.CompilerServices;
using MySql.Data.MySqlClient;

public class Menu
{
    Catalogo catalogo = new Catalogo();
    public int MenuPrincipal(Usuario usuario, OperaçoesUsuario usuarios)
    {
        int opçao;

        do
        {
            opçao = OpcoesMenuPrincipal(usuario._Nome);
            switch (opçao)
            {
                case 1: // Cadastrar filme
                    do
                    {
                        catalogo.cadastrarFilme();
                        Console.WriteLine($" {{{catalogo.QtdFilmes()}}} filme(s) cadastrado(s) no catalogo.");
                        Console.WriteLine("\nDeseja cadastrar outro filme?(Enter para confirmar)");
                    } while (Console.ReadKey(true).Key == ConsoleKey.Enter);
                    break;
                case 2: // Cadastrar serie
                    do
                    {
                        catalogo.cadastrarSerie();
                        Console.WriteLine($" {{{catalogo.QtdSeries()}}} serie(s) cadastrada(s) no catalogo.");
                        Console.WriteLine("\nDeseja cadastrar outra serie? (Enter para confirmar)");
                    } while (Console.ReadKey(true).Key == ConsoleKey.Enter);
                    break;
                case 3: // Favoritar
                    if (catalogo.QtdMidias() > 0) // Se existirem series ou filmes
                        if (usuario.QtdFavoritos() < (catalogo.QtdMidias())) // se ainda existirem series a serem favoritadas
                        {
                            Console.Clear();

                            Console.WriteLine($"\n{{{catalogo.QtdSeries() + catalogo.QtdFilmes()}}} midia(s) cadastrada(s) no catalogo,");
                            Console.WriteLine($"\n{{{usuario.QtdFavoritos()}}} favoritado(s).");
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
                                if (usuario.QtdFavoritos() > 0)
                                {
                                    usuario.ExibirFavoritos();
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
                                    usuario.AdicionarFavorito(midiafavoritada);

                                    if ((catalogo.QtdMidias()) <= usuario.QtdFavoritos()) // Se todas a series foram favoritadas encerra o codigo
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

                            Console.WriteLine($"\n{{{catalogo.QtdSeries() + catalogo.QtdFilmes()}}} midia(s) cadastrada(s) no catalogo,");
                            Console.WriteLine($"\n{{{usuario.QtdFavoritos()}}} favoritado(s).");
                            Console.WriteLine("\nNão há series ou filmes a serem favoritadas.");

                            Console.WriteLine("Deseja exibir as series e filmes favoritados ? (Enter confirma)");

                            if (Console.ReadKey(true).Key == ConsoleKey.Enter) // Exibe series e fimes favoritados
                            {
                                usuario.ExibirFavoritos();
                                Console.ReadKey(true);
                            }
                        }
                    else // Não existem series ou filmes
                    {
                        Console.WriteLine("\n Não há series ou filmes cadastrados.");
                        Console.ReadKey(true);
                    }
                    break;
                case 4: // Exibir
                    if (catalogo.QtdMidias() > 0) // Se houver series ou filmes
                    {
                        int escolha;
                        bool Existem = false;
                        // Menu com opçoes dinamicas para exibição
                        do
                        {
                            Console.Clear();

                            Console.WriteLine("  Menu Exibir");
                            Console.WriteLine("----------------------------");
                            Console.WriteLine($" 1 - Exibir filmes {{{catalogo.QtdFilmes()}}}");
                            Console.WriteLine($" 2 - Exibir series {{{catalogo.QtdSeries()}}}");
                            if (catalogo.QtdFilmes() > 0 && catalogo.QtdSeries() > 0) // Existem series e filmes cadastrados
                            {
                                Existem = true;
                                Console.WriteLine($" 3 - Exibir todos {{{catalogo.QtdMidias()}}}");
                                Console.WriteLine($" 4 - Exibir favoritos {{{usuario.QtdFavoritos()}}}");
                                Console.WriteLine(" 5 - voltar\n");
                            }
                            else // Não existem series e filmes cadastrados, logo menos 1 opçao
                            {
                                Console.WriteLine($" 3 - Exibir favoritos {{{usuario.QtdFavoritos()}}}");
                                Console.WriteLine(" 4 - voltar\n");
                            }
                            escolha = int.Parse(Console.ReadLine());

                            if (escolha > 2 && !Existem) // Não existem series E filmes e foi escolhida uma opçao dinamica 
                                escolha++; // Aumenta 1 na opçao no if para a funcionalidade do menu

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
                                if (usuario.QtdFavoritos() > 0)
                                {
                                    Console.Clear();
                                    usuario.ExibirFavoritos();
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
                case 5: // Remover
                    if (catalogo.QtdMidias() > 0) // Se houver series ou filmes
                    {
                        int escolha;

                        do
                        {
                            Console.Clear();

                            Console.WriteLine("\n  Menu Remover");
                            Console.WriteLine("----------------------------");
                            Console.WriteLine(" 1 - Remover midia");
                            Console.WriteLine(" 2 - Remover todos os filmes");
                            Console.WriteLine(" 3 - Remover todas as series");
                            Console.WriteLine(" 4 - Remover todas midias");
                            Console.WriteLine(" 5 - Remover favorito");
                            Console.WriteLine(" 6 - Remover todos os favoritos");
                            Console.WriteLine(" 7 - voltar\n");
                            escolha = int.Parse(Console.ReadLine());

                            if (escolha == 1) // Remove midia especifica
                            {
                                if ((catalogo.QtdMidias()) > 0)
                                    do
                                    {
                                        Console.Clear();

                                        Console.Write("\nInforme o nome da midia a ser removida : ");
                                        string Midia = Console.ReadLine();

                                        Midia MidiaRemovida = catalogo.BuscarPorNome(Midia);

                                        if (MidiaRemovida != null)
                                        {
                                            Console.WriteLine("\nMidia Encontrada.");
                                            MensagemAnimada("Removendo");

                                            if (catalogo.RemoveMidia(MidiaRemovida, usuarios))
                                            {
                                                Console.WriteLine($"\n{MidiaRemovida}\nRemovido(a) com sucesso.");
                                                Console.WriteLine("\nRemover outra?(Enter confima)");
                                            }
                                            else
                                                Console.WriteLine($"Erro ao remover {MidiaRemovida}.");
                                        }
                                        else
                                        {
                                            Console.WriteLine("");
                                            Console.WriteLine(Midia == null ? "Midia com nome vazio!" : $"\"{Midia}\" não encontrada.");
                                            Console.WriteLine("Tentar novamente?(Enter confirma)");
                                        }
                                    } while (Console.ReadKey(true).Key == ConsoleKey.Enter);

                                else
                                {
                                    Console.WriteLine("Não há midias a remover.");
                                    Console.ReadKey(true);
                                }
                            }
                            else if (escolha == 2) // Remove todos os filmes 
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

                                            catalogo.RemoverFilmes(usuarios);

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
                            else if (escolha == 3) // Remove todas as series
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

                                            catalogo.RemoverSeries(usuarios);

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
                            else if (escolha == 4) // Remove Todas as midas
                            {
                                if ((catalogo.QtdMidias()) > 0)
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

                                            catalogo.RemoverFilmes(usuarios);
                                            catalogo.RemoverSeries(usuarios);

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
                            else if (escolha == 5) // Remove favorito
                            {
                                if (usuario.QtdFavoritos() > 0)
                                    do
                                    {
                                        Console.Clear();

                                        Console.Write("\nInforme o nome da midia a ser removida dos favoritos: ");
                                        string Midia = Console.ReadLine();

                                        Midia FavoritoRemover = usuario.BuscarFavoritoPorNome(Midia);

                                        if (FavoritoRemover != null)
                                        {
                                            Console.WriteLine("\nMidia Encontrada.");
                                            Console.Write("Removendo.");
                                            for (int i = 0; i < 3; i++)
                                            {
                                                Thread.Sleep(300);
                                                Console.Write(".");
                                            }

                                            if (usuario.RemoverFavorito(FavoritoRemover))
                                            {
                                                Console.WriteLine($"\n{FavoritoRemover} \nRemovido(a) com sucesso.");
                                                Console.WriteLine("\nRemover outra?(Enter confima)");
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
                            else if (escolha == 6) // Remove todos favoritos
                            {
                                if (usuario.QtdFavoritos() > 0)
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

                                            usuario.RemoverFavoritos();

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
                            else if (escolha == 7) // Voltar
                            {
                                MensagemAnimada("\nVoltando.", 3, 200);
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
                case 6: // Saindo do usuário
                    MensagemAnimada("\nVoltando ao menu de login.");
                    break;
                case 7: // Sair
                    MensagemAnimada("\nFinalizando programa.");

                    return 1;
                case 8: // Teste placeholder
                    catalogo.cadastrarFilme("a grande viagem", "comedia", "pedro", 1954, 154);
                    catalogo.cadastrarFilme("mochileiro", "suspense", "unknown", 2000, 210);
                    catalogo.cadastrarSerie("Doctor house", "Medica", 1950, 5, 402, 40);
                    catalogo.cadastrarSerie("The flash", "ficçao", 2011, 9, 345, 45);

                    catalogo.exibirFilmes();
                    catalogo.exibirSeries();

                    Console.Write("Adicionando o primeiro filme e serie nos favoritos");

                    usuario.AdicionarFavorito(catalogo.BuscarPorNome("a grande viagem"));
                    usuario.AdicionarFavorito(catalogo.BuscarPorNome("Doctor house"));
                    Console.WriteLine("Mídia adicionada aos favoritos!");

                    usuario.ExibirFavoritos();

                    BancoDeDados BD = new();

                    BD.SELECT("midias", "nome");

                    Console.ReadKey(true);
                    break;
                default:
                    Console.WriteLine("\n Opção inválida!");
                    Console.ReadKey(true);
                    break;
            }
        } while (opçao != 6 && opçao != 7);

        return 0; // Menu executado normalmente e finalizado pela opção 6
    }
    public int OpcoesMenuPrincipal(string Usuario)
    {
        Console.Clear();

        Console.WriteLine("==============================");
        Console.WriteLine("  Catálogo de Filmes e Séries");
        Console.WriteLine("==============================");
        Console.WriteLine($"Usuário : {Usuario}\n");
        Console.WriteLine(" 1 - Cadastrar Filme");
        Console.WriteLine(" 2 - Cadastrar Série");
        Console.WriteLine(" 3 - Favoritar");
        Console.WriteLine(" 4 - Exibir");
        Console.WriteLine(" 5 - Remover");
        Console.WriteLine(" 6 - Trocar de usuário");
        Console.WriteLine(" 7 - Finalizar programa");
        Console.Write("\nEscolha uma opção: ");

        if (int.TryParse(Console.ReadLine(), out int opcao))
            return opcao;
        else
            return -1;
    }
    public void MenuLogin()
    {
        OperaçoesUsuario Usuarios = new OperaçoesUsuario();
        int Opcao;
        int flagMenu = 0;
        do
        {
            Opcao = OpcoesMenuLogin();

            switch (Opcao)
            {
                case 1: // Fazer login
                    if (Usuarios.QtdUsuarios() > 0)
                    {
                        string Login = Usuarios.Login();

                        if (Login != null)
                        {
                            Usuario usuario = Usuarios.BuscarUsuario(Login);
                            flagMenu = MenuPrincipal(usuario, Usuarios);
                        }
                        if (flagMenu == 1)
                            Opcao = 3;
                    }
                    else
                    {
                        Console.WriteLine("Não há usuários cadastrados.");
                        Console.ReadKey(true);
                    }
                    break;
                case 2: // Fazer cadastro
                    if (Usuarios.CadastrarUsuario())
                        Console.WriteLine("\nCadastro realizado com sucesso.");

                    else
                        Console.WriteLine("\nCadastramento cancelado.");

                    Console.WriteLine("(Pressione qualquer botão para retornar)");
                    Console.ReadKey(true);

                    break;
                case 3: // Finalizar programa
                    Console.Write("\nFinalizando o programa.");
                    for (int i = 0; i < 4; i++)
                    {
                        Thread.Sleep(400);
                        Console.Write(".");
                    }
                    break;
                case 4: // REMOVER
                    foreach (Usuario x in Usuarios.Usuarios)
                        Console.WriteLine(x._Nome);
                    Console.ReadKey(true);

                    break;
                default:
                    Console.WriteLine("Opção inválida.");
                    Console.ReadKey(true);
                    break;
            }
        } while (Opcao != 3);
    }
    public int OpcoesMenuLogin()
    {
        Console.Clear();

        Console.WriteLine("==============================");
        Console.WriteLine("  Catálogo de Filmes e Séries");
        Console.WriteLine("==============================");
        Console.WriteLine(" 1 - Login");
        Console.WriteLine(" 2 - Cadastrar");
        Console.WriteLine(" 3 - Sair");
        Console.WriteLine(" 4 - TesteListarUsuarios");
        Console.Write("\nEscolha uma opção: ");

        return int.Parse(Console.ReadLine());
    }
    public void MensagemAnimada(string Mensagem, int pontos = 4, int tempo = 400)
    {
        Console.Write(Mensagem);
        for (int i = 0; i < pontos; i++)
        {
            Thread.Sleep(tempo);
            Console.Write(".");
        }
        Console.WriteLine("");
    }
}
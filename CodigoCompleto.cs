using System;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;

class Program
{
    static void Main(string[] args)
    {
        Menu menu = new Menu();
        OperaçoesUsuario Usuarios = new OperaçoesUsuario();
        int Opcao;
        int flagMenu = 0;
        do
        {
            Opcao = menu.MenuInicial();

            switch (Opcao)
            {
                case 1: // Fazer login
                    if (Usuarios.QtdUsuarios() > 0)
                    {
                        string Login = Usuarios.Login();

                        if (Login != null)
                        {
                            Usuario usuario = Usuarios.BuscarUsuario(Login);
                            flagMenu = menu.MenuPrincipal(usuario);
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
                    if (Usuarios.Cadastrar() != null)
                        Console.WriteLine("\nCadastro realizado com sucesso.");

                    else
                        Console.WriteLine("\nCadastramento cancelado.");

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
                default:
                    Console.WriteLine("Opção inválida.");
                    Console.ReadKey(true);
                    break;
            }
        } while (Opcao != 3);
    }
}
public class Menu
{
    Catalogo catalogo = new Catalogo();
    public int MenuPrincipal(Usuario usuario)
    {
        int opçao;

        do
        {
            opçao = MenuOpcoes(usuario._Nome);
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
                        if (usuario.favoritos.QtdFavoritos() < (catalogo.QtdMidias())) // se ainda existirem series a serem favoritadas
                        {
                            Console.Clear();

                            Console.WriteLine($"\n{{{catalogo.QtdSeries() + catalogo.QtdFilmes()}}} midia(s) cadastrada(s) no catalogo,");
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

                                    if ((catalogo.QtdMidias()) <= usuario.favoritos.QtdFavoritos()) // Se todas a series foram favoritadas encerra o codigo
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
                case 4: // Exibir
                    if (catalogo.QtdMidias() > 0) // Se houver series ou filmes
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
                case 5: // Remover
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
                            else if (escolha == 5) // Remove favorito
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
                            else if (escolha == 6) // Remove todos favoritos
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
                            else if (escolha == 7) // Voltar
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
                case 6: // Saindo do usuário
                    Console.Write("\nVoltando ao menu de login.");
                    for (int i = 0; i < 4; i++)
                    {
                        Thread.Sleep(400);
                        Console.Write(".");
                    }
                    break;
                case 7: // Sair
                    Console.Write("\nFinalizando programa.");
                    for (int i = 0; i < 4; i++)
                    {
                        Thread.Sleep(400);
                        Console.Write(".");
                    }
                    return 1;
                case 8: // Teste placeholder
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
                default:
                    Console.WriteLine("\n Opção inválida!");
                    Console.ReadKey(true);
                    break;
            }
        } while (opçao != 6 && opçao != 7);

        return 0; // Menu executado normalmente e finalizado pela opção 6
    }
    public int MenuOpcoes(string Usuario)
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
    public int MenuInicial()
    {
        Console.Clear();

        Console.WriteLine("==============================");
        Console.WriteLine("  Catálogo de Filmes e Séries");
        Console.WriteLine("==============================");
        Console.WriteLine(" 1 - Login");
        Console.WriteLine(" 2 - Cadastrar");
        Console.WriteLine(" 3 - Sair");
        Console.Write("\nEscolha uma opção: ");

        return int.Parse(Console.ReadLine());
    }
}
public class Catalogo
{
    private List<Filme> filmes = new List<Filme>();
    private List<Serie> series = new List<Serie>();

    public void cadastrarFilme()
    {
        Console.Clear();
        string nome, genero, diretor;
        int anoLancamento;
        double duracao;
        bool FilmeNovo = true;

        Console.WriteLine("\n   Cadastrar Filme");
        Console.WriteLine("-----------------------------------");
        Console.WriteLine("Digite os dados do filme.");

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

        Filme novo = new Filme(nome, genero, anoLancamento, duracao, diretor);

        foreach (Filme f in filmes) // Procura na lista de filmes 
            if (f.Nome == novo.Nome)
                FilmeNovo = false;
        foreach (Serie s in series) // Procura na lista de series 
            if (s.Nome == novo.Nome)
                FilmeNovo = false;

        if (FilmeNovo)
        {
            filmes.Add(novo);
            Console.WriteLine("\nFilme cadastrado com sucesso!");
            Console.WriteLine("-----------------------------------\n");
        }
        else
            Console.WriteLine("\nFilme ou Serie já cadastrado com mesmo nome.\n");
    }
    public void cadastrarFilme(string nome, string genero, string diretor, int anoLancamento, double duracao)
    {
        bool FilmeNovo = true;
        Filme novo = new Filme(nome, genero, anoLancamento, duracao, diretor);

        foreach (Filme f in filmes) // Procura na lista de filmes 
            if (f.Nome == novo.Nome)
                FilmeNovo = false;

        foreach (Serie s in series) // Procura na lista de series 
            if (s.Nome == novo.Nome)
                FilmeNovo = false;

        if (FilmeNovo)
        {
            filmes.Add(novo);
            Console.WriteLine("\nFilme cadastrado com sucesso!");
        }
        else
            Console.WriteLine("\nFilme ou Serie já cadastrado com mesmo nome.\n");
    }

    public void cadastrarSerie()
    {
        Console.Clear();
        string nome, genero;
        int anoLancamento, temporadas, qntEpisodios;
        double duracao;
        bool SerieNova = true;

        Console.WriteLine("\n   Cadastrar Série: ");
        Console.WriteLine("-----------------------------------");
        Console.WriteLine("Digite os dados da série.");

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

        Serie nova = new Serie(nome, genero, anoLancamento, duracao, temporadas, qntEpisodios);

        foreach (Serie s in series) // Procura na lista de series 
            if (s.Nome == nova.Nome)
                SerieNova = false;

        foreach (Filme f in filmes) // Procura na lista de filmes 
            if (f.Nome == nova.Nome)
                SerieNova = false;

        if (SerieNova)
        {
            series.Add(nova);
            Console.WriteLine("\nSérie cadastrada com sucesso!");
            Console.WriteLine("-----------------------------------\n");
        }
        else
            Console.WriteLine("\nFilme ou Serie já cadastrado com mesmo nome.\n");

    }
    public void cadastrarSerie(string nome, string genero, string diretor, int anoLancamento, int temporadas, int qntEpisodios, double duracao)
    {
        bool SerieNova = true;
        Serie nova = new Serie(nome, genero, anoLancamento, duracao, temporadas, qntEpisodios);

        foreach (Serie s in series) // Procura na lista de series 
            if (s.Nome == nova.Nome)
                SerieNova = false;

        foreach (Filme f in filmes) // Procura na lista de filmes 
            if (f.Nome == nova.Nome)
                SerieNova = false;

        if (SerieNova)
        {
            series.Add(nova);
            Console.WriteLine("\nSérie cadastrada com sucesso!");
        }
        else
            Console.WriteLine("\nFilme ou Serie já cadastrado com mesmo nome.\n");
    }

    public void exibirFilmes()
    {
        Console.WriteLine("================ FILMES ================\n");
        foreach (Filme f in filmes)
            Console.WriteLine(f);

        Console.WriteLine();
    }

    public void exibirSeries()
    {
        Console.WriteLine("================ SÉRIES ================\n");
        foreach (Serie s in series)
            Console.WriteLine(s);

        Console.WriteLine();
    }

    public Midia BuscarPorNome(string nome)
    {
        foreach (Filme f in filmes)
            if (f.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase))
                return f;

        foreach (Serie s in series)
            if (s.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase))
                return s;

        return null;
    }

    public int QtdFilmes() { return filmes.Count; }

    public int QtdSeries() { return series.Count; }
    public int QtdMidias() { return QtdFilmes() + QtdSeries(); }

    public bool RemoveMidia(Midia midia)
    {
        List<Midia> Midias = new();
        Midias.AddRange(filmes);
        Midias.AddRange(series);

        foreach (Midia m in Midias)
            if (midia == m)
            {
                Midias.Remove(midia);
                return true;
            }

        return false;
    }
    public void RemoverFilmes() { filmes.Clear(); }
    public void RemoverSeries() { series.Clear(); }
}
public class Midia
{
    public BancoDeDados BancoDeDados { get; set; }
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
public class Filme : Midia
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
public class Serie : Midia
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
public class Usuario
{
    private string Nome;
    public string _Nome
    {
        get { return Nome; }
        set // Impede a modificação de um usuário existente sem um metódo
        {
            if (Nome == null)
                Nome = value;
        }
    }
    private string Senha;
    public string _Senha
    {
        get { return Senha; }
        set // Impede a modificação de uma senha existente sem um metódo
        {
            if (Senha == null)
                Senha = value;
        }
    }
    public Favoritos favoritos { get; private set; } = new Favoritos();

    public Usuario() { }
    public Usuario(string Nome, string Senha)
    {
        this.Senha = Senha;
        this.Nome = Nome;
    }
    public bool VerificaNome(string Nome, List<Usuario> Usuarios) // Impede que o nome do usuário esteja vazio ou que se repita
    {
        if (Nome == "")
        {
            Console.WriteLine("\nO usuário não pode ficar vazio.");
            return false;
        }
        foreach (Usuario x in Usuarios)// Verifica se o usuário existe na cadastro
            if (Nome.ToUpper() == x._Nome.ToUpper())
            {
                Console.WriteLine("\nUsuário já existente.");

                return false;
            }

        return true;
    }
    public void NovoNome(string Nome)
    {
        this.Nome = Nome;
    }
    public bool VerificaSenha(string Senha)
    {
        if (string.IsNullOrWhiteSpace(Senha))
            Console.WriteLine("A senha não pode ser nula");
        else if (Senha.Length < 6)
            Console.WriteLine("Senha muito curta");
        else
            return true;

        return false;
    }
    public void AtualizarSenha(string Senha) { this.Senha = Senha; }
}
public class OperaçoesUsuario
{
    List<Usuario> Usuarios = new List<Usuario>();

    public string Cadastrar()
    {
        Usuario Novo = new();
        bool sts;
        do // Loop inserção de usuário
        {
            Console.Clear();

            Console.WriteLine("Cadastro de usuário");
            Console.WriteLine("=====================");
            Console.WriteLine("Digite as informações.\n");
            Console.Write("Nome : ");
            string nome = Console.ReadLine();

            sts = Novo.VerificaNome(nome, Usuarios);

            if (sts)
            {
                Novo._Nome = nome;
                break;
            }

            Console.WriteLine("\nTentar novamente? (Enter confirma)");
        } while (Console.ReadKey(true).Key == ConsoleKey.Enter);

        if (sts)
        {
            do // Loop inserção de senha
            {
                Console.Clear();

                Console.WriteLine("Cadastro de usuário");
                Console.WriteLine("=====================");
                Console.WriteLine("Digite as informações.\n");
                Console.WriteLine($"Nome : {Novo._Nome}");
                Console.Write("Senha: ");
                string senha = Console.ReadLine();

                if (Novo.VerificaSenha(senha))
                {
                    Novo._Senha = senha;
                    Usuarios.Add(Novo);
                    return Novo._Nome;
                }
                else
                    Console.WriteLine("\nTentar novamente? (Enter confirma)");

            } while (Console.ReadKey(true).Key == ConsoleKey.Enter);
        }

        return null;
    }

    public string Login()
    {
        do // Loop 
        {
            Console.Clear();

            Console.WriteLine("Login de usuário");
            Console.WriteLine("=====================");
            Console.WriteLine("Digite as informações.\n");
            Console.Write("Nome : ");
            string nome = Console.ReadLine();

            Usuario UsuarioLogin = BuscarUsuario(nome);
            // Verifica se o usuário existe na cadastro

            if (UsuarioLogin != null)
            {
                Console.WriteLine("Usuário encontrado.");
                Thread.Sleep(400);

                do // Loop 
                {
                    Console.Clear();

                    Console.WriteLine("Login de usuário");
                    Console.WriteLine("=====================");
                    Console.WriteLine("Digite as informações.\n");
                    Console.WriteLine($"Nome : {UsuarioLogin._Nome}");
                    Console.Write("Senha: ");
                    string senha = Console.ReadLine();

                    if (UsuarioLogin._Senha == senha)
                    {
                        Console.WriteLine("Login realizado com sucesso.");
                        Thread.Sleep(400);

                        return UsuarioLogin._Nome; // Login realizado com sucesso retorna o nome do usuario
                    }
                    else
                    {
                        Console.WriteLine("Senha incorreta");
                        Console.WriteLine("\nTentar novamente? (Enter confirma)");
                    }
                } while (Console.ReadKey(true).Key == ConsoleKey.Enter);

                break; // Operçao cancelada sair do método login
            }
            else
            {
                Console.WriteLine("Usuário inexistênte");
                Console.WriteLine("\nTentar novamente? (Enter confirma)");
            }
        } while (Console.ReadKey(true).Key == ConsoleKey.Enter);

        Console.WriteLine("\nOperação cancelada.");
        return null;
    }

    public Usuario BuscarUsuario(string Nome)
    {
        foreach (Usuario x in Usuarios)
            if (Nome == x._Nome)
                return x;

        return null;
    }
    public int QtdUsuarios() { return Usuarios.Count; }
}
public class BancoDeDados
{
    public MySqlConnection Conexao;
    public BancoDeDados()
    {
        try
        {
            Conexao = new("server=localhost;port=3306;database=Catalogo_Filmes_Series;user=root;password=Root190406@;Charset=utf8mb4;");
        }
        catch (Exception)
        {
            Console.WriteLine("falha na conexão com  o bando de dados");
        }
    }
    public void Iserir(string TABLE, string COLUMN, string VALUE)
    {// Modificar o insert 
        Conexao.Open();
        using MySqlCommand Comando = new($"INSERT INTO {TABLE} ({COLUMN}) VALUES ({VALUE})", Conexao);
        Conexao.Close();
    }
    public void funçao()
    {
        Conexao.Open();
        Console.WriteLine("✅ Conectado ao MySQL!");

        // 2. Inserir uma mídia
        string inserirQuery = "INSERT INTO Midias (nome, tipo) VALUES (@nome, @tipo)";
        using (MySqlCommand comando = new MySqlCommand(inserirQuery, conexao))
        {
            comando.Parameters.AddWithValue("@nome", "A Grande Viagem");
            comando.Parameters.AddWithValue("@tipo", "Filme");
            comando.ExecuteNonQuery();
            Console.WriteLine("🎬 Mídia inserida com sucesso.");
        }

        // 3. Ler as mídias
        string selecionarQuery = "SELECT * FROM Midias";
        using (MySqlCommand comando = new MySqlCommand(selecionarQuery, conexao))
        using (MySqlDataReader leitor = comando.ExecuteReader())
        {
            Console.WriteLine("\n📄 Mídias cadastradas:");
            while (leitor.Read())
            {
                int id = leitor.GetInt32("id");
                string nome = leitor.GetString("nome");
                string tipo = leitor.GetString("tipo");

                Console.WriteLine($" - ID {id}: {nome} ({tipo})");
            }
        }

        conexao.Close();
    }

}

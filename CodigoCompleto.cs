using System;
using System.Runtime.CompilerServices;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;

class Program
{
    static void Main(string[] args)
    {
        Menu menu = new Menu();
        menu.MenuLogin();
    }
}
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
public class Catalogo
{
    BancoDeDados bancoDeDados = new();
    private List<Filme> filmes = new List<Filme>();
    private List<Serie> series = new List<Serie>();
    public Catalogo()
    {
        CarregarMidiasBD();
    }
    private void CarregarMidiasBD()
    {
        if (bancoDeDados.Conexao.State != System.Data.ConnectionState.Open)
            bancoDeDados.Conexao.Open();

        string query = @"SELECT  m.id, m.nome, m.genero, m.tipo, m.anoLancamento,
                f.duracao AS filme_duracao, f.diretor,
                s.duracao AS serie_duracao, s.temporadas, s.qntEpisodios
                    FROM Midias m
                    LEFT JOIN Filmes f ON m.id = f.MidiaId
                    LEFT JOIN Series s ON m.id = s.MidiaId";

        using MySqlCommand Comando = new(query, bancoDeDados.Conexao);
        using MySqlDataReader Leitor = Comando.ExecuteReader();

        while (Leitor.Read())
        {
            string nome = Leitor.GetString("Nome");
            string genero = Leitor.GetString("Genero");
            int anolancamento = Leitor.GetInt32("AnoLancamento");
            string tipo = Leitor.GetString("tipo");
            int Id = Leitor.GetInt32("id");

            if (tipo.ToLower() == "filme")
            {
                double duracao = Leitor.GetDouble("filme_duracao");
                string diretor = Leitor.GetString("diretor");

                Filme novo = new(nome, genero, anolancamento, Id, duracao, diretor);
                filmes.Add(novo);
            }
            else if (tipo.ToLower() == "serie")
            {
                double duracao = Leitor.GetDouble("serie_duracao");
                int QtdTemporadas = Leitor.GetInt32("temporadas");
                int QtdEpisodios = Leitor.GetInt32("QntEpisodios");

                Serie nova = new(nome, genero, anolancamento, Id, duracao, QtdTemporadas, QtdEpisodios);
                series.Add(nova);
            }
        }
        bancoDeDados.Conexao.Close();
    }

    // Manipulação de Filmes
    public void cadastrarFilme()
    {
        Console.Clear();
        string nome, genero, diretor;
        int anoLancamento;
        double duracao;

        Console.WriteLine("\n   Cadastrar Filme");
        Console.WriteLine("-----------------------------------");
        Console.WriteLine("Digite os dados do filme.");

        Console.Write("\nNome: ");
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

        if (BuscarPorNome(novo.Nome) == null)
        {
            BDCadastrarFilme(novo);
            filmes.Add(novo);
            Console.WriteLine("\nFilme cadastrado com sucesso!");
            Console.WriteLine("-----------------------------------\n");
        }
        else
            Console.WriteLine("\nFilme ou Serie já cadastrado com mesmo nome.\n");
    }
    public void cadastrarFilme(string nome, string genero, string diretor, int anoLancamento, double duracao)
    {
        Filme novo = new Filme(nome, genero, anoLancamento, duracao, diretor);

        if (BuscarPorNome(novo.Nome) == null)
        {
            BDCadastrarFilme(novo);
            filmes.Add(novo);
            Console.WriteLine("\nFilme cadastrado com sucesso!");
        }
        else
            Console.WriteLine("\nFilme ou Serie já cadastrado com mesmo nome.\n");
    }
    private void BDCadastrarFilme(Filme filme)
    {
        if (bancoDeDados.Conexao.State != System.Data.ConnectionState.Open)
            bancoDeDados.Conexao.Open();

        // Insere a parte midia do filme na tabela midias
        using MySqlCommand ComandoMidia = new($"INSERT INTO Midias (nome,genero,tipo,anolancamento) values (@nome,@genero,'filme',@anoLancamento)", bancoDeDados.Conexao);
        ComandoMidia.Parameters.AddWithValue("@nome", filme.Nome);
        ComandoMidia.Parameters.AddWithValue("@genero", filme.Genero);
        ComandoMidia.Parameters.AddWithValue("@anoLancamento", filme.AnoLancamento);
        ComandoMidia.ExecuteNonQuery();

        long MidiaId = ComandoMidia.LastInsertedId; // id da midia para inserir os dados na tabela filmes
        filme.Id = MidiaId;

        // Insere o filme na tabela filmes
        using MySqlCommand ComandoFilme = new($"INSERT INTO Filmes (MidiaId,Duracao,Diretor) values (@midiaId,@duracao,@diretor)", bancoDeDados.Conexao);
        ComandoFilme.Parameters.AddWithValue("@midiaId", filme.Id);
        ComandoFilme.Parameters.AddWithValue("@duracao", filme.Duracao);
        ComandoFilme.Parameters.AddWithValue("@diretor", filme.Diretor);
        ComandoFilme.ExecuteNonQuery();

        bancoDeDados.Conexao.Close();
    }
    public void exibirFilmes()
    {
        Console.WriteLine("================ FILMES ================\n");
        foreach (Filme f in filmes)
            Console.WriteLine(f);

        Console.WriteLine();
    }
    public void RemoverFilmes(OperaçoesUsuario operaçoes)
    {
        if (bancoDeDados.Conexao.State != System.Data.ConnectionState.Open)
            bancoDeDados.Conexao.Open();

        using MySqlCommand Comando = new("Delete FROM Midias WHERE tipo='filme'", bancoDeDados.Conexao);
        Comando.ExecuteNonQuery();
        bancoDeDados.Conexao.Close();

        operaçoes.AtualizarUsuarios();

        filmes.Clear();
    }

    // Manipulação de Series
    public void cadastrarSerie()
    {
        Console.Clear();
        string nome, genero;
        int anoLancamento, temporadas, qntEpisodios;
        double duracao;

        Console.WriteLine("\n   Cadastrar Série: ");
        Console.WriteLine("-----------------------------------");
        Console.WriteLine("Digite os dados da série.");

        Console.Write("\nNome: ");
        nome = Console.ReadLine();

        Console.Write("Gênero: ");
        genero = Console.ReadLine();

        Console.Write("Ano de lançamento: ");
        anoLancamento = int.Parse(Console.ReadLine());

        Console.Write("Tempo médio de episódio (em minutos): ");
        duracao = double.Parse(Console.ReadLine());

        Console.Write("Quantidade de Temporadas: ");
        temporadas = int.Parse(Console.ReadLine());

        Console.Write("Quantidade de Episódios (Total): ");
        qntEpisodios = int.Parse(Console.ReadLine());

        Serie nova = new Serie(nome, genero, anoLancamento, duracao, temporadas, qntEpisodios);

        if (BuscarPorNome(nova.Nome) == null)
        {
            BDCadastrarSerie(nova);
            series.Add(nova);
            Console.WriteLine("\nSérie cadastrada com sucesso!");
            Console.WriteLine("-----------------------------------\n");
        }
        else
            Console.WriteLine("\nFilme ou Serie já cadastrado com mesmo nome.\n");
    }
    public void cadastrarSerie(string nome, string genero, int anoLancamento, int temporadas, int qntEpisodios, double duracao)
    {
        Serie nova = new Serie(nome, genero, anoLancamento, duracao, temporadas, qntEpisodios);

        if (BuscarPorNome(nova.Nome) == null)
        {
            BDCadastrarSerie(nova);
            series.Add(nova);
            Console.WriteLine("\nSérie cadastrada com sucesso!");
        }
        else
            Console.WriteLine("\nFilme ou Serie já cadastrado com mesmo nome.\n");
    }
    private void BDCadastrarSerie(Serie serie)
    {
        if (bancoDeDados.Conexao.State != System.Data.ConnectionState.Open)
            bancoDeDados.Conexao.Open();

        // Insere a parte midia da serie na tabela midias
        using MySqlCommand ComandoMidia = new($"INSERT INTO Midias (nome,genero,tipo,anolancamento) values (@nome,@genero,'serie',@anoLancamento)", bancoDeDados.Conexao);
        ComandoMidia.Parameters.AddWithValue("@nome", serie.Nome);
        ComandoMidia.Parameters.AddWithValue("@genero", serie.Genero);
        ComandoMidia.Parameters.AddWithValue("@anoLancamento", serie.AnoLancamento);
        ComandoMidia.ExecuteNonQuery();

        long MidiaId = ComandoMidia.LastInsertedId; // id da midia para inserir os dados na tabela series
        serie.Id = MidiaId;

        // Insere a serie na tabela series
        using MySqlCommand ComandoSerie = new($"INSERT INTO Series (MidiaId,Duracao,Temporadas,QntEpisodios) values (@midiaId,@duracao,@temporadas,@QtdEps)", bancoDeDados.Conexao);
        ComandoSerie.Parameters.AddWithValue("@midiaId", serie.Id);
        ComandoSerie.Parameters.AddWithValue("@duracao", serie.Duracao);
        ComandoSerie.Parameters.AddWithValue("@temporadas", serie.Temporadas);
        ComandoSerie.Parameters.AddWithValue("@QtdEps", serie.QntEpisodios);

        ComandoSerie.ExecuteNonQuery();

        bancoDeDados.Conexao.Close();
    }
    public void exibirSeries()
    {
        Console.WriteLine("================ SÉRIES ================\n");
        foreach (Serie s in series)
            Console.WriteLine(s);

        Console.WriteLine();
    }
    public void RemoverSeries(OperaçoesUsuario operacoes)
    {
        if (bancoDeDados.Conexao.State != System.Data.ConnectionState.Open)
            bancoDeDados.Conexao.Open();

        using MySqlCommand Comando = new("Delete FROM Midias WHERE tipo = 'serie' ", bancoDeDados.Conexao);
        Comando.ExecuteNonQuery();

        operacoes.AtualizarUsuarios();

        series.Clear();
    }

    // Manipulção de midia
    public bool RemoveMidia(Midia midia, OperaçoesUsuario operacao)
    {
        bool Encontrado = false;

        if (midia is Filme)
        {
            foreach (Filme F in filmes)
                if (midia.Nome == F.Nome)
                {
                    Encontrado = true;
                    filmes.Remove(F);
                    break;
                }
        }
        else if (midia is Serie)
        {
            foreach (Serie S in series)
                if (midia.Nome == S.Nome)
                {
                    Encontrado = true;
                    series.Remove(S);
                    break;
                }
        }
        else
            Console.WriteLine("tipo inválido");

        if (Encontrado)
        {
            if (bancoDeDados.Conexao.State != System.Data.ConnectionState.Open)
                bancoDeDados.Conexao.Open();

            using MySqlCommand Comando = new("DELETE FROM Midias WHERE nome=@value", bancoDeDados.Conexao);
            Comando.Parameters.AddWithValue("@value", midia.Nome);
            Comando.ExecuteNonQuery();
            bancoDeDados.Conexao.Close();

            operacao.AtualizarUsuarios();// Garante que a listaFavoritos é igual o banco de dados

            return true;
        }

        return false;
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

    // Funçoes count
    public int QtdFilmes() { return filmes.Count; }
    public int QtdSeries() { return series.Count; }
    public int QtdMidias() { return QtdFilmes() + QtdSeries(); }
}
public class Midia
{
    public BancoDeDados BancoDeDados = new();
    public long Id { get; set; }
    public string Nome { get; set; }
    public string Genero { get; set; }
    public int AnoLancamento { get; set; }

    public Midia(string nome, string genero, int anoLancamento)
    {
        Nome = nome;
        Genero = genero;
        AnoLancamento = anoLancamento;
    }
    public Midia(string nome, string genero, int anoLancamento, long id)
    {
        Nome = nome;
        Genero = genero;
        AnoLancamento = anoLancamento;
        Id = id;
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
    public Filme(string nome, string genero, int anoLancamento, long id, double duracao, string diretor)
       : base(nome, genero, anoLancamento, id)
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
    public Serie(string nome, string genero, int anoLancamento, long id, double duracao, int temporadas, int qntEpisodios)
    : base(nome, genero, anoLancamento, id)
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
public class Usuario
{
    public long Id;
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
    BancoDeDados bancoDeDados = new();
    private List<Midia> listaFavoritos = new();
    public Usuario() { }
    public Usuario(string Nome, string Senha, long Id)
    {
        this.Senha = Senha;
        this.Nome = Nome;
        this.Id = Id;
        AtualizaFavoritosComBD();
    }
    public void AtualizaFavoritosComBD()
    {
        listaFavoritos.Clear(); // Limpa a lista para recarrega-la com o banco de dados

        if (bancoDeDados.Conexao.State != System.Data.ConnectionState.Open)
            bancoDeDados.Conexao.Open();

        string query = @" SELECT m.id, m.nome, m.genero, m.tipo, m.anoLancamento,
                f.duracao AS filme_duracao, f.diretor,
                s.duracao AS serie_duracao, s.temporadas, s.qntEpisodios
                    FROM Favoritos fav
                    JOIN Midias m ON fav.MidiaId = m.id
                    LEFT JOIN Filmes f ON m.id = f.MidiaId
                    LEFT JOIN Series s ON m.id = s.MidiaId
                WHERE fav.UsuarioId = @UsuarioId";

        using MySqlCommand ComandoFavoritos = new(query, bancoDeDados.Conexao);
        ComandoFavoritos.Parameters.AddWithValue("@UsuarioId", Id);

        using MySqlDataReader LeitorFavoritos = ComandoFavoritos.ExecuteReader();

        while (LeitorFavoritos.Read())
        {
            string nome = LeitorFavoritos.GetString("Nome");
            string genero = LeitorFavoritos.GetString("Genero");
            int anolancamento = LeitorFavoritos.GetInt32("AnoLancamento");
            string tipo = LeitorFavoritos.GetString("tipo");
            int Id = LeitorFavoritos.GetInt32("Id");

            Midia nova = null;

            if (tipo.ToLower() == "filme")
            {
                double duracao = LeitorFavoritos.GetDouble("filme_duracao");
                string diretor = LeitorFavoritos.GetString("diretor");

                nova = new Filme(nome, genero, anolancamento, Id, duracao, diretor);
            }
            else if (tipo.ToLower() == "serie")
            {
                double duracao = LeitorFavoritos.GetDouble("serie_duracao");
                int QtdTemporadas = LeitorFavoritos.GetInt32("temporadas");
                int QtdEpisodios = LeitorFavoritos.GetInt32("QntEpisodios");

                nova = new Serie(nome, genero, anolancamento, Id, duracao, QtdTemporadas, QtdEpisodios);
            }
            if (BuscarFavoritoPorNome(nova.Nome) == null)
                listaFavoritos.Add(nova);
        }
        bancoDeDados.Conexao.Close();
    }

    // Manipulação de atributos de Usuario
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

    // Manipulação de favoritos
    public void AdicionarFavorito(Midia midiaNova)
    {
        bool MidiaExiste = false;

        // Verificação da existencia da midia na lista de favoritos
        foreach (Midia m in listaFavoritos)
            if (m.Nome == midiaNova.Nome)
                MidiaExiste = true;

        if (!MidiaExiste)
        {
            AdicionarFavoritoBD(midiaNova);
            listaFavoritos.Add(midiaNova);
            Console.WriteLine($"'{midiaNova.Nome}' foi adicionado aos favoritos.");
        }
        else
            Console.WriteLine($"{midiaNova} \nJÁ EXISTE NOS FAVORITOS!!");
    }
    private void AdicionarFavoritoBD(Midia midia)
    {
        if (bancoDeDados.Conexao.State != System.Data.ConnectionState.Open)
            bancoDeDados.Conexao.Open();


        using MySqlCommand Comando = new($"INSERT INTO Favoritos (UsuarioId,MidiaId) VALUES (@UsuarioId, @MidiaId)", bancoDeDados.Conexao);
        Comando.Parameters.AddWithValue("@UsuarioId", Id);
        Comando.Parameters.AddWithValue("@MidiaId", midia.Id);

        Comando.ExecuteNonQuery();

        bancoDeDados.Conexao.Close();
    }
    public void ExibirFavoritos()
    {
        Console.WriteLine("========== Lista de Favoritos ==========\n");
        foreach (Midia m in listaFavoritos)
            Console.WriteLine(m);
    }
    public bool RemoverFavorito(Midia midia)
    {
        foreach (Midia m in listaFavoritos)
            if (midia == m)
            {
                if (bancoDeDados.Conexao.State != System.Data.ConnectionState.Open)
                    bancoDeDados.Conexao.Open();


                using MySqlCommand Comando = new("DELETE FROM Favoritos WHERE UsuarioId = @UsuarioId AND MidiaId = @MidiaId", bancoDeDados.Conexao);
                Comando.Parameters.AddWithValue("@UsuarioId", Id);
                Comando.Parameters.AddWithValue("@MidiaId", midia.Id);

                Comando.ExecuteNonQuery();

                bancoDeDados.Conexao.Close();

                listaFavoritos.Remove(m);
                return true;
            }

        return false;
    }
    public void RemoverFavoritos()
    {
        if (bancoDeDados.Conexao.State != System.Data.ConnectionState.Open)
            bancoDeDados.Conexao.Open();

        using MySqlCommand Comando = new("DELETE FROM Favoritos Where UsuarioId = @UsuarioId", bancoDeDados.Conexao);
        Comando.Parameters.AddWithValue("@UsuarioId", Id);
        Comando.ExecuteNonQuery();

        bancoDeDados.Conexao.Close();

        listaFavoritos.Clear();
    }
    public Midia BuscarFavoritoPorNome(string nome)
    {
        foreach (Midia m in listaFavoritos)
            if (m.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase))
                return m;

        return null;
    }
    public int QtdFavoritos() { return listaFavoritos.Count; }
}
public class OperaçoesUsuario
{
    BancoDeDados bancoDeDados = new BancoDeDados();
    public List<Usuario> Usuarios = new List<Usuario>();
    public OperaçoesUsuario() // Inicializa o objeto o preenchendo com usuarios do BD
    {
        PreencherUsuariosComBD();
    }

    // Manipulação de usuario
    private void PreencherUsuariosComBD()
    {
        if (bancoDeDados.Conexao.State != System.Data.ConnectionState.Open)
            bancoDeDados.Conexao.Open();

        using MySqlCommand ComandoUsuarios = new("SELECT * FROM Usuarios", bancoDeDados.Conexao);
        using MySqlDataReader LeitorUsuario = ComandoUsuarios.ExecuteReader();

        while (LeitorUsuario.Read())
        {
            string nome = LeitorUsuario.GetString("NomeUsuario");
            string senha = LeitorUsuario.GetString("Senha");
            int id = LeitorUsuario.GetInt32("Id");

            Usuario novo = new(nome, senha, id);
            Usuarios.Add(novo);
        }
        bancoDeDados.Conexao.Close();
    }
    public void AtualizarUsuarios()
    {
        foreach (Usuario U in Usuarios) // Atualiza a lista de favoritos de cada usuario
            U.AtualizaFavoritosComBD();
    }
    public bool CadastrarUsuario()
    {
        Usuario Novo = new();
        bool UsuarioValido = false;
        do // Loop inserção de usuário
        {
            Console.Clear();

            Console.WriteLine("Cadastro de usuário");
            Console.WriteLine("=====================");
            Console.WriteLine("Digite as informações.\n");
            Console.Write("Nome : ");
            string nome = Console.ReadLine();

            if (BuscarUsuario(nome) == null) // Verifica se existe usuário com esse nome
            {
                UsuarioValido = true;
                Novo._Nome = nome;
                break;
            }
            else
                Console.WriteLine("\nUsuario já existente.");

            Console.WriteLine("Tentar novamente? (Enter confirma)");
        } while (Console.ReadKey(true).Key == ConsoleKey.Enter);

        if (UsuarioValido)
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
                    CadastrarUsuarioBD(Novo);
                    Usuarios.Add(Novo);

                    return true;
                }
                else
                    Console.WriteLine("\nTentar novamente? (Enter confirma)");

            } while (Console.ReadKey(true).Key == ConsoleKey.Enter);
        }
        return false;
    }
    private void CadastrarUsuarioBD(Usuario usuario)
    {
        if (bancoDeDados.Conexao.State != System.Data.ConnectionState.Open)
            bancoDeDados.Conexao.Open();

        using MySqlCommand Comando = new("INSERT INTO Usuarios (NomeUsuario,Senha) VALUES (@nome,@senha)", bancoDeDados.Conexao);
        Comando.Parameters.AddWithValue("@nome", usuario._Nome);
        Comando.Parameters.AddWithValue("@senha", usuario._Senha);
        Comando.ExecuteNonQuery();

        long id = Comando.LastInsertedId;
        usuario.Id = id;

        bancoDeDados.Conexao.Close();
    }
    public string Login()
    {
        do // Loop para teste do nome
        {
            Console.Clear();

            Console.WriteLine("Login de usuário");
            Console.WriteLine("=====================");
            Console.WriteLine("Digite as informações.\n");
            Console.Write("Nome : ");
            string nome = Console.ReadLine().Trim();

            Usuario UsuarioLogin = BuscarUsuario(nome); // Verifica se o usuário existe na cadastro

            if (UsuarioLogin != null)
            {
                Console.WriteLine("Usuário encontrado.");
                Thread.Sleep(400);

                do // Loop para teste de senha 
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
            if (x._Nome.Equals(Nome, StringComparison.OrdinalIgnoreCase))
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

    public void INSERT(string TABLE, string COLUMN, string VALUE)
    {
        Conexao.Open();

        using MySqlCommand Comando = new($"INSERT INTO {TABLE} ({COLUMN}) VALUES (@value)", Conexao);
        Comando.Parameters.AddWithValue("@value", VALUE);
        Comando.ExecuteNonQuery();

        Conexao.Close();
    }
    public void INSERT(string TABLE, string COLUMN1, string COLUMN2, string VALUE1, string VALUE2)
    {
        Conexao.Open();

        using MySqlCommand Comando = new($"INSERT INTO {TABLE} ({COLUMN1},{COLUMN2}) VALUES (@value1,@value2)", Conexao);
        Comando.Parameters.AddWithValue("@value1", VALUE1);
        Comando.Parameters.AddWithValue("@value2", VALUE2);
        Comando.ExecuteNonQuery();

        Conexao.Close();
    }
    public void SELECT(string TABLE, string COLUMN)
    {
        Conexao.Open();

        using MySqlCommand Comando = new($"SELECT {COLUMN} FROM {TABLE}", Conexao);

        using MySqlDataReader Leitor = Comando.ExecuteReader();

        while (Leitor.Read())
        {
            string valor = Leitor.GetString(COLUMN);
            Console.WriteLine($"{COLUMN} - {valor} ");
        }

        Conexao.Close();
    }
}

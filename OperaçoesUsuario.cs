using System;
using System.Runtime.CompilerServices;
using MySql.Data.MySqlClient;

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
        bool sts = false;
        do // Loop inserção de usuário
        {
            Console.Clear();

            Console.WriteLine("Cadastro de usuário");
            Console.WriteLine("=====================");
            Console.WriteLine("Digite as informações.");
            Console.WriteLine("O nome e a senha não podem ficar vazios");
            Console.WriteLine("A senha precisa ter no mínimo 6 dígitos.\n");
            Console.Write($"Nome : {Novo._Nome}");
            string nome = Console.ReadLine();

            sts = Novo.VerificaNome(nome);

            if (sts)
            {
                Console.Write("Senha: ");
                string senha = Console.ReadLine();

                if (Novo.VerificaSenha(senha))
                {
                    Novo.NovoNome(nome);
                    Novo._Senha = senha;
                    CadastrarUsuarioBD(Novo);
                    Usuarios.Add(Novo);

                    return true;
                }
            }
            else
                Console.WriteLine("\nTentar novamente? (Enter confirma)");

        } while (Console.ReadKey(true).Key == ConsoleKey.Enter);
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
                Console.WriteLine("Usuário inexistente");
                Console.WriteLine("\nTentar novamente? (Enter confirma)");
            }
        } while (Console.ReadKey(true).Key == ConsoleKey.Enter);

        Console.WriteLine("\nOperação cancelada.");
        return null;
    }
    public Usuario BuscarUsuario(string Nome)
    {
        try
        {
            if (bancoDeDados.Conexao.State != System.Data.ConnectionState.Open)
                bancoDeDados.Conexao.Open();

            using MySqlCommand comando = new("SELECT Id, NomeUsuario, Senha FROM Usuarios WHERE NomeUsuario = @nome", bancoDeDados.Conexao);
            comando.Parameters.AddWithValue("@nome", Nome);

            using MySqlDataReader leitor = comando.ExecuteReader();

            if (leitor.Read())
            {
                int id = leitor.GetInt32("Id");
                string nomeUsuario = leitor.GetString("NomeUsuario");
                string senha = leitor.GetString("Senha");

                return new Usuario(nomeUsuario, senha, id);
            }
            else
            {
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao buscar usuário: {ex.Message}");
            return null;
        }
        finally
        {
            bancoDeDados.Conexao.Close();
        }
    }

    public int QtdUsuarios() { return Usuarios.Count; }
}
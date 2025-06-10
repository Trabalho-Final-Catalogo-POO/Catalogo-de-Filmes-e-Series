class OperaçoesUsuario
{
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

    List<Usuario> Usuarios = new List<Usuario>();

    public string Cadastrar()
    {
        Usuario Novo = new();
        bool existe;
        do // Loop inserção de usuário
        {
            existe = false;

            Console.Clear();

            Console.WriteLine("Cadastro de usuário");
            Console.WriteLine("=====================");
            Console.WriteLine("Digite as informações.\n");
            Console.Write("Nome : ");
            string nome = Console.ReadLine();

            foreach (Usuario x in Usuarios)// Verifica se o usuário existe na cadastro
                if (nome.ToUpper() == x._Nome.ToUpper())
                {
                    existe = true;
                    Console.WriteLine("\nUsuário já existente.");

                    break;
                }

            if (!existe)
            {
                Novo._Nome = nome;
                break;
            }

            Console.WriteLine("\nTentar novamente? (Enter confirma)");
        } while (Console.ReadKey(true).Key == ConsoleKey.Enter);

        if (!existe)// Nome de usuário aprovado
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

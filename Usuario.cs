

class Usuario
{
    public string NomeUsuario { get; private set; }
    public string Senha { get; private set; }
    public bool Cadastrado { get; private set; } = false;

    public Favoritos favoritos { get; private set; } = new Favoritos();


    public void Cadastrar()
    {
        if (!Cadastrado)
        {
            Console.Write("Digite o nome de usuário: ");
            NomeUsuario = Console.ReadLine();
            Console.Write("Digite a senha: ");
            Senha = Console.ReadLine();
            Cadastrado = true;
            Console.WriteLine("Usuário cadastrado com sucesso!\n");
        }
        else
        {
            Console.WriteLine("Usuário já cadastrado.\n");
        }
    }

    public bool FazerLogin()
    {
        if (!Cadastrado)
        {
            Console.WriteLine("Nenhum usuário cadastrado. Cadastre primeiro.\n");
            return false;
        }

        Console.Write("Digite o nome de usuário: ");
        string nome = Console.ReadLine();
        Console.Write("Digite a senha: ");
        string senha = Console.ReadLine();

        if (nome == NomeUsuario && senha == Senha)
        {
            Console.WriteLine("Login bem-sucedido!\n");
            return true;
        }
        else
        {
            Console.WriteLine("Usuário ou senha incorretos.\n");
            return false;
        }
    }
}

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
using System;

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
                            OperaçoesUsuario.Usuario usuario = Usuarios.BuscarUsuario(Login);
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
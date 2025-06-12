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

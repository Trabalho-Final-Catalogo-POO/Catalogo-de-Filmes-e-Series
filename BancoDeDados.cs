using System;
using System.Runtime.CompilerServices;
using MySql.Data.MySqlClient;
//using Mysqlx.Crud;

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

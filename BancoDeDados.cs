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
    // Inserir no BD generico
    public void Inserir(string TABLE, string COLUMN, string VALUE)
    {
        Conexao.Open();

        using MySqlCommand Comando = new($"INSERT INTO {TABLE} ({COLUMN}) VALUES (@value)", Conexao);
        Comando.Parameters.AddWithValue("@value", VALUE);
        Comando.ExecuteNonQuery();

        Conexao.Close();
    }
    // Exibir do bd generico
    public void Exibir(string TABLE, string COLUMN)
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

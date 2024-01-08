using GameStoreFase4.Domain.Entities;
using GameStoreFase4.Infrastructure.Enums;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using Dapper;

namespace GameStoreFase4.Infrastructure.Repositories;
public class JogoRepository : IJogoRepository
{
    private readonly string _connectionString;

    public JogoRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("GameStoreDB");
    }
    public void Atualizar(Jogo jogo)
    {
        SqlConnection connection = new SqlConnection(_connectionString);

        try
        {
            connection.Open();
            string query = SqlManager.GetSql(TSqlQuery.ATUALIZAR_JOGO);
            int affectedLines = connection.Execute(query, jogo);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (connection.State.Equals(ConnectionState.Open))
                connection.Close();
        }
    }
    public void Cadastrar(Jogo jogo)
    {
        SqlConnection connection = new SqlConnection(_connectionString);

        try
        {
            string query = SqlManager.GetSql(TSqlQuery.CADASTRAR_JOGO);
            bool habilitarComparacaoNomes = true;

            connection.Open();

            if (habilitarComparacaoNomes)
            {
                var jogoPesquisado = PesquisarPeloNome(jogo.Nome, connection);
                if (jogoPesquisado == null)
                {
                    int returnedId = connection.ExecuteScalar<int>(query, jogo);
                }
            }
            else
            {
                int returnedId = connection.ExecuteScalar<int>(query, jogo);
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (connection.State.Equals(ConnectionState.Open))
                connection.Close();
        }
    }
    public void Cadastrar(List<Jogo> jogos)
    {
        SqlConnection connection = new SqlConnection(_connectionString);

        try
        {
            string query = SqlManager.GetSql(TSqlQuery.CADASTRAR_JOGO);
            bool habilitarComparacaoNomes = true;

            connection.Open();

            List<Jogo> jogosPesquisados = null;
            if (habilitarComparacaoNomes)
                jogosPesquisados = Listar();

            foreach (var jogo in jogos)
            {
                if (jogo != null)
                {
                    if (habilitarComparacaoNomes)
                    {
                        var jogoPesquisado = jogosPesquisados.FirstOrDefault(p => p.Nome.Equals(jogo.Nome));
                        if (jogoPesquisado == null)
                        {
                            int returnedId = connection.ExecuteScalar<int>(query, jogo);
                            jogo.Id = returnedId;
                        }
                    }
                    else
                    {
                        int returnedId = connection.ExecuteScalar<int>(query, jogo);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (connection.State.Equals(ConnectionState.Open))
                connection.Close();
        }
    }
    public bool Excluir(int id)
    {
        SqlConnection connection = new SqlConnection(_connectionString);

        try
        {
            connection.Open();
            string query = SqlManager.GetSql(TSqlQuery.EXCLUIR_JOGO);
            int affectedLines = connection.Execute(query, new { id = id });
            return affectedLines > 0;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (connection.State.Equals(ConnectionState.Open))
                connection.Close();
        }
    }
    public List<Jogo> Listar()
    {
        List<Jogo> result = null;
        string query = SqlManager.GetSql(TSqlQuery.LISTAR_JOGOS);

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            result = connection.Query<Jogo>(query).ToList();
        }

        return result;
    }
    public Jogo PesquisarPeloId(int id)
    {
        SqlConnection connection = new SqlConnection(_connectionString);

        try
        {
            connection.Open();
            string query = SqlManager.GetSql(TSqlQuery.PESQUISAR_JOGOS_PELO_ID);
            Jogo jogo = connection.QueryFirstOrDefault<Jogo>(query, new { id = id });
            return jogo;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (connection.State.Equals(ConnectionState.Open))
                connection.Close();
        }
    }
    public Jogo PesquisarPeloNome(string nome)
    {
        SqlConnection connection = new SqlConnection(_connectionString);

        try
        {
            connection.Open();
            string query = SqlManager.GetSql(TSqlQuery.PESQUISAR_JOGOS_PELO_NOME);
            Jogo jogo = connection.QueryFirstOrDefault<Jogo>(query, new { nome = nome });
            return jogo;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (connection.State.Equals(ConnectionState.Open))
                connection.Close();
        }
    }
    private Jogo PesquisarPeloNome(string nome, SqlConnection connection)
    {
        try
        {
            string query = SqlManager.GetSql(TSqlQuery.PESQUISAR_JOGOS_PELO_NOME);
            Jogo jogo = connection.QueryFirstOrDefault<Jogo>(query, new { nome = nome });
            return jogo;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

}

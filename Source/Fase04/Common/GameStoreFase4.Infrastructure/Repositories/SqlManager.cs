using GameStoreFase4.Infrastructure.Enums;

namespace GameStoreFase4.Infrastructure.Repositories;
public class SqlManager
{
    public static string GetSql(TSqlQuery tsql)
    {
        string querySql = "";

        switch (tsql)
        {
            case TSqlQuery.CADASTRAR_JOGO:
                querySql = "insert into jogos (nome, genero, precoUnitario, console) values (@nome, @genero, @precoUnitario, @console); SELECT CAST(SCOPE_IDENTITY() as int)";
                break;

            case TSqlQuery.ATUALIZAR_JOGO:
                querySql = "update jogos set nome = @nome, genero = @genero, precoUnitario = @precoUnitario, console = @console where id = @id";
                break;

            case TSqlQuery.EXCLUIR_JOGO:
                querySql = "delete jogos where id = @id";
                break;

            case TSqlQuery.LISTAR_JOGOS:
                querySql = "select * from jogos";
                break;

            case TSqlQuery.PESQUISAR_JOGOS_PELO_ID:
                querySql = "select * from jogos where id = @id";
                break;

            case TSqlQuery.PESQUISAR_JOGOS_PELO_NOME:
                querySql = "select * from jogos where nome = @nome";
                break;
        }

        return querySql;
    }
}


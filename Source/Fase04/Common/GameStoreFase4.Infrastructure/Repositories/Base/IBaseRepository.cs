namespace GameStoreFase4.Infrastructure.Repositories.Base;
public interface IBaseRepository<T, Y>
{
    void Cadastrar(T entity);
    void Atualizar(T entity);
    bool Excluir(Y id);
    T PesquisarPeloNome(string nome);
    List<T> Listar();
    T PesquisarPeloId(Y id);
}

using GameStoreFase4.Domain.Entities;
using GameStoreFase4.Infrastructure.Repositories.Base;

namespace GameStoreFase4.Infrastructure.Repositories;
public interface IJogoRepository : IBaseRepository<Jogo, int>
{
    void Cadastrar(List<Jogo> jogos);
}

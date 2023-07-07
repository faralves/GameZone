namespace GameZone.Core.Data
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}
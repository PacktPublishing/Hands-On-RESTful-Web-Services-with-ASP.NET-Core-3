namespace VinylStore.Catalog.Domain.Infrastructure.Repositories
{
    public interface IRepository
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
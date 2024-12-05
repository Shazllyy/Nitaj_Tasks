using DomainLayer.Interfaces;

namespace InfrastructureLayer.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ITodoItemRepository TodoItemRepository { get; }
        Task CommitAsync();

    }
}

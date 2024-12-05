using DomainLayer.Interfaces;
using InfrastructureLayer.DbContextData;
using InfrastructureLayer.Interfaces;
using System;
using System.Threading.Tasks;

namespace InfrastructureLayer.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbcontext _context;
        public ITodoItemRepository TodoItemRepository { get; }

        public UnitOfWork(ApplicationDbcontext context, ITodoItemRepository todoItemRepository)
        {
            _context = context;
            TodoItemRepository = todoItemRepository;
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}

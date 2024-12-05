using DomainLayer.Entities;
using InfrastructureLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.Services
{
    public class TodoItemService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TodoItemService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TodoItem> CreateTodoItemAsync(string title, string? description = null)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Title is required for the Todo item.");
            }
            var todoItem = new TodoItem(title, description); 

            await _unitOfWork.TodoItemRepository.AddAsync(todoItem);
            await _unitOfWork.CommitAsync();

            return todoItem;
        }

        public async Task<IEnumerable<TodoItem>> GetAllTodoItemsAsync()
        {
            var todoItems = await _unitOfWork.TodoItemRepository.GetAllAsync();

            if (todoItems == null || !todoItems.Any())
            {
                throw new InvalidOperationException("No Todo items found.");
            }

            return todoItems;
        }

        public async Task<IEnumerable<TodoItem>> GetPendingTodoItemsAsync()
        {
            var pendingTodoItems = await _unitOfWork.TodoItemRepository.GetPendingAsync();

            if (pendingTodoItems == null || !pendingTodoItems.Any())
            {
                throw new InvalidOperationException("No pending Todo items found.");
            }

            return pendingTodoItems;
        }
        public async Task<TodoItem> GetTodoItemByIdAsync(int id)
        {
            var todoItem = await _unitOfWork.TodoItemRepository.GetByIdAsync(id);
            if (todoItem == null)
            {
                throw new KeyNotFoundException($"Todo item with ID {id} not found.");
            }
            return todoItem;
        }


        public async Task MarkAsCompletedAsync(int id)
        {
            var todoItem = await _unitOfWork.TodoItemRepository.GetByIdAsync(id);

            if (todoItem == null)
            {
                throw new KeyNotFoundException($"Todo item with ID {id} not found.");
            }

            todoItem.MarkAsCompleted();
            await _unitOfWork.TodoItemRepository.UpdateAsync(todoItem);
            await _unitOfWork.CommitAsync();
        }
    }
}

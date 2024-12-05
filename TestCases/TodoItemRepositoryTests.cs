using Xunit;
using InfrastructureLayer.DbContextData;
using InfrastructureLayer.Repositories;
using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace TestCases
{
    public class TodoItemRepositoryTests
    {
        private readonly ApplicationDbcontext _dbContext;
        private readonly TodoItemRepository _todoItemRepository;

        public TodoItemRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbcontext>()
                .UseInMemoryDatabase("TestDb")
                .Options;

            _dbContext = new ApplicationDbcontext(options);
            _todoItemRepository = new TodoItemRepository(_dbContext);
        }

        private async Task CleanupDatabaseAsync()
        {
            _dbContext.TodoItems.RemoveRange(_dbContext.TodoItems);
            await _dbContext.SaveChangesAsync();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnTodoItem()
        {
            await CleanupDatabaseAsync();
            var todoItem = new TodoItem("Todo 1");
            _dbContext.TodoItems.Add(todoItem);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _todoItemRepository.GetByIdAsync(todoItem.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(todoItem.Title, result.Title);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllTodoItems()
        {
            // Arrange
            await CleanupDatabaseAsync(); // Ensure DB is clean before the test
            var todoItem1 = new TodoItem("Todo 1");
            var todoItem2 = new TodoItem("Todo 2");
            _dbContext.TodoItems.AddRange(todoItem1, todoItem2);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _todoItemRepository.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, t => t.Title == "Todo 1");
            Assert.Contains(result, t => t.Title == "Todo 2");
        }

        [Fact]
        public async Task AddAsync_ShouldAddTodoItem()
        {
            // Arrange
            await CleanupDatabaseAsync(); // Ensure DB is clean before the test
            var todoItem = new TodoItem("New Todo");

            // Act
            await _todoItemRepository.AddAsync(todoItem);
            var savedItem = await _dbContext.TodoItems.FirstOrDefaultAsync(t => t.Title == "New Todo");

            // Assert
            Assert.NotNull(savedItem);
            Assert.Equal("New Todo", savedItem.Title);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateTodoItem()
        {
            // Arrange
            await CleanupDatabaseAsync(); 
            var todoItem = new TodoItem("New Todo");
            _dbContext.TodoItems.Add(todoItem);
            await _dbContext.SaveChangesAsync();

            // Act
            todoItem.SetTitle("Updated Todo");
            await _todoItemRepository.UpdateAsync(todoItem);
            var updatedItem = await _dbContext.TodoItems.FirstOrDefaultAsync(t => t.Title == "Updated Todo");

            // Assert
            Assert.NotNull(updatedItem);
            Assert.Equal("Updated Todo", updatedItem.Title);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveTodoItem()
        {
            // Arrange
            await CleanupDatabaseAsync(); 
            var todoItem = new TodoItem("Todo 1");
            _dbContext.TodoItems.Add(todoItem);
            await _dbContext.SaveChangesAsync();

            // Act
            await _todoItemRepository.DeleteAsync(todoItem.Id);
            var deletedItem = await _dbContext.TodoItems.FirstOrDefaultAsync(t => t.Id == todoItem.Id);

            // Assert
            Assert.Null(deletedItem);
        }

        [Fact]
        public async Task GetPendingTodoItemsAsync_ShouldReturnPendingItems()
        {
            // Arrange
            await CleanupDatabaseAsync(); 
            var todoItem1 = new TodoItem("Todo 1");
            var todoItem2 = new TodoItem("Todo 2");
            var todoItem3 = new TodoItem("Todo 3");
            todoItem2.MarkAsCompleted(); 
            _dbContext.TodoItems.AddRange(todoItem1, todoItem2, todoItem3);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _todoItemRepository.GetPendingAsync();

            // Assert
            Assert.Equal(2, result.Count()); 
            Assert.Contains(result, t => t.Title == "Todo 1");
            Assert.Contains(result, t => t.Title == "Todo 3");
        }
    }
}

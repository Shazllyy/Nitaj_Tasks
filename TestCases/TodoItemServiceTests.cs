using Moq;
using Xunit;
using Business_Logic_Layer.Services;
using DomainLayer.Entities;
using DomainLayer.Interfaces;
using InfrastructureLayer.Repositories;
using InfrastructureLayer.DbContextData;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using InfrastructureLayer.UnitOfWork;
using InfrastructureLayer.Interfaces;

namespace TestCases
{
    public class TodoItemServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly TodoItemService _todoItemService;
        private readonly Mock<ITodoItemRepository> _mockTodoItemRepository;

        public TodoItemServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockTodoItemRepository = new Mock<ITodoItemRepository>();
            _mockUnitOfWork.Setup(u => u.TodoItemRepository).Returns(_mockTodoItemRepository.Object);
            _todoItemService = new TodoItemService(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task CreateTodoItemAsync_ShouldAddTodoItem()
        {
            // Arrange
            var newTodo = new TodoItem("New Todo");

            _mockTodoItemRepository.Setup(r => r.AddAsync(It.IsAny<TodoItem>())).Returns(Task.CompletedTask);

            // Act
            var result = await _todoItemService.CreateTodoItemAsync("New Todo");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("New Todo", result.Title);
            _mockTodoItemRepository.Verify(r => r.AddAsync(It.IsAny<TodoItem>()), Times.Once);
        }

        [Fact]
        public async Task GetPendingTodoItemsAsync_ShouldReturnPendingItems()
        {
            // Arrange
            var todoItem1 = new TodoItem("Todo 1");
            var todoItem2 = new TodoItem("Todo 2");
            todoItem2.MarkAsCompleted(); 
            var pendingItems = new List<TodoItem> { todoItem1 };
            _mockTodoItemRepository.Setup(r => r.GetPendingAsync()).ReturnsAsync(pendingItems);

            // Act
            var result = await _todoItemService.GetPendingTodoItemsAsync();

            // Assert
            Assert.Single(result);
            Assert.Contains(result, t => t.Title == "Todo 1");
        }

        [Fact]
        public async Task MarkAsCompletedAsync_ShouldMarkItemAsCompleted()
        {
            // Arrange
            var todoItem = new TodoItem("Todo to mark");
            _mockTodoItemRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(todoItem);
            _mockTodoItemRepository.Setup(r => r.UpdateAsync(It.IsAny<TodoItem>())).Returns(Task.CompletedTask);

            // Act
            await _todoItemService.MarkAsCompletedAsync(todoItem.Id);

            // Assert
            Assert.True(todoItem.IsCompleted);
            _mockTodoItemRepository.Verify(r => r.UpdateAsync(It.IsAny<TodoItem>()), Times.Once);
        }
    }
}

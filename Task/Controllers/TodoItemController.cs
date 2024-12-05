using Business_Logic_Layer.Services;
using DomainLayer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemController : ControllerBase
    {
        private readonly TodoItemService _todoItemService;
        private readonly ILogger<TodoItemController> _logger;

        public TodoItemController(TodoItemService todoItemService, ILogger<TodoItemController> logger)
        {
            _todoItemService = todoItemService;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(TodoItem), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateTodoItemAsync([FromBody] TodoItem todoItem)
        {
            if (todoItem == null)
            {
                return BadRequest("Todo item cannot be null.");
            }

            if (string.IsNullOrEmpty(todoItem.Title))
            {
                return BadRequest("Todo item title cannot be empty.");
            }

            _logger.LogInformation("Creating a new Todo item with title: {Title}", todoItem.Title);

            try
            {
                var createdTodoItem = await _todoItemService.CreateTodoItemAsync(todoItem.Title, todoItem.Description);

                _logger.LogInformation("Todo item created successfully with ID: {Id}", createdTodoItem.Id);

                return Created("", createdTodoItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the Todo item.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTodoItemsAsync()
        {
            _logger.LogInformation("Retrieving all Todo items.");

            try
            {
                var todoItems = await _todoItemService.GetAllTodoItemsAsync();

                if (todoItems == null || !todoItems.Any())
                {
                    return Ok("No data available for Todo items.");  
                }

                return Ok(todoItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving Todo items.");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingTodoItemsAsync()
        {
            _logger.LogInformation("Retrieving all pending Todo items.");

            try
            {
                var pendingTodoItems = await _todoItemService.GetPendingTodoItemsAsync();

                if (pendingTodoItems == null || !pendingTodoItems.Any())
                {
                    return Ok("No pending Todo items available.");  
                }

                return Ok(pendingTodoItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving pending Todo items.");
                return StatusCode(500, "Theres no Pending Task");
            }
        }

        [HttpPut("{id}/complete")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> MarkAsCompletedAsync(int id)
        {
            _logger.LogInformation("Marking Todo item with ID: {Id} as completed.", id);

            try
            {
                await _todoItemService.MarkAsCompletedAsync(id);

                _logger.LogInformation("Todo item with ID: {Id} marked as completed successfully.", id);
                return Ok("Todo item marked as completed successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Todo item not found with ID: {Id}", id);
                return NotFound(ex.Message);  
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while marking Todo item with ID: {Id} as completed.", id);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

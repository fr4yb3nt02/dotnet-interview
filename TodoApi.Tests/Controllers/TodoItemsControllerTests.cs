using Microsoft.AspNetCore.Mvc;
using Moq;
using TodoApi.BusinessLogic.Interfaces;
using TodoApi.Controllers;
using TodoApi.Dtos;
using TodoApi.Models;

namespace TodoApi.Tests.Controllers;

#nullable disable
public class TodoItemsControllerTests
{
    private readonly Mock<ITodoItemsBL> _todoItemsBL = new Mock<ITodoItemsBL>();
    private readonly TodoitemsController _controller;

    public TodoItemsControllerTests()
    {
        _todoItemsBL = new Mock<ITodoItemsBL>();
        _controller = new TodoitemsController(_todoItemsBL.Object);
    }

    private void SetupMocks()
    {
        _todoItemsBL.Setup(x => x.GetTodoItems(1)).ReturnsAsync(new List<TodoItem>
        {
            new TodoItem { Id = 1, Title = "Item 1", IsCompleted = false},
            new TodoItem {  Id = 2, Title = "Item 2", IsCompleted = true }
        });
    }

    [Fact]
    public async Task GetTodoItems_WhenCalled_ReturnsTodoItems()
    {

        SetupMocks();

        var result = await _controller.GetTodoItems(1);

        Assert.IsType<OkObjectResult>(result.Result);
        var todoItems = Assert.IsAssignableFrom<IList<TodoItem>>(Assert.IsType<OkObjectResult>(result.Result).Value);
        Assert.Equal(2, todoItems.Count);

    }

    [Fact]
    public async Task GetTodoItem_WhenCalled_ReturnsTodoItemtById()
    {

        _todoItemsBL.Setup(x => x.GetTodoItem(1, 1)).ReturnsAsync(new TodoItem { Id = 1, Title = "Item 1", IsCompleted = false });

        var result = await _controller.GetTodoItem(1, 1);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var todoItem = Assert.IsType<TodoItem>(okResult.Value);
        Assert.Equal(1, todoItem.Id);
        Assert.Equal("Item 1", todoItem.Title);
    }

    [Fact]
    public async Task GetTodoItem_InvalidId_ReturnsKeyNotFoundException()
    {
        _todoItemsBL.Setup(x => x.GetTodoItem(1, 99)).ThrowsAsync(new KeyNotFoundException("Item with ID 99 not found on list with ID 1"));

        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _controller.GetTodoItem(1, 99));
        Assert.Equal("Item with ID 99 not found on list with ID 1", exception.Message);
    }

    [Fact]
    public async Task CreateTodoItem_WhenCalled_CreatesTodoItem()
    {
        var newTodoItem = new CreateTodoitem { Title = "Item 3" };
        _todoItemsBL.Setup(x => x.CreateTodoItem(1, newTodoItem))
        .ReturnsAsync(new TodoItem { Id = 3, Title = "Item 3", IsCompleted = false });

        var result = await _controller.PostTodoItem(1, newTodoItem);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var todoItem = Assert.IsType<TodoItem>(createdResult.Value);
        Assert.Equal(3, todoItem.Id);
        Assert.Equal("Item 3", todoItem.Title);
    }

    [Fact]
    public async Task UpdateTodoItem_ValidId_UpdatesTodoItem()
    {
        var updatedTodoItem = new UpdateTodoItem { Title = "Item 1 changed", IsCompleted = true };
        _todoItemsBL.Setup(x => x.UpdateTodoItem(1, 1, updatedTodoItem))
            .ReturnsAsync(new TodoItem { Id = 1, Title = "Item 1 changed", IsCompleted = true });

        var result = await _controller.PutTodoItem(1, 1, updatedTodoItem);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var todoItem = Assert.IsType<TodoItem>(okResult.Value);
        Assert.Equal(1, todoItem.Id);
        Assert.Equal("Item 1 changed", todoItem.Title);
        Assert.True(todoItem.IsCompleted);
    }

    [Fact]
    public async Task UpdateTodoItem_InvalidId_ReturnsKeyNotFoundException()
    {
        var updatedTodoItem = new UpdateTodoItem { Title = "Item 1 Not exists", IsCompleted = true };
        _todoItemsBL.Setup(x => x.UpdateTodoItem(1, 99, updatedTodoItem))
            .ThrowsAsync(new KeyNotFoundException("Item with ID 99 not found on list with ID 1"));

        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _controller.PutTodoItem(1, 99, updatedTodoItem));
        Assert.Equal("Item with ID 99 not found on list with ID 1", exception.Message);
    }

    [Fact]
    public async Task DeleteTodoItem_ValidId_RemovesTodoItem()
    {
        _todoItemsBL.Setup(x => x.DeleteTodoItem(1, 1))
            .ReturnsAsync(true);

        var result = await _controller.DeleteTodoItem(1, 1);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteTodoItem_InvalidId_ReturnsKeyNotFoundException()
    {
        _todoItemsBL.Setup(x => x.DeleteTodoItem(1, 99))
        .ThrowsAsync(new KeyNotFoundException("Item with ID 99 not found on list with ID 1"));

        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _controller.DeleteTodoItem(1, 99));
        Assert.Equal("Item with ID 99 not found on list with ID 1", exception.Message);
    }
}
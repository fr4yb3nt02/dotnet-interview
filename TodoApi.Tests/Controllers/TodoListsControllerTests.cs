using Microsoft.AspNetCore.Mvc;
using Moq;
using TodoApi.BusinessLogic.Interfaces;
using TodoApi.Controllers;
using TodoApi.Dtos;
using TodoApi.Models;

namespace TodoApi.Tests.Controllers;

#nullable disable
public class TodoListsControllerTests
{
    private readonly Mock<ITodoListsBL> _todoListsBL = new Mock<ITodoListsBL>();
    private readonly TodoListsController _controller;

    public TodoListsControllerTests()
    {
        _todoListsBL = new Mock<ITodoListsBL>();
        _controller = new TodoListsController(_todoListsBL.Object);
    }

    private void SetupMocks()
    {
        _todoListsBL.Setup(x => x.GetTodoLists()).ReturnsAsync(new List<TodoList>
        {
            new TodoList { Id = 1, Name = "Task 1", Items = new List<TodoItem>() },
            new TodoList { Id = 2, Name = "Task 2" , Items = new List<TodoItem>() }
        });

        _todoListsBL.Setup(x => x.GetTodoList(1)).ReturnsAsync(new TodoList
        {
            Id = 1,
            Name = "Task 1",
            Items = new List<TodoItem>()
        });

        _todoListsBL.Setup(x => x.GetTodoList(It.Is<long>(id => id != 1))).ReturnsAsync((TodoList)null);
    }

    [Fact]
    public async Task GetTodoList_WhenCalled_ReturnsTodoListList()
    {
        SetupMocks();

        var result = await _controller.GetTodoLists();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var todoLists = Assert.IsAssignableFrom<IList<TodoList>>(okResult.Value);
        Assert.Equal(2, todoLists.Count);
        Assert.Contains(todoLists, t => t.Name == "Task 1");
        Assert.Contains(todoLists, t => t.Name == "Task 2");
    }

    [Fact]
    public async Task GetTodoList_WhenCalled_ReturnsTodoListById()
    {

        SetupMocks();

        var result = await _controller.GetTodoList(1);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var todoList = Assert.IsType<TodoList>(okResult.Value);
        Assert.Equal(1, todoList.Id);
        Assert.Equal("Task 1", todoList.Name);

    }

    [Fact]
    public async Task GetTodoList_InvalidId_ShouldReturnKeyNotFoundException()
    {
        SetupMocks();

        _todoListsBL.Setup(x => x.GetTodoList(99))
        .ThrowsAsync(new KeyNotFoundException("List with ID 99 not found."));

        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _controller.GetTodoList(99));
        Assert.Equal("List with ID 99 not found.", exception.Message);
    }

    [Fact]
    public async Task PutTodoList_WhenTodoListDoesntExist_ReturnsBadRequest()
    {

        SetupMocks();

        _todoListsBL.Setup(x => x.UpdateTodoList(99, It.IsAny<Dtos.UpdateTodoList>()))
       .ThrowsAsync(new KeyNotFoundException("List with ID 99 not found."));

        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _controller.PutTodoList(99, new Dtos.UpdateTodoList { Name = "Task will explode" }));
        Assert.Equal("List with ID 99 not found.", exception.Message);

    }

    [Fact]
    public async Task PutTodoList_WhenCalled_UpdatesTheTodoList()
    {

        SetupMocks();

        var result = await _controller.PutTodoList(2, new Dtos.UpdateTodoList { Name = "Changed Task 2" });

        Assert.IsType<OkObjectResult>(result);

    }

    [Fact]
    public async Task PostTodoList_WhenCalled_CreatesTodoList()
    {

        SetupMocks();

        var newTodoList = new CreateTodoList { Name = "Task 3" };

        _todoListsBL.Setup(x => x.CreateTodoList(newTodoList))
        .ReturnsAsync(new TodoList { Id = 3, Name = newTodoList.Name, Items = new List<TodoItem>() });

        var result = await _controller.PostTodoList(newTodoList);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var todoList = Assert.IsType<TodoList>(createdResult.Value);
        Assert.Equal(3, todoList.Id);
        Assert.Equal("Task 3", todoList.Name);

    }

    [Fact]
    public async Task DeleteTodoList_WhenCalled_RemovesTodoList()
    {

        SetupMocks();

        _todoListsBL.Setup(x => x.DeleteTodoList(1)).ReturnsAsync(true);

        var result = await _controller.DeleteTodoList(1);

        Assert.IsType<NoContentResult>(result);

    }

    [Fact]
    public async Task DeleteTodoList_IncorrectId_ShouldThrowException()
    {

        SetupMocks();

        _todoListsBL.Setup(x => x.DeleteTodoList(99))
        .ThrowsAsync(new KeyNotFoundException("List with ID 99 not found."));

        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _controller.DeleteTodoList(99));
        Assert.Equal("List with ID 99 not found.", exception.Message);

    }
}

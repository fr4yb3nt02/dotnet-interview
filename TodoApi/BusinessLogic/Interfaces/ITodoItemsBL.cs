using TodoApi.Dtos;
using TodoApi.Models;

namespace TodoApi.BusinessLogic.Interfaces
{
    public interface ITodoItemsBL
    {
        public Task<IList<TodoItem>> GetTodoItems(long todoListId);
        public Task<TodoItem> GetTodoItem(long todoListId, long id);
        public Task<TodoItem> UpdateTodoItem(long todoListId, long id, UpdateTodoItem payload);
        public Task<TodoItem> CreateTodoItem(long todoListId, CreateTodoitem payload);
        public Task<bool> DeleteTodoItem(long todoListId, long id);
        public Task CompleteTodoItem(long todoListId, long id);
    }
}
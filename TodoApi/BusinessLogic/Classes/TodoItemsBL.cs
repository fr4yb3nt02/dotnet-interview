using TodoApi.BusinessLogic.Interfaces;
using TodoApi.DataAccess.Interfaces;
using TodoApi.Dtos;
using TodoApi.Models;

namespace TodoApi.BusinessLogic.Classes
{
    public class TodoItemsBL : ITodoItemsBL
    {
        private readonly ITodoItemsDA _todoItemsDa;

        public TodoItemsBL(ITodoItemsDA todoItemsDa)
        {
            _todoItemsDa = todoItemsDa;
        }

        public async Task<IList<TodoItem>> GetTodoItems(long todoListId)
        {
            return await _todoItemsDa.GetTodoItems(todoListId);
        }
        public async Task<TodoItem> GetTodoItem(long todoListId, long id)
        {
            return await _todoItemsDa.GetTodoItem(todoListId, id);
        }
        public async Task<TodoItem> UpdateTodoItem(long todoListId, long id, UpdateTodoItem payload)
        {
            return await _todoItemsDa.UpdateTodoItem(todoListId, id, payload);
        }
        public async Task<TodoItem> CreateTodoItem(long todoListId, CreateTodoitem payload)
        {
            return await _todoItemsDa.CreateTodoItem(todoListId, payload);
        }
        public async Task<bool> DeleteTodoItem(long todoListId, long id)
        {
            return await _todoItemsDa.DeleteTodoItem(todoListId, id);
        }
        public async Task CompleteTodoItem(long todoListId, long id)
        {
            await _todoItemsDa.CompleteTodoItem(todoListId, id);
        }
    }
}
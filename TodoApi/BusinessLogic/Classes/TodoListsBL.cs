using TodoApi.BusinessLogic.Interfaces;
using TodoApi.DataAccess.Interfaces;
using TodoApi.Dtos;
using TodoApi.Models;

namespace TodoApi.BusinessLogic.Classes
{
    public class TodoListsBL : ITodoListsBL
    {
        private readonly ITodoListsDA _todoListsDA;

        public TodoListsBL(ITodoListsDA todoListsDA)
        {
            _todoListsDA = todoListsDA;
        }

        public async Task<IList<TodoList>> GetTodoLists()
        {
            return await _todoListsDA.GetTodoLists();
        }
        public async Task<TodoList> GetTodoList(long id)
        {
            return await _todoListsDA.GetTodoList(id);
        }
        public async Task<TodoList> UpdateTodoList(long id, UpdateTodoList payload)
        {
            return await _todoListsDA.UpdateTodoList(id, payload);
        }
        public async Task<TodoList> CreateTodoList(CreateTodoList payload)
        {
            return await _todoListsDA.CreateTodoList(payload);
        }
        public async Task<bool> DeleteTodoList(long id)
        {
            return await _todoListsDA.DeleteTodoList(id);
        }

    }
}
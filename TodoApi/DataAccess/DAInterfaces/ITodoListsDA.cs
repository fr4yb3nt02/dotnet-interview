using TodoApi.Dtos;
using TodoApi.Models;

namespace TodoApi.DataAccess.Interfaces
{
    public interface ITodoListsDA
    {
        public Task<IList<TodoList>> GetTodoLists();
        public Task<TodoList> GetTodoList(long id);
        public Task<TodoList> UpdateTodoList(long id, UpdateTodoList payload);
        public Task<TodoList> CreateTodoList(CreateTodoList payload);
        public Task<bool> DeleteTodoList(long id);


    }
}
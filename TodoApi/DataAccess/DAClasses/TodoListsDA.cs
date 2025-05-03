using Microsoft.EntityFrameworkCore;
using TodoApi.DataAccess.Interfaces;
using TodoApi.Dtos;
using TodoApi.Models;

namespace TodoApi.DataAccess.Classes
{
    public class TodoListsDA : ITodoListsDA
    {
        private readonly TodoContext _context;

        public TodoListsDA(TodoContext context)
        {
            _context = context;
        }

        public async Task<IList<TodoList>> GetTodoLists()
        {
            return await _context.TodoList.ToListAsync();
        }
        public async Task<TodoList> GetTodoList(long id)
        {
            return await FindTodoList(id);
        }
        public async Task<TodoList> UpdateTodoList(long id, UpdateTodoList payload)
        {

            var todoList = await FindTodoList(id);

            if (string.IsNullOrWhiteSpace(payload.Name))
                throw new ArgumentException("Name cannot be empty.");

            todoList.Name = payload.Name;
            await _context.SaveChangesAsync();

            return todoList;
        }
        public async Task<TodoList> CreateTodoList(CreateTodoList payload)
        {
            var todoList = new TodoList { Name = payload.Name };

            if (string.IsNullOrWhiteSpace(payload.Name))
                throw new ArgumentException("Name cannot be empty.");

            _context.TodoList.Add(todoList);
            await _context.SaveChangesAsync();

            return todoList;
        }
        public async Task<bool> DeleteTodoList(long id)
        {
            var todoList = await FindTodoList(id);

            _context.TodoList.Remove(todoList);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<TodoList> FindTodoList(long id)
        {
            var todoList = await _context.TodoList.FindAsync(id);

            if (todoList == null)
                throw new KeyNotFoundException($"List with ID {id} not found.");

            return todoList;
        }
    }
}
using Microsoft.EntityFrameworkCore;
using TodoApi.DataAccess.Interfaces;
using TodoApi.Dtos;
using TodoApi.Models;

namespace TodoApi.DataAccess.Classes
{
    public class TodoItemsDA : ITodoItemsDA
    {
        private readonly TodoContext _context;

        public TodoItemsDA(TodoContext context)
        {
            _context = context;
        }

        public async Task<IList<TodoItem>> GetTodoItems(long todoListId)
        {
            var todoList = await FindTodoList(todoListId);
            return todoList.Items;
        }
        public async Task<TodoItem> GetTodoItem(long todoListId, long id)
        {
            var todoList = await FindTodoList(todoListId);

            var todoItem = todoList.Items.SingleOrDefault(x => x.Id == id);

            if (todoItem == null)
                throw new KeyNotFoundException($"Item with ID {id} not found on list with ID {todoListId}");

            return todoItem;
        }
        public async Task<TodoItem> UpdateTodoItem(long todoListId, long id, UpdateTodoItem payload)
        {
            var todoItem = await GetTodoItem(todoListId, id);

            if (string.IsNullOrWhiteSpace(payload.Title))
                throw new ArgumentException("Title of the item cannot be empty.");

            todoItem.Title = payload.Title;
            todoItem.IsCompleted = payload.IsCompleted;

            _context.TodoItem.Update(todoItem);
            await _context.SaveChangesAsync();

            return todoItem;

        }
        public async Task<TodoItem> CreateTodoItem(long todoListId, CreateTodoitem payload)
        {

            var todoList = await FindTodoList(todoListId);

            if (string.IsNullOrWhiteSpace(payload.Title))
                throw new ArgumentException("Title of the item cannot be empty.");

            var todoItemToCreate = new TodoItem
            {
                Title = payload.Title
            };

            todoList.Items.Add(todoItemToCreate);

            _context.TodoList.Update(todoList);
            await _context.SaveChangesAsync();

            return todoItemToCreate;

        }
        public async Task<bool> DeleteTodoItem(long todoListId, long id)
        {

            var todoList = await FindTodoList(todoListId);
            var todoItemToDelete = await GetTodoItem(todoListId, id);

            todoList.Items.Remove(todoItemToDelete);
            _context.TodoList.Update(todoList);
            await _context.SaveChangesAsync();

            return true;

        }

        public async Task CompleteTodoItem(long todoListId, long id)
        {
            var todoItem = await GetTodoItem(todoListId, id);

            todoItem.IsCompleted = true;

            _context.TodoItem.Update(todoItem);
            await _context.SaveChangesAsync();

        }

        private async Task<TodoList> FindTodoList(long todoListId)
        {
            var todoList = await _context.TodoList
                           .Include(x => x.Items)
                           .FirstOrDefaultAsync(x => x.Id == todoListId);

            if (todoList == null)
                throw new KeyNotFoundException($"Todo list with id {todoListId} not found");

            return todoList;
        }

    }
}
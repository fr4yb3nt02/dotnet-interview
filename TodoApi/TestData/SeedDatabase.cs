using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Models;

namespace TodoApi.TestData
{
    public class SeedDatabase
    {
        public static async Task SeedAsync(TodoContext context)
        {
            await context.Database.EnsureCreatedAsync();

            if (context.TodoList.Any())
            {
                Console.WriteLine("La DB ya fue seedeada.");
                return;
            }
            var todoList = new TodoList
            {
                Name = "Ejemplo con un montón de items para tarea de completar",
                Items = new List<TodoItem>()
            };

            for (int i = 1; i <= 5000; i++)
            {
                todoList.Items.Add(new TodoItem
                {
                    Title = $"Item {i}",
                    IsCompleted = false
                });
            }

            context.TodoList.Add(todoList);
            await context.SaveChangesAsync();

            Console.WriteLine("Se agrego Lista para probar la operacion de Completar Items.");
        }
    }
}
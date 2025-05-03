using Microsoft.AspNetCore.Mvc;
using TodoApi.BusinessLogic.Interfaces;
using TodoApi.Dtos;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    /// <summary>
    /// Controlador para manejar las los items Listas de tareas (TodoItems).
    /// </summary>
    [Route("api/todolists/{todoListId}/todoItems")]
    public class TodoitemsController : Controller
    {
        private readonly ITodoItemsBL _todoItemsBl;

        public TodoitemsController(ITodoItemsBL todoItemsBl)
        {
            _todoItemsBl = todoItemsBl;
        }

        /// <summary>
        /// Devuelve una lista de todos los items de una las listas de tareas.
        /// </summary>
        /// <param name="todoListId">ID del Todo List</param>
        /// <returns>Una List de TodoItem.</returns>
        [HttpGet]
        public async Task<ActionResult<IList<TodoItem>>> GetTodoItems(long todoListId)
        {
            return Ok(await _todoItemsBl.GetTodoItems(todoListId));
        }

        /// <summary>
        /// Devuelve un Item de una Lista de Tareas por su ID y el ID de la TodoList.
        /// </summary>
        /// <param name="todoListId">ID del Todo List</param>
        /// <param name="id">ID del Todo Item</param>
        /// <returns>El Item solicitado.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoList>> GetTodoItem(long todoListId, long id)
        {
            return Ok(await _todoItemsBl.GetTodoItem(todoListId, id));
        }

        /// <summary>
        /// Actualiza un Item de una lista de tareas existente.
        /// </summary>
        /// <param name="todoListId">ID del Todo List</param>
        /// <param name="id">ID del Item.</param>
        /// <param name="payload">La data para actualizar la Todo List</param>
        /// <returns> Los Todo List con sus nuevos datos. </returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> PutTodoItem(long todoListId, long id, [FromBody] UpdateTodoItem payload)
        {
            return Ok(await _todoItemsBl.UpdateTodoItem(todoListId, id, payload));
        }

        /// <summary>
        /// Crea un Item en una lista de tareas.
        /// </summary>
        /// <param name="todoListId">ID del Todo List</param>
        /// <param name="payload">La data para crear el Item</param>
        /// <returns> El Item creado. </returns>
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(long todoListId, [FromBody] CreateTodoitem payload)
        {

            var todoItemCreated = await _todoItemsBl.CreateTodoItem(todoListId, payload);

            return CreatedAtAction("GetTodoItem", new { todoListId, id = todoItemCreated.Id }, todoItemCreated);
        }

        /// <summary>
        /// Elimina un Item de una lista de tareas por su ID y el ID de la TodoList.
        /// </summary>
        /// <param name="todoListId">ID del Todo List donde se encuentra el Item</param>
        /// <param name="id"> El ID de la Todo list a eliminar.</param>
        /// <returns> No devuelve nada.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTodoItem(long todoListId, long id)
        {
            await _todoItemsBl.DeleteTodoItem(todoListId, id);
            return NoContent();
        }
    }
}
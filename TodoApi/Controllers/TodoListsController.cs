using Hangfire;
using Microsoft.AspNetCore.Mvc;
using TodoApi.BusinessLogic.Interfaces;
using TodoApi.DataAccess;
using TodoApi.Dtos;
using TodoApi.Models;


namespace TodoApi.Controllers
{
    /// <summary>
    /// Controlador para manejar las Listas de tareas (TodoLists).
    /// </summary>
    [Route("api/todolists")]
    [ApiController]
    public class TodoListsController : ControllerBase
    {
        private readonly ITodoListsBL _todoListsBl;
        private readonly ITodoItemsBL _todoItemsBl;

        public TodoListsController(ITodoListsBL todoListsBl, ITodoItemsBL todoItemsBl)
        {
            _todoListsBl = todoListsBl;
            _todoItemsBl = todoItemsBl;
        }

        /// <summary>
        /// Devuelve una lista de todas las listas de tareas (TodoLists).
        /// </summary>
        /// <returns>Una List de TodoList.</returns>
        [HttpGet]
        public async Task<ActionResult<IList<TodoList>>> GetTodoLists()
        {
            return Ok(await _todoListsBl.GetTodoLists());
        }

        /// <summary>
        /// Devuelve una lista de tareas (TodoList) por su ID.
        /// </summary>
        /// <param name="id">ID del Todo List</param>
        /// <returns>La Todo List solicitada.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoList>> GetTodoList(long id)
        {
            return Ok(await _todoListsBl.GetTodoList(id));
        }

        /// <summary>
        /// Actualiza una lista de tareas (TodoList) existente.
        /// </summary>
        /// <param name="id">ID de la Todo List.</param>
        /// <param name="payload">La data para actualizar la Todo List</param>
        /// <returns> Los Todo List con sus nuevos datos. </returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> PutTodoList(long id, UpdateTodoList payload)
        {
            return Ok(await _todoListsBl.UpdateTodoList(id, payload));
        }

        /// <summary>
        /// Crea una nueva lista de tareas (TodoList).
        /// </summary>
        /// <param name="payload">La data para crear la Todo List</param>
        /// <returns> Los Todo List creada con sus datos. </returns>
        [HttpPost]
        public async Task<ActionResult<TodoList>> PostTodoList(CreateTodoList payload)
        {
            var todoList = await _todoListsBl.CreateTodoList(payload);

            return CreatedAtAction("GetTodoList", new { id = todoList.Id }, todoList);
        }

        /// <summary>
        /// Elimina una lista de tareas (TodoList) por su ID.
        /// </summary>
        /// <param name="id"> El ID de la Todo list a eliminar.</param>
        /// <returns> No devuelve nada.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTodoList(long id)
        {
            await _todoListsBl.DeleteTodoList(id);

            return NoContent();
        }

        /// <summary>
        /// Crea una tarea para completar todos los items de una Todo List.
        /// </summary>
        /// <param name="id"> El ID de la todo list de la cual se completaran sus items.</param>
        /// <returns> No devuelve nada.</returns>
        [HttpPost("{id}/complete-all-items")]
        public ActionResult StartCompleteAllItemsTask(long id)
        {
            BackgroundJob.Enqueue(() => _todoItemsBl.CompleteAllItems(id));
            return Accepted(new { Message = "Task to complete all items has been started." });
        }


    }
}

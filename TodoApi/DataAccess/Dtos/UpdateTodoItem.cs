namespace TodoApi.Dtos;

public class UpdateTodoItem
{
    public required string Title { get; set; }
    public bool IsCompleted { get; set; }
}

namespace TodoApi.Dtos;

public class CreateTodoitem
{
    public required string Title { get; set; }
    public bool IsCompleted { get; set; }
}

using System.ComponentModel.DataAnnotations;
namespace TodoApi.Models;

public class UpdateTodoItemRequest
{
    [Required]
    [StringLength(200)]
    public required string Title { get; set; } = "";
    public bool IsCompleted { get; set; }

}
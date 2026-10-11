using System.ComponentModel.DataAnnotations;
namespace TodoApi.Models;

public class CreateTodoRequest
{
    [Required]
    [StringLength(200)]
    public string Title {get; set;} = "";
}
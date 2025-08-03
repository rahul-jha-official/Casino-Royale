using System.ComponentModel.DataAnnotations;

namespace Casino_Royale_Backend.RequestModels;

public class RegisterRequest
{
    [Required]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
    [Required]
    public string Name { get; set; } = string.Empty;
}

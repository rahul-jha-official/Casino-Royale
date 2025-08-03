namespace Casino_Royale_Backend.ResponseModels;

public class RegisterResponse
{
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public required bool IsSuccess { get; set; }
    public required string ErrorMessage { get; set; }
}
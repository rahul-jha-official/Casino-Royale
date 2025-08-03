namespace Casino_Royale_Backend.ResponseModels;

public class JoinRoomResponse
{
    public required string RoomId { get; set; }
    public required string RoomName { get; set; }
    public required int MaxPlayers { get; set; }
    public required bool IsSuccess { get; set; }
    public required string ErrorMessage { get; set; }
}

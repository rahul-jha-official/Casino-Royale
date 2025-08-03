namespace Casino_Royale_Backend.Models;

public class Player
{
    public required string RoomId { get; set; }
    public required string ConnectionId { get; set; }
    public required string Id { get; set; } 
    public required string Name { get; set; }
    public required string Email { get; set; } 

    public int CurrentBet { get; set; } = 0;
    public BidOperation CurrentBidType { get; set; }
    public int CurrentBalance { get; set; } = 0;
    public bool IsLocked { get; set; } = false;
}
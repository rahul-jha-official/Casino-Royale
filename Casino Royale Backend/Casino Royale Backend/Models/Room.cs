namespace Casino_Royale_Backend.Models;
public class Room
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required int MaxPlayers { get; set; }
    public required int TotalRounds { get; set; }
    public required int CurrentRound { get; set; }
    public int CurrentRoomCapacity { get; set; } = 0;
    public required int InitialAmount { get; set; }
    public required int PercentSeven { get; set; }
    public required int PercentOther { get; set; }
    public List<Player> Players { get; set; } = new List<Player>();
}

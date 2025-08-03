using Casino_Royale_Backend.Models;
using Casino_Royale_Backend.ResponseModels;
using Microsoft.AspNetCore.SignalR;

namespace Casino_Royale_Backend.Hubs;

public class CasinoHub : Hub
{
    public static Random RandomValueGenerator = new Random();
    public static HashSet<Room> Rooms = new HashSet<Room>();
    public async Task<RoomCreateResponse> CreateRoom(string roomName, 
        int maxPlayer, 
        int initialAmount, 
        int totalRounds,
        int percentSeven, 
        int percentOther, 
        string id, 
        string email, 
        string name)
    {
        var roomId = Guid.NewGuid().ToString();
        var response = new RoomCreateResponse
        {
            RoomId = roomId,
            RoomName = roomName,
            MaxPlayers = maxPlayer,
            InitialAmount = initialAmount,
            PercentSeven = percentSeven,
            PercentOther = percentOther,
            IsSuccess = false,
            ErrorMessage = string.Empty
        };
        if (string.IsNullOrWhiteSpace(roomId))
        {
            response.ErrorMessage = "Invalid room name.";
            return response;
        }


        var alreadyPresent = Rooms.SelectMany(e => e.Players).FirstOrDefault(e => e.Id.Equals(id) || e.Email.Equals(email));

        if (alreadyPresent is not null)
        {
            response.RoomId = alreadyPresent.RoomId;
            response.IsSuccess = true;
            return response;
        }

        Rooms.Add(new Room
        {
            Id = roomId,
            Name = roomName,
            MaxPlayers = maxPlayer,
            InitialAmount = initialAmount,
            TotalRounds = totalRounds,
            CurrentRound = 1,
            PercentSeven = percentSeven,
            PercentOther = percentOther,
            CurrentRoomCapacity = 1,
            Players = new List<Player>
            {
                new Player
                {
                    RoomId = roomId,
                    Name = name,
                    ConnectionId = Context.ConnectionId,
                    Id = id,
                    Email = email,
                    CurrentBalance = initialAmount
                }
            },
        });

        response.IsSuccess = true;

        await Groups.AddToGroupAsync(Context.ConnectionId, roomId);

        return response;
    }
    public bool IsRoomIdExists(string roomId)
    {
        if (Rooms.Any(e => e.Id.Equals(roomId)))
        {
            return true;
        }
        return false;
    }
    public async Task<JoinRoomResponse> JoinRoom(string roomId, string id, string email, string name)
    {
        var response = new JoinRoomResponse
        {
            RoomId = roomId,
            IsSuccess = false,
            ErrorMessage = string.Empty,
            RoomName = string.Empty,
            MaxPlayers = 0
        };
        var roomExists = IsRoomIdExists(roomId);
        if (roomExists)
        {
            var room = Rooms.First(e => e.Id.Equals(roomId));
            response.RoomName = room.Name;
            response.MaxPlayers = room.MaxPlayers;
            var alreadyPresent = Rooms.SelectMany(e => e.Players).FirstOrDefault(e => e.Id.Equals(id) || e.Email.Equals(email));
            if (alreadyPresent is null)
            {                
                if ((room.CurrentRoomCapacity + 1) <= room.MaxPlayers)
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, roomId);

                    room.Players.Add(new Player
                    {
                        RoomId = roomId,
                        ConnectionId = Context.ConnectionId,
                        Id = id,
                        Email = email,
                        Name = name,
                        CurrentBalance = room.InitialAmount
                    });

                    response.ErrorMessage = string.Empty;
                    response.IsSuccess = true;

                    await Clients.Group(roomId).SendAsync("fetchPlayer");
                    await Clients.Group(roomId).SendAsync("newPersonJoined", name, email);

                    room.CurrentRoomCapacity++;

                    return response;
                }
                else                 
                {
                    response.ErrorMessage = "Room is full.";
                    response.IsSuccess = false;
                    return response;
                }

            }
            else
            {
                response.IsSuccess = true;
                response.ErrorMessage = string.Empty;
                return response;
            }
        }
        else
        {
            response.ErrorMessage = "Room does not exist.";
            return response;
        }
    }
    public List<Player> GetPlayersByRoomId(string roomId)
    {
        if (IsRoomIdExists(roomId))
        {
            var room = Rooms.FirstOrDefault(e => e.Id.Equals(roomId));
            if (room is not null)
            {
                return room.Players;
            }
        }
        return [];
    }

    public int GetCurrentRound(string roomId)
    {
        return Rooms.FirstOrDefault(e => e.Id.Equals(roomId))?.CurrentRound ?? 0;
    }
    public override Task OnConnectedAsync()
    {
        return base.OnConnectedAsync();
    }
    public async Task LeaveRoom(string roomId)
    {
        if (IsRoomIdExists(roomId))
        {
            var room = Rooms.FirstOrDefault(e => e.Id.Equals(roomId));
            if (room is not null)
            {
                var player = room.Players.FirstOrDefault(e => e.ConnectionId.Equals(Context.ConnectionId));
                if (player is not null)
                {
                    room.Players.Remove(player);
                    room.CurrentRoomCapacity--;
                    if (room.Players.Count == 0)
                    {
                        Rooms.Remove(room);
                    }
                    else
                    {
                        Clients.Group(room.Id).SendAsync("fetchPlayer").GetAwaiter().GetResult();
                        Clients.Group(room.Id).SendAsync("personLeft", player.Name, player.Email).GetAwaiter().GetResult();
                    }
                }
            }
        }
    }
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var player = Rooms.SelectMany(e => e.Players).FirstOrDefault(e => e.ConnectionId.Equals(Context.ConnectionId));
        if (player is not null)
        {
            var room = Rooms.FirstOrDefault(e => e.Id.Equals(player.RoomId));
            if (room is not null)
            {
                room.Players.Remove(player);
                room.CurrentRoomCapacity--;
                if (room.Players.Count == 0)
                {
                    Rooms.Remove(room);
                }
                else
                {
                    Clients.Group(room.Id).SendAsync("fetchPlayer").GetAwaiter().GetResult();
                    Clients.Group(room.Id).SendAsync("personLeft", player.Name, player.Email).GetAwaiter().GetResult();
                }
            }
        }
        return base.OnDisconnectedAsync(exception);
    }

    public async Task<BidLockResponse> LockYourBidding(string roomId, string id, string email, int lockedValue, BidOperation operation)
    {
        var response = new BidLockResponse
        {
            IsSuccess = false,
            ErrorMessage = string.Empty
        };
        if (IsRoomIdExists(roomId))
        {
            var room = Rooms.First(e => e.Id.Equals(roomId));
            var player = room.Players.FirstOrDefault(e => e.Id.Equals(id) || e.Email.Equals(email));
            if (player is not null)
            {
                if (player.CurrentBalance < lockedValue || lockedValue < 0)
                {
                    response.ErrorMessage = "Insufficient balance to lock the bidding.";
                    response.IsSuccess = false;
                    return response;
                }
                player.IsLocked = true;
                player.CurrentBet = lockedValue;
                player.CurrentBidType = operation;
                response.IsSuccess = true;
                response.ErrorMessage = string.Empty;
                await Clients.Group(roomId).SendAsync("fetchPlayer");
                await Clients.Caller.SendAsync("lockBidding");

                if (room.CurrentRoomCapacity > 1 && room.Players.All(e => e.IsLocked))
                {
                    var dice1 = RandomValueGenerator.Next(1, 6);
                    var dice2 = RandomValueGenerator.Next(1, 6);
                    await Clients.Group(roomId).SendAsync("rollDice", dice1, dice2);

                    var total = dice1 + dice2;
                    var operationType = total switch
                    {
                        < 7 => BidOperation.LessThanSeven,
                        7 => BidOperation.EqualToSeven,
                        _ => BidOperation.GreaterThanSeven
                    };
                    foreach (var p in room.Players)
                    {
                        if (p.CurrentBidType == operationType)
                        {
                            p.CurrentBalance += p.CurrentBet;
                            if (p.CurrentBidType == BidOperation.EqualToSeven)
                            {
                                p.CurrentBalance += (int)(p.CurrentBet * room.PercentSeven * 0.01);
                            }
                            else
                            {
                                p.CurrentBalance += (int)(p.CurrentBet * room.PercentOther * 0.01);
                            }
                        }
                        else
                        {
                            p.CurrentBalance -= p.CurrentBet;
                        }
                        p.IsLocked = false;
                        p.CurrentBet = 0;
                    }
                    await Clients.Group(roomId).SendAsync("fetchPlayer");
                    if (room.CurrentRound < room.TotalRounds) await Clients.Group(roomId).SendAsync("unlockBidding");
                    else
                    {
                        await Clients.Group(roomId).SendAsync("fetchPlayer");
                        await Task.Delay(2000);
                        await Clients.Group(roomId).SendAsync("gameOver", room.Players);
                        Rooms.Remove(room);
                    }
                    room.CurrentRound++;
                    await Clients.Group(roomId).SendAsync("setCurrentRound", room.CurrentRound);
                }

                return response;
            }
        }
        return response;
    }
}

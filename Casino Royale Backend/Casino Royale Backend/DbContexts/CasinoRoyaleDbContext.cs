using Casino_Royale_Backend.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Casino_Royale_Backend.DbContexts;

public class CasinoRoyaleDbContext : IdentityDbContext<User>
{
    public CasinoRoyaleDbContext(DbContextOptions<CasinoRoyaleDbContext> options) : base(options)
    {
    }
}

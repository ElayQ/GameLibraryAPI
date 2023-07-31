using GameLibraryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GameLibraryAPI.Data;

public class GameLibraryContext : DbContext
{
    public GameLibraryContext(DbContextOptions<GameLibraryContext> options) : base(options)
    {
    }
    
    public DbSet<GameLibrary> Games { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<GameLibrary>();
    }
}
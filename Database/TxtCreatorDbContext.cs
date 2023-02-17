using Microsoft.EntityFrameworkCore;
using TxtCreatorBOT.Database.Models;

namespace TxtCreatorBOT.Database;

public class TxtCreatorDbContext : DbContext
{
    public DbSet<UserModel> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=txtcreatorbot.db");
    }
}
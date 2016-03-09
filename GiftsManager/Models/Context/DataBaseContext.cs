using System.Data.Entity;

namespace GiftsManager.Models.Context
{
    // Update-Database -TargetMigration:0 -force --> Revert DB

    public class DataBaseContext : DbContext
    {
        public DataBaseContext() : base("DataBaseContext")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Gift> Gifts { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Event> Events { get; set; }
    }
}
using Microsoft.EntityFrameworkCore;
using timesheet_calculation.Data.Entities;

namespace timesheet_calculation.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<im_TimeSheet> TimeSheets { get; set; } = null!;
    public DbSet<im_TimeSheetManager?> TimeSheetManagers { get; set; } = null!;
    public DbSet<im_User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //Users
        modelBuilder.Entity<im_User>().HasData(new im_User
        {
            UserId = Guid.Parse("d29a38b9566c4c95a3802351796764f1"),
            Name = "Tuan Anh"
        });
        
        //TimeSheet
        // 5/8/2022
        modelBuilder.Entity<im_TimeSheet>().HasData(new im_TimeSheet
        {
            Id = Guid.Parse("e688b871-464c-4015-88f9-bd1cde3ebdfe"),
            CheckInTime = new DateTime(2022, 8, 8, 7, 59, 50, DateTimeKind.Utc),
            CheckOutTime = new DateTime(2022, 8, 8, 17, 31, 18, DateTimeKind.Utc),
            UserId = Guid.Parse("d29a38b9566c4c95a3802351796764f1")
        });
        
        // 8/8/2022
        modelBuilder.Entity<im_TimeSheet>().HasData(new im_TimeSheet
        {
            Id = Guid.Parse("bd002b36-daf2-4208-b3fb-132985d41eae"),
            CheckInTime = new DateTime(2022, 8, 8, 7, 58, 14, DateTimeKind.Utc),
            CheckOutTime = new DateTime(2022, 8, 8, 17, 32, 46, DateTimeKind.Utc),
            UserId = Guid.Parse("d29a38b9566c4c95a3802351796764f1")
        });
    }
}
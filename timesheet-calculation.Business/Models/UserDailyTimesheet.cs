namespace timesheet_calculation.Business.Models;

public class UserDailyTimesheet
{
    public Guid UserId { get; set; }
    public int Day { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public bool IsLate { get; set; }
    public int TotalLateInSeconds { get; set; }
    public int TotalActualWorkingTimeInSeconds { get; set; }
    public string Status { get; set; } = null!;
}
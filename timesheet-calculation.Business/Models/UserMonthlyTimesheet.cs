namespace timesheet_calculation.Business.Models;

public class UserMonthlyTimesheet
{
    public Guid UserId { get; set; }
    public int TotalWorkingHours { get; set; }
    public int TotalActualWorkingHours { get; set; }
    public List<UserDailyTimesheet> DailyTimesheets { get; set; } = null!;
}
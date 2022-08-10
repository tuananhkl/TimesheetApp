namespace timesheet_calculation.Data.Dtos;

public class CreateTimeSheetDto
{
    //public Guid UserId { get; set; }
    public DateTime CheckInTime { get; set; }
    public DateTime CheckOutTime { get; set; }
}

public class TimeSheetDto
{
    public Guid Id { get; set; }
    public DateTime? CheckInTime { get; set; }
    public DateTime? CheckOutTime { get; set; }
}
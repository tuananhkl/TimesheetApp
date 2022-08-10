using Type = timesheet_calculation.Data.Entities.Type;

namespace timesheet_calculation.Data.Dtos;

public class TimeSheetManagerDto : UpdateTimeSheetDto
{
    public string DateTime { get; set; }
}

public class UpdateTimeSheetDto
{
    public int Type { get; set; }
    public string Note { get; set; } = null!;
}
using timesheet_calculation.Data.Entities;
using Type = timesheet_calculation.Data.Entities.Type;

namespace timesheet_calculation.Common.Utilities;

public static class TimeSheetManagerConversion
{
    public static void ConvertToTimeSheetManager(this DateTime date, Type dateType, string dateNote, im_TimeSheetManager? timeSheetManager)
    {
        timeSheetManager.Id = Guid.NewGuid();
        timeSheetManager.Day = date.Day;
        timeSheetManager.Month = date.Month;
        timeSheetManager.Year = date.Year;
        timeSheetManager.Type = (int) dateType;
        timeSheetManager.Note = dateNote;
    }
}
using timesheet_calculation.Data.Dtos;
using timesheet_calculation.Data.Entities;
using X.PagedList;
using Type = timesheet_calculation.Data.Entities.Type;

namespace timesheet_calculation.Common.Utilities;

public static class DtoConversion
{
    public static IEnumerable<TimeSheetManagerDto> ConvertToDto(this IEnumerable<im_TimeSheetManager> timeSheetManagers)
    {
        return (from timeSheetManager in timeSheetManagers
                select new TimeSheetManagerDto
                {
                    DateTime = new DateTime(timeSheetManager.Year, timeSheetManager.Month, timeSheetManager.Day).ToString("dd-MM-yyyy"),
                    Type = timeSheetManager.Type,
                    // Type = Enum.GetName(typeof(Type), timeSheetManager.Type),
                    Note = timeSheetManager.Note
                }
            );
    }
    
    public static IPagedList<TimeSheetManagerDto> ConvertToDto(this IPagedList<im_TimeSheetManager?> timeSheetManagers)
    {
        return (from timeSheetManager in timeSheetManagers
                select new TimeSheetManagerDto
                {
                    DateTime = new DateTime(timeSheetManager.Year, timeSheetManager.Month, timeSheetManager.Day).ToString("dd-MM-yyyy"),
                    Type = timeSheetManager.Type,
                    // Type = Enum.GetName(typeof(Type), timeSheetManager.Type),
                    Note = timeSheetManager.Note
                }
            );
    }

    public static im_TimeSheet ConvertToDto(this CreateTimeSheetDto dto)
    {
        return new im_TimeSheet
        {
            Id = Guid.NewGuid(),
            CheckInTime = dto.CheckInTime,
            CheckOutTime = dto.CheckOutTime
        };
    }

    public static IEnumerable<TimeSheetDto> ConvertToDto(this IEnumerable<im_TimeSheet> timeSheets)
    {
        return (from timeSheet in timeSheets
            select new TimeSheetDto
            {
                Id = timeSheet.Id,
                CheckInTime = timeSheet.CheckInTime,
                CheckOutTime = timeSheet.CheckOutTime
            });
    }
}
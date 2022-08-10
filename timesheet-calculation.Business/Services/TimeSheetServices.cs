using Microsoft.EntityFrameworkCore;
using timesheet_calculation.Business.Models;
using timesheet_calculation.Data;
using timesheet_calculation.Data.Dtos;
using timesheet_calculation.Data.Entities;

namespace timesheet_calculation.Business.Services;

public class TimeSheetServices
{
    private readonly AppDbContext _dbContext;
    private readonly ITimeSheetManagerServices _timeSheetManagerServices;
    
    public TimeSheetServices(AppDbContext dbContext, ITimeSheetManagerServices timeSheetManagerServices)
    {
        _dbContext = dbContext;
        _timeSheetManagerServices = timeSheetManagerServices;
    }

    public async Task<im_TimeSheet> CreateTimSheet(im_TimeSheet timeSheet)
    {
        await _dbContext.TimeSheets.AddAsync(timeSheet);
        await _dbContext.SaveChangesAsync();

        return timeSheet;
    }

    public async Task<ICollection<im_TimeSheet>?> GetTimeSheetByUserId(Guid userId)
    {
        var result = _dbContext.TimeSheets.Where(t => t.UserId == userId);

        return await result.AsNoTracking().ToListAsync();
    }

    public Task<UserDailyTimesheet> GetUserId(im_TimeSheet timeSheet)
    {
        var userDailyTimesheet = new UserDailyTimesheet
        {
            UserId = timeSheet.UserId
        };

        return Task.FromResult(userDailyTimesheet);
    }

    public Task<UserDailyTimesheet> GetDateInfo(im_TimeSheet timeSheet)
    {
        var userDailyTimesheet = new UserDailyTimesheet
        {
            Day = timeSheet.CheckInTime.GetValueOrDefault().Day,
            Month = timeSheet.CheckInTime.GetValueOrDefault().Month,
            Year = timeSheet.CheckInTime.GetValueOrDefault().Year
        };

        if (timeSheet.CheckInTime is null)
        {
            userDailyTimesheet.Day = 0;
            userDailyTimesheet.Month = 0;
            userDailyTimesheet.Year = 0;
        }
        
        return Task.FromResult(userDailyTimesheet);
    } 

    public Task<UserDailyTimesheet> CalculateTotalLateInSecondsAndIsLate(im_TimeSheet timeSheet)
    {
        var userDailyTimesheet = new UserDailyTimesheet();
        var workCheckInTime = new DateTime(timeSheet.CheckInTime.GetValueOrDefault().Year,
            timeSheet.CheckInTime.GetValueOrDefault().Month,
            timeSheet.CheckInTime.GetValueOrDefault().Day,
            8,30,00);

        if (timeSheet.CheckInTime > workCheckInTime)
        {
            userDailyTimesheet.IsLate = true;
            userDailyTimesheet.TotalLateInSeconds = (int) (timeSheet.CheckInTime - workCheckInTime).GetValueOrDefault().TotalSeconds;
        }
        else
        {
            userDailyTimesheet.IsLate = false;
            userDailyTimesheet.TotalLateInSeconds = 0;
        }
        
        return Task.FromResult(userDailyTimesheet);
    }

    public Task<UserDailyTimesheet> GetTotalActualWorkingTimeInSeconds(im_TimeSheet timeSheet)
    {
        var workMorningEndingTime = new DateTime(timeSheet.CheckInTime.GetValueOrDefault().Year,
            timeSheet.CheckInTime.GetValueOrDefault().Month,
            timeSheet.CheckInTime.GetValueOrDefault().Day,
            12,00,00);
        var workAfternoonStartingTime = new DateTime(timeSheet.CheckInTime.GetValueOrDefault().Year,
            timeSheet.CheckInTime.GetValueOrDefault().Month,
            timeSheet.CheckInTime.GetValueOrDefault().Day,
            13,30,00);
        var workEndTime = new DateTime(timeSheet.CheckInTime.GetValueOrDefault().Year,
            timeSheet.CheckInTime.GetValueOrDefault().Month,
            timeSheet.CheckInTime.GetValueOrDefault().Day,
            18,00,00);
        var endOfDay = new DateTime(timeSheet.CheckInTime.GetValueOrDefault().Year,
            timeSheet.CheckInTime.GetValueOrDefault().Month,
            timeSheet.CheckInTime.GetValueOrDefault().Day,
            23,59,59);

        int total = 0;
        
        if (timeSheet.CheckInTime.GetValueOrDefault() >= workMorningEndingTime &&
            timeSheet.CheckInTime.GetValueOrDefault() < workAfternoonStartingTime)
        {
            total = (int) (timeSheet.CheckOutTime - workAfternoonStartingTime).GetValueOrDefault().TotalSeconds;
        }
        else if(workAfternoonStartingTime <= timeSheet.CheckInTime.GetValueOrDefault() && timeSheet.CheckInTime.GetValueOrDefault() < workEndTime)
        {
            total = (int) (timeSheet.CheckOutTime - timeSheet.CheckInTime).GetValueOrDefault().TotalSeconds;
        }
        else if (workEndTime <= timeSheet.CheckInTime.GetValueOrDefault() &&
                 timeSheet.CheckInTime.GetValueOrDefault() <= endOfDay)
        {
            total = 0;
        }
        else
        {
            var totalMorningWorkTimeInSeconds = (workMorningEndingTime - timeSheet.CheckInTime).GetValueOrDefault().TotalSeconds;
            var totalAfternoonWorkTimeInSeconds = (timeSheet.CheckOutTime - workAfternoonStartingTime).GetValueOrDefault().TotalSeconds;

            total = (int) (totalMorningWorkTimeInSeconds + totalAfternoonWorkTimeInSeconds);
        }

        var userDailyTimesheet = new UserDailyTimesheet
        {
            TotalActualWorkingTimeInSeconds = total
        };
        
        if(timeSheet.CheckInTime is null || timeSheet.CheckOutTime is null)
        {
            userDailyTimesheet.TotalActualWorkingTimeInSeconds = 0;
        }
        
        return Task.FromResult(userDailyTimesheet);
    }

    public Task<UserDailyTimesheet> GetStatus(im_TimeSheet timeSheet)
    {
        var workEndTime = new DateTime(timeSheet.CheckInTime.GetValueOrDefault().Year,
            timeSheet.CheckInTime.GetValueOrDefault().Month,
            timeSheet.CheckInTime.GetValueOrDefault().Day,
            18,00,00);
        
        var userDailyTimesheet = new UserDailyTimesheet();

        var totalActualWorkingTimeInSeconds =
            GetTotalActualWorkingTimeInSeconds(timeSheet).Result.TotalActualWorkingTimeInSeconds;
        const int eightHoursInSeconds = 8 * 60 * 60;
        
        /* Logic
         - Ngày nào là ngày làm việc, có checkin và checkout đầy đủ, không muộn, thời gian làm việc trên 8h thì trạng thái là VALID
           - Ngày nào là ngày làm việc, có checkin và checkout đầy đủ, thời gian làm việc < 8h thì trạng thái là INCOMPLETE
           - Ngày nào là ngày làm việc, có checkin mà không có checkout thì trạng thái là INPROCESS
           - Ngày nào là ngày làm việc, không có checkin thì trạng thái là ABSENT 
           */
        if (timeSheet.CheckInTime is not null && timeSheet.CheckOutTime is not null &&
            totalActualWorkingTimeInSeconds >= eightHoursInSeconds && CalculateTotalLateInSecondsAndIsLate(timeSheet).Result.IsLate == false)
        {
            userDailyTimesheet.Status = "VALID";
        }
        else if ((timeSheet.CheckInTime is not null && timeSheet.CheckOutTime is not null &&
                totalActualWorkingTimeInSeconds < eightHoursInSeconds) || (timeSheet.CheckInTime is not null && timeSheet.CheckOutTime is not null &&
                totalActualWorkingTimeInSeconds >= eightHoursInSeconds && CalculateTotalLateInSecondsAndIsLate(timeSheet).Result.IsLate == true))
        {
            userDailyTimesheet.Status = "INCOMPLETE";
        }
        else if (timeSheet.CheckInTime is not null && timeSheet.CheckOutTime is null)
        {
            userDailyTimesheet.Status = "INPROCESS";
        }
        else if(timeSheet.CheckInTime is null || timeSheet.CheckInTime.GetValueOrDefault() > workEndTime)
        {
            userDailyTimesheet.Status = "ABSENT";
        }
       
        return Task.FromResult(userDailyTimesheet);
    }

    public string CheckWorkDayOrNot(CreateTimeSheetDto dto)
    {
        var dtoDay = dto.CheckInTime.Day;
        var dtoMonth = dto.CheckInTime.Month;
        var dtoYear = dto.CheckInTime.Year;
        
         //lay im_TimeSheetManager.cs theo dto.Day,Month,Year
         var timeSheetManager = _timeSheetManagerServices.GetADay(dtoYear, dtoMonth, dtoDay).Result;

         if (timeSheetManager is not null)
         {
             return timeSheetManager.Type switch
             {
                 1 => "dayoff",
                 2 => "holiday",
                 _ => "workday"
             };
         }

         return "null";
    }
}
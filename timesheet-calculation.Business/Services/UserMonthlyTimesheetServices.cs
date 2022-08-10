using timesheet_calculation.Business.Models;
using timesheet_calculation.Data.Entities;

namespace timesheet_calculation.Business.Services;

public class UserMonthlyTimesheetServices
{
    //private readonly AppDbContext _dbContext;
    private readonly ITimeSheetManagerServices _timeSheetManagerServices;
    private readonly TimeSheetServices _timeSheetServices;

    public UserMonthlyTimesheetServices(ITimeSheetManagerServices timeSheetManagerServices, TimeSheetServices timeSheetServices)
    {
        //_dbContext = dbContext;
        _timeSheetManagerServices = timeSheetManagerServices;
        _timeSheetServices = timeSheetServices;
    }
    
    // field TotalWorkingHours = tổng số ngày làm việc * 8
    // field TotalActualWorkingHours = tổng số giờ làm việc thực tế
    
    // tinh so ngay lam viec 1 thang (29 ngay/ 30 ngay/ 31 ngay)
    private int CalculateDaysInAMonth(int year, int month)
    {
        //lay TimeSheetManager theo type = 0
        var daysCount = _timeSheetManagerServices.GetDays(year, month, null, null)
            .Result.Count(t => t is {Type: 0});

        return daysCount;
    }

    // public Task<UserMonthlyTimesheet> GetTotalWorkingHoursInAMonth(int year, int month)
    // {
    //     var doyCounts = CalculateDaysInAMonth(year, month);
    //
    //     var totalActualWorkingHours = doyCounts * 8;
    //
    //     var userMonthlyTimesheet = new UserMonthlyTimesheet
    //     {
    //         TotalWorkingHours = totalActualWorkingHours
    //     };
    //     
    //     return Task.FromResult(userMonthlyTimesheet);
    // }
    
    public int GetTotalActualWorkingHoursInADay(im_TimeSheet timeSheet)
    {
        var workMorningEndingTime = new DateTime(2022, 8,9,12,00,00);
        var workAfternoonStartingTime = new DateTime(2022, 8,9,13,30,00);
        

        var totalMorningWorkTimeInHours = (workMorningEndingTime - timeSheet.CheckInTime).GetValueOrDefault().TotalHours;
        var totalAfternoonWorkTimeInSHours = (timeSheet.CheckOutTime - workAfternoonStartingTime).GetValueOrDefault().TotalHours;

        var totalActualWorkingHoursInADay = (int) (totalMorningWorkTimeInHours + totalAfternoonWorkTimeInSHours);


        return totalActualWorkingHoursInADay;
    }

    private int CalculateWorkingDaysInAMonth(int year, int month, Guid userId)
    {
        var daysCount = 0;
        
        var imTimeSheets = _timeSheetServices.GetTimeSheetByUserId(userId).Result;
        if (imTimeSheets != null)
        {
            daysCount = imTimeSheets.Count();
        }

        return daysCount;
    }
    
    public Task<UserMonthlyTimesheet> GetTotalWorkingHoursInAMonth(int year, int month, Guid userId)
    {
        var doyCounts = CalculateWorkingDaysInAMonth(year, month, userId);

        var totalActualWorkingHours = doyCounts * 8;

        var userMonthlyTimesheet = new UserMonthlyTimesheet
        {
            TotalWorkingHours = totalActualWorkingHours
        };
        
        return Task.FromResult(userMonthlyTimesheet);
    }
}
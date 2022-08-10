using System.Globalization;
using Microsoft.EntityFrameworkCore;
using timesheet_calculation.Business.Models;
using timesheet_calculation.Common.Utilities;
using timesheet_calculation.Data;
using timesheet_calculation.Data.Dtos;
using timesheet_calculation.Data.Entities;
using X.PagedList;
using Type = timesheet_calculation.Data.Entities.Type;

namespace timesheet_calculation.Business.Services;

public class TimeSheetManagerServices : ITimeSheetManagerServices
{
    private readonly AppDbContext _dbContext;

    public TimeSheetManagerServices(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IPagedList<im_TimeSheetManager?>> GetDays(int year, int? month, int? day, 
        RequestParams? requestParams)
    {
        IQueryable<im_TimeSheetManager?> result;
        
        if (month is null && day is null)
        {
            result = _dbContext.TimeSheetManagers.Where(t => t.Year == year);
        }
        else if (day is null)
        {
            result = _dbContext.TimeSheetManagers.Where(t => t.Year == year && t.Month == month);
        }
        else
        {
            result = _dbContext.TimeSheetManagers.Where(t 
                => t.Year == year && t.Month == month && t.Day == day);
        }

        if (requestParams != null)
            return await result.ToPagedListAsync(requestParams.PageNumber, requestParams.PageSize);
        
        return await result.ToPagedListAsync(1, 100); 
    }

    public async Task<im_TimeSheetManager?> GetADay(int year, int month, int day)
    {
        var result = await _dbContext.TimeSheetManagers.FirstOrDefaultAsync(t 
            => t.Year == year && t.Month == month && t.Day == day);

        return result;
    }
    
    public async Task UpdateADay(im_TimeSheetManager? timeSheetManager)
    {
        _dbContext.TimeSheetManagers.Update(timeSheetManager);
        await _dbContext.SaveChangesAsync();
    }

    // public async Task UpdateADay(int year, int month, int day, UpdateTimeSheetDto dto)
    // {
    //     var aDay = _dbContext.TimeSheetManagers.FirstOrDefault(t 
    //         => t.Year == year && t.Month == month && t.Day == day);
    //     
    //     if (aDay is not null)
    //     {
    //         aDay.Type = dto.Type;
    //         aDay.Note = dto.Note;
    //     }
    // }

    // public async Task<IPagedList<im_TimeSheetManager>> GetDaysInAParticularMonth(int year, int month, RequestParams requestParams)
    // {
    //     var monthDays = await _dbContext.TimeSheetManagers.Where(t => t.Year == year && t.Month == month).ToListAsync();
    //
    //     return await monthDays.ToPagedListAsync(requestParams.PageNumber, requestParams.PageSize);
    // }
    //
    // public async Task<IEnumerable<im_TimeSheetManager>> GetDaysInParticularYear(int year)
    // {
    //     var days = await _dbContext.TimeSheetManagers.Where(t => t.Year == year).ToListAsync();
    //
    //     return days;
    // }
    // public async Task<IPagedList<im_TimeSheetManager>> GetDaysInParticularYear(int year,
    //     RequestParams requestParams)
    // {
    //     var days = await _dbContext.TimeSheetManagers.Where(t => t.Year == year).ToListAsync();
    //
    //     return await days.ToPagedListAsync(requestParams.PageNumber, requestParams.PageSize);
    // }

    public async Task CreateDaysForParticularYear(int year)
    {
        var date = new DateTime(year, 1, 1);
        var lunarDate = new ChineseLunisolarCalendar();

        var isFirstSaturdayWorking = false;

        while (date.Year == year)
        {
            var timeSheetManager = new im_TimeSheetManager();
            
            //Holidays
            if (TetDuongLich(date))
            {
                date.ConvertToTimeSheetManager(Type.Holiday, "Tet Duong Lich", timeSheetManager);
                /*
                timeSheetManager.Id = Guid.NewGuid();
                timeSheetManager.Day = date.Day;
                timeSheetManager.Month = date.Month;
                timeSheetManager.Year = year;
                timeSheetManager.Type = (int) Type.Holiday;
                timeSheetManager.Note = "Tet Duong Lich";
                */
            }
            else if (GiaiPhongMienNam(date))
            {
                date.ConvertToTimeSheetManager(Type.Holiday, "Giai Phong Mien Nam", timeSheetManager);
            }
            else if (QuocTeLaoDong(date))
            {
                date.ConvertToTimeSheetManager(Type.Holiday, "Quoc Te Lao Dong", timeSheetManager);
            }
            else if (QuocKhanh(date))
            {
                date.ConvertToTimeSheetManager(Type.Holiday, "Quoc Khanh", timeSheetManager);
            }
            else if (CheckLunarNewYearDays(lunarDate, date) is true)
            {
                date.ConvertToTimeSheetManager(Type.Holiday, "Tet Nguyen Dan", timeSheetManager);
            }
            else if (GioToHungVuong(date, lunarDate))
            {
                date.ConvertToTimeSheetManager(Type.Holiday, "Gio To Hung Vuong", timeSheetManager);
            }
            else if (Sunday(date))
            {
                date.ConvertToTimeSheetManager(Type.DayOff, "Sunday Day-Off", timeSheetManager);
            }
            else if (SaturdayDayOff(date, isFirstSaturdayWorking))
            {
                date.ConvertToTimeSheetManager(Type.DayOff, "Saturday Day-Off", timeSheetManager);
                isFirstSaturdayWorking = true;
            }
            else if (SaturdayWorkDay(date, isFirstSaturdayWorking))
            {
                date.ConvertToTimeSheetManager(Type.WorkDay, "Saturday WorkDay", timeSheetManager);
                isFirstSaturdayWorking = false;
            }
            else
            {
                date.ConvertToTimeSheetManager(Type.WorkDay, "Normal Workday", timeSheetManager);
            }

            await _dbContext.TimeSheetManagers.AddAsync(timeSheetManager);
            await _dbContext.SaveChangesAsync();

            date = date.AddDays(1);
        }
    }

    private bool TetDuongLich(DateTime date) => date.Day == 1 && date.Month == 1;

    private bool GiaiPhongMienNam(DateTime date) => date.Day is 30 && date.Month is 4;
    private bool QuocTeLaoDong(DateTime date) => date.Day is 1 && date.Month is 5;
    private bool QuocKhanh(DateTime date) => date.Day is 2 && date.Month is 9;
    
    private bool CheckLunarNewYearDays(ChineseLunisolarCalendar lunarDate, DateTime date)
    {
        if (lunarDate.GetDayOfYear(date) is 31 && lunarDate.GetMonthsInYear(date.Year) is 12 &&
            lunarDate.IsLeapYear(date.Year) is false)
        {
            return true;
        }

        if (lunarDate.GetDayOfYear(date) is 31 && lunarDate.GetMonthsInYear(date.Year) is 13 &&
            lunarDate.IsLeapYear(date.Year) is true)
        {
            return true;
        }

        if (lunarDate.GetMonthsInYear(date.Year) is 1)
        {
            switch (lunarDate.GetDayOfYear(date))
            {
                case 1:
                case 2:
                case 3:
                case 4:
                    return true;
                default:
                    return false;
            }
        }
        
        return false;
    }

    private bool GioToHungVuong(DateTime date, ChineseLunisolarCalendar lunarDate) =>
        lunarDate.GetDayOfYear(date) is 10 && lunarDate.GetMonthsInYear(date.Year) is 3;

    private bool Sunday(DateTime date) => date.DayOfWeek is DayOfWeek.Sunday;
    private bool SaturdayDayOff(DateTime date, bool isFirstSaturdayWorking) => 
        date.DayOfWeek is DayOfWeek.Saturday && isFirstSaturdayWorking is false;

    private bool SaturdayWorkDay(DateTime date, bool isFirstSaturdayWorking) =>
        date.DayOfWeek is DayOfWeek.Saturday && isFirstSaturdayWorking is true;
}
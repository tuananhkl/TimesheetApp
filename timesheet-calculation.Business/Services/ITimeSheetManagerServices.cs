using timesheet_calculation.Business.Models;
using timesheet_calculation.Data.Dtos;
using timesheet_calculation.Data.Entities;
using X.PagedList;

namespace timesheet_calculation.Business.Services;

public interface ITimeSheetManagerServices
{
    Task CreateDaysForParticularYear(int year);
    //Task<IEnumerable<im_TimeSheetManager>> GetDaysInParticularYear(int year);
    // Task<IPagedList<im_TimeSheetManager>> GetDaysInParticularYear(int year, RequestParams requestParams);
    // Task<IPagedList<im_TimeSheetManager>> GetDaysInAParticularMonth(int year, int month, RequestParams requestParams);
    Task<IPagedList<im_TimeSheetManager?>> GetDays(int year, int? month, int? day, RequestParams? requestParams);
    Task<im_TimeSheetManager?> GetADay(int year, int month, int day);
    Task UpdateADay(im_TimeSheetManager? timeSheetManager);
}
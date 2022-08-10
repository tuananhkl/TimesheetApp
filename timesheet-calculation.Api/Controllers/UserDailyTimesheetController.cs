using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using timesheet_calculation.Business.Models;
using timesheet_calculation.Business.Services;
using timesheet_calculation.Data;

namespace timesheet_calculation.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDailyTimesheetController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly TimeSheetServices _timeSheetServices;

        public UserDailyTimesheetController(AppDbContext dbContext, TimeSheetServices timeSheetServices)
        {
            _dbContext = dbContext;
            _timeSheetServices = timeSheetServices;
        }
        
        [HttpGet("{userId:Guid}")]
        public async Task<ActionResult> GetUserDailyTimesheet(Guid userId)
        {
            var timesheets = await _timeSheetServices.GetTimeSheetByUserId(userId);

            if (timesheets is null)
            {
                return NotFound();
            }

            ICollection<UserDailyTimesheet> resultList = new List<UserDailyTimesheet>();
            foreach (var timesheet in timesheets)
            {
                var userDailyTimesheet = new UserDailyTimesheet
                {
                    UserId = userId,
                    Day = _timeSheetServices.GetDateInfo(timesheet).Result.Day,
                    Month = _timeSheetServices.GetDateInfo(timesheet).Result.Month,
                    Year = _timeSheetServices.GetDateInfo(timesheet).Result.Year,
                    IsLate = _timeSheetServices.CalculateTotalLateInSecondsAndIsLate(timesheet).Result.IsLate,
                    TotalLateInSeconds = _timeSheetServices.CalculateTotalLateInSecondsAndIsLate(timesheet).Result.TotalLateInSeconds,
                    TotalActualWorkingTimeInSeconds = _timeSheetServices.GetTotalActualWorkingTimeInSeconds(timesheet)
                        .Result.TotalActualWorkingTimeInSeconds,
                    Status = _timeSheetServices.GetStatus(timesheet).Result.Status
                };
                
                resultList.Add(userDailyTimesheet);
            }

            return Ok(resultList);
        }
    }
}

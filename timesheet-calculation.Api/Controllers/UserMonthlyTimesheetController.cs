using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using timesheet_calculation.Business.Models;
using timesheet_calculation.Business.Services;

namespace timesheet_calculation.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserMonthlyTimesheetController : ControllerBase
    {
        private readonly UserMonthlyTimesheetServices _userMonthlyTimesheetServices;
        private readonly TimeSheetServices _timeSheetServices;

        public UserMonthlyTimesheetController(UserMonthlyTimesheetServices timeSheetManagerServices, TimeSheetServices timeSheetServices)
        {
            _userMonthlyTimesheetServices = timeSheetManagerServices;
            _timeSheetServices = timeSheetServices;
        }

        [HttpGet]
        public async Task<ActionResult> GetUserMonthlyTimesheet(Guid userId, int year, int month)
        {
            var timesheets = await _timeSheetServices.GetTimeSheetByUserId(userId);
            if (timesheets is null)
            {
                return NotFound();
            }

            var totalActualWorkingHoursInAMonth = 0;
            foreach (var timeSheet in timesheets)
            {
                totalActualWorkingHoursInAMonth +=
                    _userMonthlyTimesheetServices.GetTotalActualWorkingHoursInADay(timeSheet);
            }

            var userMonthlyTimesheet = new UserMonthlyTimesheet
            {
                UserId = userId,
                TotalWorkingHours = _userMonthlyTimesheetServices.GetTotalWorkingHoursInAMonth(year, month, userId).Result.TotalWorkingHours,
                TotalActualWorkingHours = totalActualWorkingHoursInAMonth
            };
            
            return Ok(userMonthlyTimesheet);
        }
    }
}

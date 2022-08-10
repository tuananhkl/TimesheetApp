using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using timesheet_calculation.Business.Models;
using timesheet_calculation.Business.Services;
using timesheet_calculation.Common.Utilities;
using timesheet_calculation.Data;
using timesheet_calculation.Data.Dtos;
using timesheet_calculation.Data.Entities;
using X.PagedList;

namespace timesheet_calculation.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeSheetManagerController : ControllerBase
    {
        private readonly ITimeSheetManagerServices _timeSheetManagerServices;
        private readonly AppDbContext _dbContext;

        public TimeSheetManagerController(ITimeSheetManagerServices timeSheetManagerServices, AppDbContext dbContext)
        {
            _timeSheetManagerServices = timeSheetManagerServices;
            _dbContext = dbContext;
        }

        /// <summary>
        /// Post days information for a year
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        [HttpPost("{year:int}")]
        public async Task<ActionResult> CreateDaysInAParticularYear(int year)
        {
            var yearExisted = _dbContext.TimeSheetManagers.FirstOrDefault(t => t.Year == year);
            if (yearExisted is not null)
            {
                return BadRequest("That year is already exists");
            }
            
            await _timeSheetManagerServices.CreateDaysForParticularYear(year);

            return StatusCode(204, "Created.");
        }

        [HttpGet]
        public async Task<ActionResult> GetDays(int year, int? month, int? day,[FromQuery] RequestParams? requestParams)
        {
            if ((year < 2022) || month is < 0 or > 12 || day is < 0 or > 31 || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var days = await _timeSheetManagerServices.GetDays(year, month, day, requestParams);

            var daysDto = days.ConvertToDto();

            return Ok(daysDto);
        }

        
        /// <summary>
        /// Type: 0 -> Workday, 1 -> DayOff, 2 -> Holiday
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        
        [HttpPatch]
        public async Task<ActionResult> UpdateAParticularDay(int year, int month, int day,
            [FromBody] UpdateTimeSheetDto dto)
        {
            var aDay = await _timeSheetManagerServices.GetADay(year, month, day);

            if (aDay is null)
            {
                return NotFound($"The day {year}/{month}/{day} is not found");
            }
            
            aDay.Type = dto.Type;
            aDay.Note = dto.Note;

            await _timeSheetManagerServices.UpdateADay(aDay);
            
            return NoContent();
        }










        // [HttpGet("{year:int}")]
        // public async Task<ActionResult> GetDaysInParticularYear(int year, [FromQuery] RequestParams requestParams)
        // {
        //     if (year < 2020 || !ModelState.IsValid)
        //     {
        //         return BadRequest(ModelState);
        //     }
        //     
        //     var days = await _timeSheetManagerServices.GetDaysInParticularYear(year, requestParams);
        //
        //     var daysDto = days.ConvertToDto();
        //     
        //     return Ok(daysDto);
        // }
        //
        // [HttpGet]
        // public async Task<ActionResult> GetDaysInAParticularMonth(int year, int month,
        //     [FromQuery] RequestParams requestParams)
        // {
        //     if (year < 2020|| !ModelState.IsValid || month is < 0 or > 12)
        //     {
        //         return BadRequest(ModelState);
        //     }
        //     
        //     var monthDays = await _timeSheetManagerServices.GetDaysInAParticularMonth(year, month, requestParams);
        //     
        //     var monthDaysDto = monthDays.ConvertToDto();
        //     
        //     return Ok(monthDaysDto);
        // }
    }
}

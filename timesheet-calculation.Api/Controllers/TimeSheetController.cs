using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using timesheet_calculation.Business.Services;
using timesheet_calculation.Common.Utilities;
using timesheet_calculation.Data;
using timesheet_calculation.Data.Dtos;
using timesheet_calculation.Data.Entities;

namespace timesheet_calculation.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeSheetController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly TimeSheetServices _timeSheetServices;

        public TimeSheetController(AppDbContext dbContext, TimeSheetServices timeSheetServices)
        {
            _dbContext = dbContext;
            _timeSheetServices = timeSheetServices;
        }

        /// <summary>
        /// Checkin - Checkout Daily
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("{userId:Guid}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DailyCheckInAndOut(Guid userId, [FromBody]CreateTimeSheetDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //check neu ngay nay da checkin => return BadRequest();
            var imTimeSheets = _timeSheetServices.GetTimeSheetByUserId(userId).Result;
            var timeSheets = imTimeSheets?.FirstOrDefault(t => t.CheckInTime.GetValueOrDefault().Day == dto.CheckInTime.Day);
            if (timeSheets != null)
            {
                return BadRequest("You already have checked in this day.");
            }

            // check CheckIn va CheckOut phai cung 1 ngay
            if (dto.CheckInTime.Day != dto.CheckOutTime.Day)
            {
                return BadRequest("Checkin Date has to be the same as Checkout Date.");
            }
            
            // check gio cua checkIn < checkOut
            if (dto.CheckInTime.TimeOfDay > dto.CheckOutTime.TimeOfDay)
            {
                return BadRequest("Checkin time has to less than Checkout time.");
            }

            // check neu checkin qua som ( truoc 6 a.m)
            if (dto.CheckInTime.Hour < 6)
            {
                return BadRequest("You checkin too soon. Wait util 6 a.m");
            }

            if (_timeSheetServices.CheckWorkDayOrNot(dto).ToLower() == "holiday")
            {
                return StatusCode(StatusCodes.Status202Accepted, "Today is a holiday.");
            }

            if (_timeSheetServices.CheckWorkDayOrNot(dto).ToLower() == "dayoff")
            {
                return StatusCode(StatusCodes.Status202Accepted, "Today is a dayoff.");
            }

            if (_timeSheetServices.CheckWorkDayOrNot(dto).ToLower() == "null")
            {
                return NotFound();
            }

            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null)
            {
                return BadRequest($"the User with id {userId} is not found.");
            }
            
            var timeSheet = dto.ConvertToDto();
            timeSheet.UserId = userId;
            
            var result = await _timeSheetServices.CreateTimSheet(timeSheet);
            
            return NoContent();
        }

        
        /// <summary>
        /// Get total working days of userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{userId:Guid}")]
        public async Task<ActionResult> GetTimeSheetByUserId(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return BadRequest();
            }
            
            var timeSheet = await _timeSheetServices.GetTimeSheetByUserId(userId);
            if (timeSheet is null)
            {
                return NoContent();
            }
            
            var timeSheetDto = timeSheet.ConvertToDto();
            
            return Ok(timeSheetDto);
        }
    }
}

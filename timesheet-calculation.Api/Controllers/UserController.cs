using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using timesheet_calculation.Business.Models;
using timesheet_calculation.Business.Services;
using timesheet_calculation.Data;
using X.PagedList;

namespace timesheet_calculation.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly TimeSheetServices _timeSheetServices;

        public UserController(AppDbContext dbContext, TimeSheetServices timeSheetServices)
        {
            _dbContext = dbContext;
            _timeSheetServices = timeSheetServices;
        }

        [HttpGet]
        public async Task<ActionResult> GetUsers()
        {
            var users = await _dbContext.Users.ToListAsync();
            if (users is null)
            {
                return NotFound();
            }

            return Ok(users);
        }
    }
}

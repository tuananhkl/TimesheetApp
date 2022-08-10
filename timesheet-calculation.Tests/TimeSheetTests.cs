using System;
using timesheet_calculation.Business.Services;
using timesheet_calculation.Data.Dtos;
using timesheet_calculation.Data.Entities;
using Xunit;

namespace timesheet_calculation.Tests;

public class TimeSheetTests
{
    [Fact]
    public async void TimeSheetServices_GetDateInfo_Test()
    {
        var timeSheet = new TimeSheetServices(null!, null!);
        var data = new im_TimeSheet
        {
            CheckInTime = DateTime.UtcNow
        };

        var actual = await timeSheet.GetDateInfo(data);
        
        Assert.Equal(2022, actual.Year);
        Assert.Equal(8, actual.Month);
        Assert.Equal(9, actual.Day);
    }

    [Fact]
    public async void TimeSheetServices_GetTotalActualWorkingTimeInSeconds_Test()
    {
        var service = new TimeSheetServices(null!, null!);
        var data = new im_TimeSheet
        {
            CheckInTime = new DateTime(2022, 8, 9, 8, 29, 25),
            CheckOutTime = new DateTime(2022, 8, 9, 18, 01, 00)
        };

        var actual = await service.GetTotalActualWorkingTimeInSeconds(data);

        Assert.Equal(28895, actual.TotalActualWorkingTimeInSeconds);
    }
    [Fact]
    public async void TimeSheetServices_GetTotalActualWorkingTimeInSeconds_IfCheckOutTimeNull_Test()
    {
        var service = new TimeSheetServices(null!, null!);
        var data = new im_TimeSheet
        {
            CheckInTime = new DateTime(2022, 8, 9, 8, 29, 25),
            //CheckOutTime = new DateTime(2022, 8, 9, 18, 01, 00)
        };

        var actual = await service.GetTotalActualWorkingTimeInSeconds(data);

        Assert.Equal(0, actual.TotalActualWorkingTimeInSeconds);
    }

    [Fact]
    public async void TimeSheetServices_CheckWorkDayOrNot_ReturnHoliday_Test()
    {
        // phai Mock timeSheetManagerServiecs => failed
        var service = new TimeSheetServices(null!, null!);
        var data = new CreateTimeSheetDto
        {
            UserId = Guid.Parse("d29a38b9566c4c95a3802351796764f1"),
            CheckInTime = new DateTime(2022, 1, 1, 8, 29, 25),
            CheckOutTime = new DateTime(2022, 1, 1, 18, 01, 00)
        };
        var expected = "holiday";

        var actual = service.CheckWorkDayOrNot(data);

        Assert.Equal(expected, actual);
    }
}
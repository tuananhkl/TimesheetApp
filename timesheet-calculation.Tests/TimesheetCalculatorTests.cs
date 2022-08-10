using System;
using timesheet_calculation.Business.Services;
using Xunit;

namespace timesheet_calculation.Tests;

public class TimesheetCalculatorTests
{
    public static readonly object[] CorrectData =
    {
        new object[] { 8, 2022, new DateTime(2022,8,6)}
    };
    
    [Theory]
    [MemberData(nameof(CorrectData))]
    public void Business_TimesheetCalculator_GetFirstSaturday(int month, int year, DateTime expected)
    {
        var oldService = new OldService(true);
        var result = oldService.GetFirstSaturdayForTest(month, year);
        
        Assert.Equal(expected, result);
    }
    
    [Theory]
    [InlineData(8,2022,6)]
    public void Business_TimesheetCalculator_GetFirstSaturday_GetDay(int month, int year, int dayExpected)
    {
        var oldService = new OldService(true);
        var result = oldService.GetFirstSaturdayForTest(month, year).Day;
        
        Assert.Equal(dayExpected, result);
    }
}
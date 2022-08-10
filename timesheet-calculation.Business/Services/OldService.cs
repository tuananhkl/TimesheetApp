namespace timesheet_calculation.Business.Services;

public class OldService
{
    private readonly bool _isFirstSaturdayWorking;

    public OldService(bool isFirstSaturdayWorking)
    {
        _isFirstSaturdayWorking = isFirstSaturdayWorking;
    }
    
    private DateTime GetFirstSaturday(int month, int year)
    {

        DateTime firstSaturdayWorking = new(year, month, 1);
        
        while (firstSaturdayWorking.DayOfWeek != DayOfWeek.Saturday)
        {
            firstSaturdayWorking = firstSaturdayWorking.AddDays(1);
        }

        firstSaturdayWorking = firstSaturdayWorking;

        return firstSaturdayWorking;
    }
    public DateTime GetFirstSaturdayForTest(int month, int year) => GetFirstSaturday(month, year);

    private void CalculateSaturdayWorking()
    {
        DateTime firstSaturdayWorking;
        
        //calculate the day of first Saturday
        
        if (_isFirstSaturdayWorking == true)
        {
            
        }
    }
}
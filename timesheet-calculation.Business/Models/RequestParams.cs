namespace timesheet_calculation.Business.Models;

public class RequestParams
{
    private const int maxPageSize = 31;
        
    public int PageNumber { get; set; } = 1;
        
    private int _pageSize = 10;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
    }
}
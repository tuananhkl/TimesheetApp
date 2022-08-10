using System.ComponentModel.DataAnnotations;

namespace timesheet_calculation.Data.Entities;

// ReSharper disable once InconsistentNaming
public class im_User
{
    [Key]
    public Guid UserId { get; set; }
    public string Name { get; set; } = null!;
    public virtual ICollection<im_TimeSheet> TimeSheets { get; set; } = null!;
}
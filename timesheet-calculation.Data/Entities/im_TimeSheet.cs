using System.ComponentModel.DataAnnotations;

namespace timesheet_calculation.Data.Entities;

// ReSharper disable once InconsistentNaming
public class im_TimeSheet
{
    [Key]
    public Guid Id { get; set; }
    public DateTime? CheckInTime { get; set; }
    public DateTime? CheckOutTime { get; set; }
    public Guid UserId { get; set; }
}

/*
"Id" uuid NOT NULL,
"CheckInTime" timestamp NULL,
"CheckOutTime" timestamp NULL,
"UserId" uuid NOT NULL,
CONSTRAINT "PK_in_TimeSheet" PRIMARY KEY ("Id")
*/
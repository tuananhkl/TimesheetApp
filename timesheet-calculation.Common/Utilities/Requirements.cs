namespace timesheet_calculation.Common.Utilities;

public class Requirements
{
    private class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");


            /* CÔNG VIỆC CẦN LÀM
             * Cài đặt EF Core 5.0 vào solution này
             * Kết nối vào database PostgreSQL
             * Tạo một table timesheet như dưới đây 
             * CREATE TABLE public."in_TimeSheet" (
	            "Id" uuid NOT NULL,
	            "CheckInTime" timestamp NULL,
	            "CheckOutTime" timestamp NULL,
	            "UserId" uuid NOT NULL,
	            CONSTRAINT "PK_in_TimeSheet" PRIMARY KEY ("Id")
            );
            * Trong đó
            * - CheckinTime là thời gian một user check-in vào hệ thống (bắt đầu làm việc)
            * - CheckoutTime là thời gian một user checkout khỏi hệ thống (kết thúc làm việc)
            * 
            */
        }
    }

    private class UserDailyTimesheetModel
    {
        public Guid UserId { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public bool IsLate { get; set; }
        public int TotalLateInSeconds { get; set; }
        public int TotalActualWorkingTimeInSeconds { get; set; }
        public string Status { get; set; }
    }

    private class UserMonthlyTimesheetModel
    {
        public Guid UserId { get; set; }
        public int TotalWorkingHours { get; set; }
        public int TotalActualWorkingHours { get; set; }
        public List<UserDailyTimesheetModel> DailyTimesheets { get; set; }
    }

    private class TimesheetCalculator
    {
        bool isFirstSaturdayWorking = false;

        public TimesheetCalculator(bool isFirstSaturdayWorking)
        {
            this.isFirstSaturdayWorking = isFirstSaturdayWorking;
        }

        /* 
         * Method này dùng để tính toán thời gian làm việc của một user trong một tháng 
         * 
         * Với input truyền vào là userId, month và year. mục tiêu là tính ra thống kê timesheet trong tháng đó
         * 
         * Các rule như sau:
         * 1/ Rule ngày làm việc và ngày nghỉ
         * - Ngày làm việc được xác định là từ thứ hai đến thứ sáu, và xen kẽ 2 ngày thứ bảy
         * - Ví dụ tháng 5/2022 có 4 ngày thứ bảy vào các ngày 7, 14, 21, 28 thì 
         * - Nếu isFirstSaturdayWorking = true (ngày thứ 7 đầu tiên là ngày làm việc) thì ngày 21 làm việc.
         * - Nếu isFirstSaturdayWorking = false (ngày thứ 7 đầu tiên là ngày nghỉ) thì ngày 14 và 28 làm việc.
         * 
         *
         * 2/ Tính muộn trong 1 ngày
         * - Nếu thời gian checkin < 8h30 thì là muộn
         * - Số giây muộn được tính bằng: thời gian Checkin - thời điểm 08h30
         * - Ghi số giây muộn vào field TotalLateInSeconds và IsLate
         * 
         * 3/ Tính thời gian làm việc 1 ngày:
         * - Do giờ nghỉ trưa sẽ từ: 12h00 đến 1h30, thời gian làm việc tính như sau
         * - Buổi sáng tính từ lúc Checkin đến 12h00
         * - Buổi chiều tính từ lúc 1h30 đến lúc checkout
         * - Ghi thời gian làm việc vào field: TotalActualWorkingTimeInSeconds
         * 
         * 4/ Điền trạng thái Ngày (field Status)
         * - Ngày nào là ngày làm việc, có checkin và checkout đầy đủ, không muộn, thời gian làm việc trên 8h thì trạng thái là VALID
         * - Ngày nào là ngày làm việc, có checkin và checkout đầy đủ, thời gian làm việc < 8h thì trạng thái là INCOMPLETE
         * - Ngày nào là ngày làm việc, có checkin mà không có checkout thì trạng thái là INPROCESS
         * - Ngày nào là ngày làm việc, không có checkin thì trạng thái là ABSENT
         *  
         *  field TotalWorkingHours = tổng số ngày làm việc * 8
         *  field TotalActualWorkingHours = tổng số giờ làm việc thực tế
         */

        public UserMonthlyTimesheetModel Calculate(Guid userId, int month, int year)
        {
            // TODO
            // Implement this function
            return null;
        }
    }
}
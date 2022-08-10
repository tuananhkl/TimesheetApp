using System;

namespace TestDate
{
    class Program
    {
        static void Main(string[] args)
        {
            var student = new Student();

            student.FirstName = GetFirstName("Nguyen").FirstName;
            student.LastName = GetLastName("Tuan Anh").LastName;
            student.Age = GetAge(23).Age;

            Console.WriteLine($"{student.FirstName} {student.LastName} is {student.Age} years old");
        }

        private static Student GetFirstName(string firstName)
        {
            var student = new Student();
            student.FirstName = firstName;
            
            return student;
        }
        
        private static Student GetLastName(string lastName)
        {
            var student = new Student();
            student.LastName = lastName;
            
            return student;
        }
        
        private static Student GetAge(int age)
        {
            var student = new Student();
            student.Age = age;
            
            return student;
        }

        private static void CheckinTime()
        {
            var checkInTime = new DateTime(2022, 8,9,8,31,04);
            var workCheckInTime = new DateTime(2022, 8,9,8,30,00);

            // TimeSpan diff = checkInTime.ToUniversalTime() - workCheckInTime;

            if (checkInTime <= workCheckInTime)
            {
                Console.WriteLine($"{checkInTime} <= {workCheckInTime}: Not Late");
            }
            else
            {
                Console.WriteLine($"{checkInTime} > {workCheckInTime}: Late");
                //Console.WriteLine("Total late seconds: " + Math.Floor(diff.TotalSeconds));
                Console.WriteLine("Total late seconds: " + (checkInTime - workCheckInTime).TotalSeconds);
            }
        }
    }

    public class Student
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
    }
}
using System;

namespace CheckDate
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime date = new DateTime(2022, 8, 7);

            var day = date.Day;

            Console.WriteLine(day);
        }
    }
}
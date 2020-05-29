using System;

namespace CourseWorkOperationResearch
{
    class Program
    {
        static void Main(string[] args)
        {
            var c = new Import.Import().GetFullGraphFormFile(
                @"D:\RiderProjects\CourseWorkOperationResearch\Import\import.json");
            var d = null as string;
        }
    }
}

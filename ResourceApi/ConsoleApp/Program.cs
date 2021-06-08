using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp
{
    class Program
    {
        static void Main()
        {
            List<UserActivity> list = new List<UserActivity>() 
            { 
                new UserActivity { UserID = 0, DateRegistration = DateTime.Parse("01.05.2021"), DateLastActivity = DateTime.Parse("08.05.2021") },
                new UserActivity { UserID = 0, DateRegistration = DateTime.Parse("02.05.2021"), DateLastActivity = DateTime.Parse("05.05.2021") },
                new UserActivity { UserID = 0, DateRegistration = DateTime.Parse("03.05.2021"), DateLastActivity = DateTime.Parse("04.05.2021") },
                new UserActivity { UserID = 0, DateRegistration = DateTime.Parse("04.05.2021"), DateLastActivity = DateTime.Parse("12.05.2021") },
                new UserActivity { UserID = 0, DateRegistration = DateTime.Parse("05.05.2021"), DateLastActivity = DateTime.Parse("18.05.2021") },
                new UserActivity { UserID = 0, DateRegistration = DateTime.Parse("06.05.2021"), DateLastActivity = DateTime.Parse("09.05.2021") },
                new UserActivity { UserID = 0, DateRegistration = DateTime.Parse("07.05.2021"), DateLastActivity = DateTime.Parse("22.05.2021") },
                new UserActivity { UserID = 0, DateRegistration = DateTime.Parse("01.06.2021"), DateLastActivity = DateTime.Parse("06.06.2021") },
                new UserActivity { UserID = 0, DateRegistration = DateTime.Parse("02.06.2021"), DateLastActivity = DateTime.Parse("05.06.2021") },
            };

            var groups = list.GroupBy(l => l.DaysByActivity).Select(g => new { Name = g.Key, Count = g.Count() });
            groups.ToList().ForEach(g => Console.WriteLine($"Name: {g.Name} Count: {g.Count}"));

            var result = Calculate(list);



            Console.ReadKey();
        }

        public static int Calculate(IEnumerable<UserActivity> ua, int days)
        {
            double quantityActivity = ua.Where(a => a.DaysByActivity >= days).Count();
            double quantityRegistration = ua.Where(a => (DateTime.Now - a.DateRegistration).Days >= days).Count();
            double result = (quantityActivity / (quantityRegistration < 1 ? 1 : quantityRegistration)) * 100;

            return (int)result;
        }

        public static object Calculate(IEnumerable<UserActivity> ua)
        {
            double quantityActivity = ua.Where(a => a.DaysByActivity >= 7).Count();
            double quantityRegistration = ua.Where(a => (DateTime.Now - a.DateRegistration).Days >= 7).Count();
            double result = (quantityActivity / (quantityRegistration < 1 ? 1 : quantityRegistration)) * 100;

            var groups = ua.GroupBy(l => l.DaysByActivity).Select(g => new { Days = g.Key, UsersCount = g.Count() });

            var group = new { Data = groups.ToList(), result = (int)result };

            return group;
        }
    }

    public class UserActivity
    {
        public int UserID { get; set; }

        public DateTime DateRegistration { get; set; }

        public DateTime DateLastActivity { get; set; }

        public int DaysByActivity => (DateLastActivity - DateRegistration).Days;
    }
}

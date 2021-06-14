using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using MainApi.Models;
using MainApi.Models.Types;

namespace MainApi.Services
{
    public class UsersRepository : IDisposable, IRepository
    {
        private UserContext db = new UserContext();
        private ExecutionContext ec = new ExecutionContext();
        private readonly Stopwatch sw = new Stopwatch();

        public UserActivity Update(UserActivity ua)
        {
            sw.Start();

            var activity = db.UsersActivities.FirstOrDefault(u => u.UserID == ua.UserID);

            if (activity is null) throw new Exception($"Объект {ua.UserID} не найден");

            activity.DateRegistration = ua.DateRegistration;
            activity.DateLastActivity = ua.DateLastActivity;

            db.UsersActivities.Update(activity);
            db.SaveChanges();

            sw.Stop();
            ec.Profiling.Add(new ExecutionTime(Operations.Update, sw.ElapsedTicks));
            ec.SaveChanges();

            return activity;
        }

        public void SaveOne(UserActivity ua)
        {
            sw.Start();

            db.UsersActivities.Add(ua);
            db.SaveChanges();

            sw.Stop();
            ec.Profiling.Add(new ExecutionTime(Operations.GetAll, sw.ElapsedTicks));
            ec.SaveChanges();
        }

        public void SaveAll(IEnumerable<UserActivity> ua)
        {

            db.UsersActivities.AddRange(ua.Distinct(new Compare()));
            db.SaveChanges();
        }

        public int Calculate(IEnumerable<UserActivity> ua, int days)
        {
            double quantityActivity = ua.Where(a => a.DaysByActivity >= days).Count();
            double quantityRegistration = ua.Where(a => (DateTime.Now - a.DateRegistration).Days >= days).Count();
            double result = (quantityActivity / (quantityRegistration < 1 ? 1 : quantityRegistration)) * 100;

            return (int)result;
        }

        public object Calculate(IEnumerable<UserActivity> ua)
        {
            sw.Start();
            
            double quantityActivity = ua.Where(a => a.DaysByActivity >= 7).Count();
            double quantityRegistration = ua.Where(a => (DateTime.Now - a.DateRegistration).Days >= 7).Count();
            double result = (quantityActivity / (quantityRegistration < 1 ? 1 : quantityRegistration)) * 100;

            var groups = ua.GroupBy(l => l.DaysByActivity).Select(g => new { Days = g.Key, UsersCount = g.Count() });

            var group = new { Data = groups.ToList(), result = (int)result };

            sw.Stop();
            ec.Profiling.Add(new ExecutionTime(Operations.Calculate, sw.ElapsedTicks));
            ec.SaveChanges();

            return group;
        }

        public IEnumerable<UserActivity> GetAll()
        {
            sw.Start();
            var usersActivities = db.UsersActivities;
            sw.Stop();
            ec.Profiling.Add(new ExecutionTime(Operations.GetAll, sw.ElapsedTicks));
            ec.SaveChanges();
            return usersActivities;
        }

        public void RemoveAll(IEnumerable<UserActivity> ua)
        {
            db.UsersActivities.RemoveRange(ua);
            db.SaveChanges();
        }

        public void RemoveOne(int id)
        {
            UserActivity userActivity = db.UsersActivities.FirstOrDefault(u => u.UserID == id);

            if (userActivity is null) throw new Exception($"Объект {id} не найден.");

            db.UsersActivities.Remove(userActivity);
            db.SaveChanges();
        }

        public UserActivity GetOne(int id)
        {
            return db.UsersActivities.Find(id);
        }

        public IEnumerable<object> GetExecutionTimes()
        {
            var groups = ec.Profiling.GroupBy(o => o.Operation).Select(g => new { Operation = g.Key, Elapsed = g.Sum(o => o.Elapsed), Count = g.Count() });

            return groups;
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null)
                {
                    db.Dispose();
                    db = null;
                }

                if (ec != null)
                {
                    ec.Dispose();
                    ec = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    internal class Compare : IEqualityComparer<UserActivity>
    {
        public bool Equals([AllowNull] UserActivity x, [AllowNull] UserActivity y)
        {
            return x.UserID == y.UserID;
        }

        public int GetHashCode([DisallowNull] UserActivity u)
        {
            int hCode = u.UserID;
            return hCode.GetHashCode();
        }
    }
}

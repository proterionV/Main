using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using MainApi.Models;

namespace MainApi.Services
{
    public class UsersRepository : IDisposable, IRepository
    {
        private UserContext db = new UserContext();

        public void SaveOne(UserActivity ua)
        {
            db.Add(ua);
            db.SaveChanges();
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
            double quantityActivity = ua.Where(a => a.DaysByActivity >= 7).Count();
            double quantityRegistration = ua.Where(a => (DateTime.Now - a.DateRegistration).Days >= 7).Count();
            double result = (quantityActivity / (quantityRegistration < 1 ? 1 : quantityRegistration)) * 100;

            var groups = ua.GroupBy(l => l.DaysByActivity).Select(g => new { Days = g.Key, UsersCount = g.Count() });

            var group = new { Data = groups.ToList(), result = (int)result };

            return group;
        }

        public IEnumerable<UserActivity> GetAll()
        {
            return db.UsersActivities;
        }

        public void RemoveAll(IEnumerable<UserActivity> ua)
        {
            db.UsersActivities.RemoveRange(ua);
            db.SaveChanges();
        }

        public void RemoveOne(int id)
        {
            db.Remove(db.UsersActivities.Find(id));
            db.SaveChanges();
        }

        public UserActivity GetOne(int id)
        {
            return db.UsersActivities.Find(id);
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

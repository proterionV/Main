using MainApi.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace MainApi.Services
{
    public class UsersRepository : IDisposable, IRepository
    {
        private UserContext db = new UserContext();

        public void SaveOne(UserActivity ua)
        {
            db.UsersActivities.Add(ua);
            db.SaveChanges();
        }

        public void SaveAll(IEnumerable<UserActivity> ua)
        {
            db.UsersActivities.AddRange(ua.Distinct(new Compare()));
            db.SaveChanges();
        }

        public int Calculate(IEnumerable<UserActivity> ua, int days)
        {
            int quantityActivity = ua.Where(a => a.DaysByActivity >= days).Count();
            int quantityRegistration = ua.Where(a => (DateTime.Now - a.DateRegistration).Days >= days).Count();

            return (quantityActivity / (quantityRegistration < 1 ? 1 : quantityRegistration)) * ua.Count();
        }

        public IEnumerable<UserActivity> GetAll()
        {
            return db.UsersActivities;
        }

        public void Remove(IEnumerable<UserActivity> ua)
        {
            db.UsersActivities.RemoveRange(ua);
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

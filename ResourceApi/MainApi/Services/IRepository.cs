using System.Collections.Generic;

using MainApi.Models;

namespace MainApi.Services
{
    public interface IRepository
    {
        void SaveOne(UserActivity ua);

        void SaveAll(IEnumerable<UserActivity> ua);

        IEnumerable<UserActivity> GetAll();

        UserActivity GetOne(int id);

        void Remove(IEnumerable<UserActivity> ua);

        int Calculate(IEnumerable<UserActivity> ua, int days);
    }
}

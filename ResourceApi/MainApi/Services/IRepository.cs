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

        void RemoveAll(IEnumerable<UserActivity> ua);

        void RemoveOne(int id);

        int Calculate(IEnumerable<UserActivity> ua, int days);
    }
}

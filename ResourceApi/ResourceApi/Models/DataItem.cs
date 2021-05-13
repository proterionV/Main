using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceApi.Models
{
    public static class DataItem
    {
        public static ConcurrentDictionary<decimal, Resource> Items { get; set; } = new ConcurrentDictionary<decimal, Resource>();

        static DataItem()
        {
            ItemsFillDefault();
        }

        private static void ItemsFillDefault() // заполняем словарь данными по умолчанию 
        {
            for (int i = 1; i < 5; i++)
                Items.TryAdd(i, new Resource { Id = i, Name = $"Name {i}" });
        }

        public static bool AddItem(Resource Resource) // Добавим новый ресурс
        {
            if (Items.ContainsKey(Resource.Id))
                return false;

            return Items.TryAdd(Resource.Id, Resource);
        }

        public static bool RemoveItem(decimal Key) // удаляем ресурс
        {
            return Items.TryRemove(Key, out _);
        }

        public static bool UpdateItem(Resource Resource) // обновляем ресурс
        {
            if (Items.ContainsKey(Resource.Id))
            {
                Items[Resource.Id] = Resource;
                return true;
            }
            return false;
        }
    }
}

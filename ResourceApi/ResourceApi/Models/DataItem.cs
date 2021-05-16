using System;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace ResourceApi.Models
{
    public static class DataItem
    {
        public static ConcurrentDictionary<decimal, Resource> Items;

        static DataItem()
        {
            ItemsFillDefault();
        }

        /// <summary>
        /// Заполнение потокобезопасного словаря данными по умолчанию
        /// </summary>
        private static void ItemsFillDefault() 
        {
            Items = new ConcurrentDictionary<decimal, Resource>();

            for (int i = 1; i < 5; i++)
                Items.TryAdd(i, new Resource { Id = i, Name = $"Name {i}" });
        }
    }
}

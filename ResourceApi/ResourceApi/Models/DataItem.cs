using System;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace ResourceApi.Models
{
    public static class DataItem
    {
        public static ConcurrentDictionary<decimal, Resource> Items { get; set; } = new ConcurrentDictionary<decimal, Resource>();

        static DataItem()
        {
            ItemsFillDefault();
        }

        /// <summary>
        /// Заполнение потокобезопасного словаря данными по умолчанию
        /// </summary>
        public static void ItemsFillDefault() // заполняем словарь данными по умолчанию 
        {
            for (int i = 1; i < 5; i++)
                Items.TryAdd(i, new Resource { Id = i, Name = $"Name {i}" });
        }

        /// <summary>
        /// Добавление одного элемента в словарь
        /// </summary>
        /// <param name="Resource">Добавляемый объект</param>
        /// <returns></returns>
        public static bool AddItem(Resource Resource) // Добавим новый ресурс
        {
            if (Items.ContainsKey(Resource.Id))
            {
                Trace.TraceError($"Невозможно добавить ресурс, объект с идентификатором \"{Resource.Id}\" уже существует");
                return false;
            }

            Trace.TraceInformation($"Оъект \"{Resource.Id}\" добавлен");
            return Items.TryAdd(Resource.Id, Resource);
        }

        /// <summary>
        /// Обновляем инфо о ресурсе
        /// </summary>
        /// <param name="Resource">Объект обновляемого ресурса</param>
        /// <returns></returns>
        public static bool UpdateItem(Resource Resource) 
        {
            try
            {
                if (Items.ContainsKey(Resource.Id))
                {
                    Items[Resource.Id] = Resource;
                    Trace.TraceInformation($"Оъект \"{Resource.Id}\" обновлен");
                    return true;
                }
                Trace.TraceError($"Невозможно обновить ресурс, объект с идентификатором \"{Resource.Id}\" не существует");
                return false;
            }
            catch(Exception ex)
            {
                Trace.TraceError($"Невозможно обновить ресурс. {ex.Message}");
                return false;
            }
        }
    }
}

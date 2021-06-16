using System;
using System.Linq;
using System.Collections;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;

namespace ConsoleApp
{
    class Program
    {
        static readonly object locker = new object(); // экземпляр объекта для блокировки выполнения команд консоли
        static readonly Queue queue = new Queue();    // очередь для определения очередности завершения выполнения

        static void Main()
        {
            #region ввод с консоли 
            Console.Write("Object Count: ");
            int maxObjects = int.Parse(Console.ReadLine()); // количество объектов

            Console.Write("Thread Count: ");
            int maxThreads = int.Parse(Console.ReadLine()); // количество потоков

            Console.Write("Load Length: ");
            int maxSteps = int.Parse(Console.ReadLine()); // количество итераций
            #endregion

            ConsoleInit(maxSteps, maxObjects); 

            // создание последовательности экземпляров c инициализацией начальных положений в конструкторе
            var works = Enumerable.Range(0, maxObjects).Select(index => new Work(0, index));

            // выполнение метода обработчика в отдельном потоке для каждого объекта с ожиданием завершения каждого потока
            Parallel.ForEach(works, new ParallelOptions { MaxDegreeOfParallelism = maxThreads }, work => Handler(work, maxSteps));

            // вывод сообщения о завершении обработки объектов на позицию следующей строки после последнего результата
            Console.SetCursorPosition(0, maxObjects);
            Console.WriteLine("All Done!");

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// Выполнение имитации и вывода информации объекта на его позицию
        /// </summary>
        /// <param name="worker"></param>
        private static void Handler(Work work, int maxLength)
        {
            Stopwatch sw = new Stopwatch(); // инициализация экземпляра таймера для фиксации времени выполнения
            sw.Start(); // запуск таймера

            // поскольку класс Console является статическим, для операций с консолью необходимо использовать блокировку для позиционирования курсора
            lock (locker) 
            {
                // вывод информации об объекте и текущем потоке
                Console.SetCursorPosition(work.X, work.Y);
                Console.Write($"{work.Y}({Environment.CurrentManagedThreadId})\t");
                work.X = Console.CursorLeft; // сохранение текущего положения в строке, деблокировка
            }

            int startPos = work.X; // стартовая позиция после вывода первичной информации для корректного отображения количества итераций

            for (var i = 0; i < maxLength; i++)
            {
                try
                {
                    Task.Delay(new Random().Next(10, 100)).Wait(); // имитация выполнения обработки

                    // сравнение случайно сгенерированного свойства Chance с прозвольным значением, при выполении условия, выброс обрабатываемого исключения
                    if (work.Chance > 8) throw new Exception($"Случайное исключение в потоке {Environment.CurrentManagedThreadId}, шаг {work.X}");

                    // если нет исключения - блокировка для вывода итерации цветом по умолчанию
                    lock (locker) 
                    {
                        Console.SetCursorPosition(work.X, work.Y);
                        Console.Write($"■ {work.X - startPos}");  
                    }
                }
                catch (Exception ex)
                {
                    Trace.TraceError(ex.Message); // запись сообщения об исключении в отладчике с уровнем Error

                    lock (locker)
                    {
                        // вывод итерации цвета исклюения, возврат цвета в исходное состояние
                        Console.SetCursorPosition(work.X, work.Y);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write($"■ {work.X - startPos}");
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                }

                work.X++; // позиция для вывода следующей итерации
                work.UpdateChance(); // генерация случайного значения для имитации исключения
            }

            lock (locker)
            {
                queue.Enqueue(Environment.CurrentManagedThreadId); // добавление в очередь потока
                Console.SetCursorPosition(work.X, work.Y);         // позиционирование
                Console.Write($" {work.X - startPos}  ");          // расчет текущей итерации
                Console.ForegroundColor = ConsoleColor.Red;        // смена цвета вывода
                Console.Write($"{queue.Count}! ({(double)sw.ElapsedMilliseconds/1000})"); // вывод позиции завершенной имитации, времени выполнения в секундах
                Console.ForegroundColor = ConsoleColor.Green;      // исходный цвет вывода 
            }
        }

        /// <summary>
        /// Установка свойст окна консоли.
        /// </summary>
        /// <param name="width">Ширина окна консоли</param>
        /// <param name="height">Высота окна консоли</param>
        private static void ConsoleInit(int width, int height)
        {
            Console.Clear(); // очистка окна консоли, позиция курсора по умолчанию

            Console.CursorVisible = false; // отключение видимости курсора

            Console.ForegroundColor = ConsoleColor.Green; // цвет выводящейся информации

            Console.SetWindowSize(width + 50, height + 25); // размер окна консоли (константные значения произвольные)
        }
    }
}

using System;

namespace ConsoleApp
{
    internal class Work // internal - видимость в области текущего проекта
    {
        #region свойства
        internal int Id { get; }       // порядковый номер экзепляра
        internal int X  { get; set; }  // текущий столбец
        internal int Y  { get; set; }  // текущая строка

        internal int Chance            // случайное число для вызова исключения
        { 
            get; 
            private set; 
        }
        
        private Random Rand { get; }   // генератор случайных чисел 
        #endregion

        /// <summary>
        /// Обрабатываемый объект имитации процесса потока.
        /// </summary>
        /// <param name="xPos">Начальное положения курсора консоли в столбце.</param>
        /// <param name="yPos">Начального положения курсора консоли в строке.</param>
        internal Work(int xPos, int yPos)
        {
            X = xPos;
            Y = yPos;
            Id = yPos; // изначально порядковый номер совпадает с положением строки

            // инициализация экземпляра со значением тактов текущего времени + число позиции курсора консоли для большей разницы случайных чисел в потоках
            Rand = new Random((int)DateTime.Now.Ticks + Y); 

            // обновление значения свойства Chance
            UpdateChance();
        }

        /// <summary>
        /// Обновление свойства Chance
        /// </summary>
        internal void UpdateChance() => Chance = Rand.Next(10); 
    }
}

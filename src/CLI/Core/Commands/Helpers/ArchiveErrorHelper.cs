using System;

namespace TTG_Tools.CLI.Core.Commands.Helpers
{
    public static class ArchiveErrorHelper
    {
        /// <summary>
        /// Перевіряє агрегований виняток на наявність <see cref="ArgumentOutOfRangeException"/>. 
        /// Якщо знайдено — виводить повідомлення в консоль; інакше — прокидує виняток далі.
        /// </summary>
        /// <param name="aggEx">Обгортка агрегованих винятків асинхронної задачі.</param>
        /// <exception cref="AggregateException">Кидається, якщо виняток не належить до очікуваних (ArgumentOutOfRangeException).</exception>
        public static void HandleArchiveError(AggregateException aggEx)
        {
            bool isOutOfRange = false;
            
            foreach (var inner in aggEx.InnerExceptions)
            {
                if (inner is ArgumentOutOfRangeException)
                {
                    isOutOfRange = true;
                    Console.WriteLine(@"Program cannot auto-detect game type, please specify game name manually.");
                    break;
                }
            }

            if (!isOutOfRange)
            {
                throw aggEx;
            }
        }
    }
}
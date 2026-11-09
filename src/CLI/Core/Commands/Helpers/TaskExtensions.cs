using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TTG_Tools.CLI.Core.Commands.Helpers
{
    /// <summary>
    /// Допоміжний клас для безпечного синхронного очікування асинхронних завдань в UI-оточенні.
    /// </summary>
    public static class TaskExtensions
    {
        /// <summary>
        /// Синхронно очікує завершення задачі (<see cref="Task"/>), використовуючи <see cref="Application.DoEvents()"/> 
        /// для запобігання дедлокам в UI-потоці, та коректно розгортає внутрішні винятки.
        /// </summary>
        /// <param name="task">Задача, яку потрібно дочекатися.</param>
        public static void WaitWithDoEvents(this Task task)
        {
            while (!task.IsCompleted)
            {
                Application.DoEvents();
                Thread.Sleep(100);
            }

            if (task.IsFaulted && task.Exception != null)
            {
                // Розгортаємо AggregateException і кидаємо справжній оригінальний виняток
                ExceptionDispatchInfo.Capture(task.Exception.InnerException ?? task.Exception).Throw();
            }
        }

        /// <summary>
        /// Синхронно очікує завершення задачі з результатом (<see cref="Task{T}"/>) 
        /// та повертає отримане значення.
        /// </summary>
        /// <typeparam name="T">Тип результату задачі.</typeparam>
        /// <param name="task">Задача з результатом, яку потрібно дочекатися.</param>
        /// <returns>Результат виконання задачі.</returns>
        public static T WaitWithDoEvents<T>(this Task<T> task)
        {
            while (!task.IsCompleted)
            {
                Application.DoEvents();
                Thread.Sleep(100);
            }

            if (task.IsFaulted && task.Exception != null)
            {
                ExceptionDispatchInfo.Capture(task.Exception.InnerException ?? task.Exception).Throw();
            }

            return task.Result;
        }
    }
}
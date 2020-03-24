namespace DNI.Core.Services.Extensions
{
    using System.Threading.Tasks;

    public static class TaskHelper
    {
        public static async void RunTask(Task task, bool continueCapturedContext = false)
        {
            await task
                .ConfigureAwait(continueCapturedContext);
        }

        public static async Task<T> RunTask<T>(Task<T> task, bool continueCapturedContext = false)
        {
            return await task
                .ConfigureAwait(continueCapturedContext);
        }
    }
}

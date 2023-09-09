using System.Threading.Tasks;

namespace Utilities
{
    public static class TaskExtensions
    {
        // Used so we can use fire and forget tasks and still catch exceptions.
        public static async void Forget(this Task task)
        {
            await task;
        }
    }
}

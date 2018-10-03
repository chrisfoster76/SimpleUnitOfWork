using System;
using System.Threading.Tasks;

namespace SimpleUnitOfWork
{
    public interface IUnitOfWork
    {
        Task ExecuteAsync(Func<Task> action);
    }
}

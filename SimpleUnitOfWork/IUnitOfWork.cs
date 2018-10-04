using System;
using System.Threading.Tasks;
using System.Transactions;

namespace SimpleUnitOfWork
{
    public interface IUnitOfWork
    {
        Task ExecuteAsync(Func<Task> action, IsolationLevel isolationLevel = IsolationLevel.Serializable);
    }
}

using System.Threading;
using System.Threading.Tasks;

namespace Calendar.DataModels
{
    public interface IStorageTools
    {
        /// <summary>
        /// Приводит БД в актуальное состояние
        /// </summary>
        Task MigrateAsync(CancellationToken cancellationToken);
    }
}

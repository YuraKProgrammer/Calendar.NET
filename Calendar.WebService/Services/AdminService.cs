using System;
using System.Threading;
using System.Threading.Tasks;
using Calendar.DataModels;
using Kalantyr.Web;

namespace Calendar.WebService.Services
{
    /// <summary>
    /// Сервис для администрирования сайта
    /// </summary>
    public class AdminService
    {
        private readonly IStorageTools _storageTools;

        public AdminService(IStorageTools storageTools)
        {
            _storageTools = storageTools ?? throw new ArgumentNullException(nameof(storageTools));
        }

        public async Task<ResultDto<bool>> MigrateAsync(string token, CancellationToken cancellationToken)
        {
            // TODO: нужна авторизация

            await _storageTools.MigrateAsync(cancellationToken);

            return ResultDto<bool>.Ok;
        }
    }
}

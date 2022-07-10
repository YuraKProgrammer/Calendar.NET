using Kalantyr.Auth.Client;
using Kalantyr.Auth.Models;
using Kalantyr.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Calendar.DesktopClient.Wrappers
{
    public class AuthClientWrapper : IAuthClient
    {
        private IAuthClient _authClient;
        private Random _random = new Random();
        public AuthClientWrapper(IAuthClient authClient)
        {
            _authClient = authClient;
        }

        public async Task<ResultDto<uint>> CreateUserWithPasswordAsync(string login, string password, string userToken, CancellationToken cancellationToken)
        {
            await TestError();
            return await _authClient.CreateUserWithPasswordAsync(login, password, userToken, cancellationToken);
        }

        private async Task TestError()
        {
            if (_random.Next(2) == 0)
            {
                throw new Exception("Случилась ошибка");
            }
            if (_random.Next(2) == 0)
            {
                await Task.Delay(TimeSpan.FromSeconds(2));
            }
        }

        public async Task<ResultDto<TokenInfo>> LoginByPasswordAsync(LoginPasswordDto loginPasswordDto, CancellationToken cancellationToken)
        {
            await TestError();
            return await _authClient.LoginByPasswordAsync(loginPasswordDto, cancellationToken);
        }

        public async Task<ResultDto<bool>> LogoutAsync(string userToken, CancellationToken cancellationToken)
        {
            await TestError();
            return await _authClient.LogoutAsync(userToken, cancellationToken);
        }

        public async Task<ResultDto<bool>> SetPasswordAsync(string userToken, string oldPassword, string newPassword, CancellationToken cancellationToken)
        {
            await TestError();
            return await _authClient.SetPasswordAsync(userToken, oldPassword, newPassword, cancellationToken);
        }

       
    }
}

using Kalantyr.Auth.Client;
using Kalantyr.Auth.Models;
using Kalantyr.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Calendar.DesktopClient.Wrappers
{
    internal class AuthClientRetryWrapper : IAuthClient
    {
        private readonly IAuthClient _authClient;

        private const int MaxRetryCount = 4;

        public AuthClientRetryWrapper(IAuthClient authClient)
        {
            _authClient = authClient;
        }
        public async Task<ResultDto<uint>> CreateUserWithPasswordAsync(string login, string password, string userToken, CancellationToken cancellationToken)
        {
            return await _authClient.CreateUserWithPasswordAsync(login, password, userToken, cancellationToken);
        }

        public async Task<ResultDto<TokenInfo>> LoginByPasswordAsync(LoginPasswordDto loginPasswordDto, CancellationToken cancellationToken)
        {
            var count = 0;
            while (count < MaxRetryCount)
            {
                try
                {
                    count++;
                    return await _authClient.LoginByPasswordAsync(loginPasswordDto, cancellationToken);
                }
                catch
                {
                    if (count == MaxRetryCount)
                    {
                        throw;
                    }
                    else
                    {
                        await Task.Delay(GetRetryDelay(count));
                        Debug.WriteLine("Попытка #"+(count+1));
                    }
                }
            }
            throw new NotImplementedException();
        }

        public async Task<ResultDto<bool>> LogoutAsync(string userToken, CancellationToken cancellationToken)
        {
            var count = 0;
            while (count < MaxRetryCount)
            {
                try
                {
                    count++;
                    return await _authClient.LogoutAsync(userToken, cancellationToken);
                }
                catch
                {
                    if (count == MaxRetryCount)
                    {
                        throw;
                    }
                    else
                    {
                        await Task.Delay(GetRetryDelay(count));
                        Debug.WriteLine("Попытка #" + (count + 1));
                    }
                }
            }
            throw new NotImplementedException();
        }

        public async Task<ResultDto<bool>> SetPasswordAsync(string userToken, string oldPassword, string newPassword, CancellationToken cancellationToken)
        {
            return await _authClient.SetPasswordAsync(userToken, oldPassword, newPassword, cancellationToken);
        }

        private TimeSpan GetRetryDelay(int retryNumber)
        {
            return TimeSpan.FromSeconds(retryNumber*retryNumber);
        }
    }
}

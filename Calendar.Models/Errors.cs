using Kalantyr.Web;

namespace Calendar.Models
{
    public static class Errors
    {
        public static Error TokenNotFound { get; } = new() {
            Code = nameof(TokenNotFound),
            Message = "Токен не найден"
        };
    }
}

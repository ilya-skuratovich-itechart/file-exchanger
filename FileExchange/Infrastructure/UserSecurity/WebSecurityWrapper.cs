using WebMatrix.WebData;

namespace FileExchange.Infrastructure.UserSecurity
{
    public class WebSecurityWrapper : IWebSecurity
    {
        public int GetCurrentUserId()
        {
            return WebSecurity.CurrentUserId;
        }

        public bool ResetPassword(string passwordResetToken, string newPassword)
        {
            return WebSecurity.ResetPassword(passwordResetToken, newPassword);
        }

        public string GeneratePasswordResetToken(string userName, int tokenExpirationInMinutesFromNow = 1440)
        {
            return WebSecurity.GeneratePasswordResetToken(userName, tokenExpirationInMinutesFromNow);
        }

        public int GetUserId(string userName)
        {
            return WebSecurity.GetUserId(userName);
        }
        public bool IsAuthenticated()
        {
            return WebSecurity.IsAuthenticated;
        }

        public string CreateUserAndAccount(string userName, string password, object propertyValues = null,
            bool requireConfirmationToken = false)
        {
            return WebSecurity.CreateUserAndAccount(userName, password, propertyValues, requireConfirmationToken);
        }

        public bool Login(string userName, string password, bool persistCookie = false)
        {
            return WebSecurity.Login(userName,password,persistCookie);
        }

        public void Logout()
        {
            WebSecurity.Logout();
        }
    }
}
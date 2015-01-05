namespace FileExchange.Infrastructure.UserSecurity
{
    public interface IWebSecurity
    {
        int GetCurrentUserId();
        int GetUserId(string userName);
        bool ResetPassword(string passwordResetToken, string newPassword);
        string GeneratePasswordResetToken(string userName, int tokenExpirationInMinutesFromNow = 0x5a0);
        bool IsAuthenticated();

        string CreateUserAndAccount(string userName, string password, object propertyValues = null,
            bool requireConfirmationToken = false);
      
        bool Login(string userName,string password,bool persistCookie=false);
        void Logout();
    }
}
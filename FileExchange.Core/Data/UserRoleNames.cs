namespace FileExchange.Core.Data
{
    public class UserRoleNames
    {
        public const string ActiveUser = "ActiveUser";
        public const string Admin = "Admin";
        public const string Moderator = "Moderator";
        public const string EditNewsAllowRoles = Admin + "," + Moderator;
    }
}
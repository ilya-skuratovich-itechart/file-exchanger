namespace FileExchange.Core.DTO
{
    public class FileUserNotification
    {
        public NotificationType NotificationType { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }
    }

    public enum NotificationType
    {
        descriptionChanged,
        accessDienied,
        fileDelited
    }
}
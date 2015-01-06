namespace FileExchange.Infrastructure.Captcha
{
    public interface ICaptcha
    {
        byte[] GetCaptchaImage();
        bool IsValidCaptchaValue(string captchaValue);
    }
}
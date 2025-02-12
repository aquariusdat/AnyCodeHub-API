using BCrypt.Net;
using MassTransit;

namespace AnyCodeHub.Contract.CommonServices;
public static class CommonHelper
{
    public static DateTime ConvertUnixTimeToDateTime(long utcExpireDate)
    {
        DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc); // Thời gian bắt đầu của Epoch Unix
        DateTime dateTime = epoch.AddSeconds(utcExpireDate); // Thêm giá trị giây vào thời gian bắt đầu Epoch Unix
        dateTime = dateTime.ToLocalTime();
        return dateTime;
    }

    public static string HashPasswordWithBCrypt(this string passWord)
    {
        return BCrypt.Net.BCrypt.EnhancedHashPassword(passWord, HashType.SHA256);
    }

    public static string DecodedPasswordWithBCrypt(this string passWord)
    {
        return BCrypt.Net.BCrypt.EnhancedHashPassword(passWord, HashType.SHA256);
    }

    public static bool VerifyPassword(this string passWordHahsed, string passWord)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(passWord, passWordHahsed, HashType.SHA256);
    }

    public static Uri GetQueueNameByEntity(this object entity) => new($"queue:{KebabCaseEndpointNameFormatter.Instance.SanitizeName(entity.GetType().Name)}");
}

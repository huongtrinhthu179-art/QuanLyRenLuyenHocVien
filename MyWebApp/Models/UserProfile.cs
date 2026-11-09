namespace MyWebApp.Models;

/// <summary>Thông tin hiển thị của người dùng gắn với tài khoản Identity.</summary>
public class UserProfile
{
    public string UserId { get; set; } = string.Empty;

    public string? FullName { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string? Gender { get; set; }
}

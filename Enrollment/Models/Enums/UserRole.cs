namespace Enrollment.Models.Enums;

public static class UserRole
{
    public const string Admin = "Admin";
    public const string Professor = "Professor";
    public const string Student = "Student";
    
    public const string AnyLogin = $"{Admin},{Professor},{Student}";
}
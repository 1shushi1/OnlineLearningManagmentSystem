using System;
using System.Collections.Generic;

public class Admin : User
{
    public int AdminID { get; private set; }
    private List<string> _permissions = new List<string>();

    public IReadOnlyList<string> Permissions => _permissions.AsReadOnly();

    private static List<Admin> _admins = new List<Admin>();

    public Admin(int userID, string name, string email, string password, int adminID)
        : base(userID, name, email, password)
    {
        AdminID = adminID;
        AddToExtent(this);
    }

    public void AddPermission(string permission)
    {
        if (string.IsNullOrEmpty(permission))
            throw new ArgumentException("Permission cannot be empty.");
        _permissions.Add(permission);
    }

    public static void AddToExtent(Admin admin)
    {
        _admins.Add(admin ?? throw new ArgumentException("Admin cannot be null."));
    }

    public static IReadOnlyList<Admin> GetExtent() => _admins.AsReadOnly();

    public static void ClearExtent() => _admins.Clear();
}

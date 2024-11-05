using System;
using System.Collections.Generic;

public abstract class User
{
    private static List<User> _users = new List<User>();

    public int UserID { get; private set; }
    private string _name;
    private string _email;
    private string _password;

    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("User name cannot be empty.");
            _name = value;
        }
    }

    public string Email
    {
        get => _email;
        set
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Email cannot be empty.");
            _email = value;
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Password cannot be empty.");
            _password = value;
        }
    }

    protected User(int userID, string name, string email, string password)
    {
        UserID = userID;
        Name = name;
        Email = email;
        Password = password;
        AddUser(this);
    }

    public static void AddUser(User user)
    {
        if (user == null)
            throw new ArgumentException("User cannot be null.");
        _users.Add(user);
    }

    public static IReadOnlyList<User> GetAllUsers()
    {
        return _users.AsReadOnly();
    }
}

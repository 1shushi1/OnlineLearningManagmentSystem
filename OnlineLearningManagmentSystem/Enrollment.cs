using System;
using System.Collections.Generic;

public class Enrollment
{
    public int EnrollmentID { get; private set; }
    public DateTime EnrollmentDate { get; private set; }
    private string _status;

    public string Status
    {
        get => _status;
        set
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Status cannot be empty.");
            _status = value;
        }
    }

    private static List<Enrollment> _enrollments = new List<Enrollment>();

    public Enrollment(int enrollmentID, string status)
    {
        EnrollmentID = enrollmentID;
        EnrollmentDate = DateTime.Now;
        Status = status;
        AddToExtent(this);
    }

    public static void AddToExtent(Enrollment enrollment)
    {
        _enrollments.Add(enrollment ?? throw new ArgumentException("Enrollment cannot be null."));
    }

    public static IReadOnlyList<Enrollment> GetExtent() => _enrollments.AsReadOnly();

    public static void ClearExtent() => _enrollments.Clear();
}

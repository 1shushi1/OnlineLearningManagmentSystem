using System;
using System.Collections.Generic;

public class Course
{
    public int CourseID { get; private set; }
    private string _title;
    private string _description;
    private List<Enrollment> _enrollments = new List<Enrollment>();
    private List<Payment> _payments = new List<Payment>();

    public string Title
    {
        get => _title;
        set
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Course title cannot be empty.");
            _title = value;
        }
    }

    public string Description
    {
        get => _description;
        set
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Course description cannot be empty.");
            _description = value;
        }
    }

    public int CurrentEnrollment => _enrollments.Count;

    private static List<Course> _courses = new List<Course>();

    public Course(int courseID, string title, string description)
    {
        CourseID = courseID;
        Title = title;
        Description = description;
        AddToExtent(this);
    }

    public void AddEnrollment(Enrollment enrollment)
    {
        if (enrollment == null)
            throw new ArgumentException("Enrollment cannot be null.");
        _enrollments.Add(enrollment);
    }

    public void AddPayment(Payment payment)
    {
        if (payment == null)
            throw new ArgumentException("Payment cannot be null.");
        _payments.Add(payment);
    }

    public static void AddToExtent(Course course)
    {
        _courses.Add(course ?? throw new ArgumentException("Course cannot be null."));
    }

    public static IReadOnlyList<Course> GetExtent() => _courses.AsReadOnly();

    public static void ClearExtent() => _courses.Clear();
}

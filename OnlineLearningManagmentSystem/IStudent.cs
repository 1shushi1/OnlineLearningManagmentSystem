public interface IStudent
{
    int StudentID { get; }
    void SubmitAssignment(Assignment assignment);
    void AddEnrollment(Enrollment enrollment);
}

public interface IInstructor
{
    int InstructorID { get; }
    void AssignCourse(Course course);
}

public class TeachingAssistant : User, IStudent, IInstructor
{
    public int StudentID { get; private set; }
    public int InstructorID { get; private set; }
    public string Expertise { get; private set; }
    private List<Enrollment> enrollments = new List<Enrollment>();
    private List<Course> courses = new List<Course>();

    public TeachingAssistant(int userID, string name, string email, string password, int studentID, int instructorID, string expertise)
        : base(userID, name, email, password)
    {
        StudentID = studentID;
        InstructorID = instructorID;
        Expertise = expertise;
    }

    public void SubmitAssignment(Assignment assignment)
    {
        Console.WriteLine($"{Name} (TA) has submitted an assignment.");
    }

    public void AddEnrollment(Enrollment enrollment)
    {
        enrollments.Add(enrollment);
    }

    public void AssignCourse(Course course)
    {
        courses.Add(course);
    }
}

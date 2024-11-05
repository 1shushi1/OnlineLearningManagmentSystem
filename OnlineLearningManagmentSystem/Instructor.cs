public class Instructor : User
{
    public int InstructorID { get; private set; }
    private string _expertise;
    private List<Course> _courses = new List<Course>();

    public string Expertise
    {
        get => _expertise;
        set
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Expertise cannot be empty.");
            _expertise = value;
        }
    }

    public IReadOnlyList<Course> Courses => _courses.AsReadOnly();

    public Instructor(int userID, string name, string email, string password, int instructorID, string expertise)
        : base(userID, name, email, password)
    {
        InstructorID = instructorID;
        Expertise = expertise;
    }

    public void AssignCourse(Course course)
    {
        if (course == null)
            throw new ArgumentException("Course cannot be null.");
        _courses.Add(course);
    }
}

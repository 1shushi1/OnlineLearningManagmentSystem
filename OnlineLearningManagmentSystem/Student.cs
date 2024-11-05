public class Student : User
{
    public int StudentID { get; private set; }

    private List<Enrollment> _enrollments = new List<Enrollment>();
    private List<SubmittedAssignment> _submittedAssignments = new List<SubmittedAssignment>();
    private List<Payment> _payments = new List<Payment>();

    public IReadOnlyList<Enrollment> Enrollments => _enrollments.AsReadOnly();
    public IReadOnlyList<SubmittedAssignment> SubmittedAssignments => _submittedAssignments.AsReadOnly();
    public IReadOnlyList<Payment> Payments => _payments.AsReadOnly();

    public Student(int userID, string name, string email, string password, int studentID)
        : base(userID, name, email, password)
    {
        if (studentID <= 0)
            throw new ArgumentException("Student ID must be positive.");
        StudentID = studentID;
    }

    public void AddEnrollment(Enrollment enrollment)
    {
        if (enrollment == null)
            throw new ArgumentException("Enrollment cannot be null.");
        _enrollments.Add(enrollment);
    }

    public void AddSubmittedAssignment(SubmittedAssignment assignment)
    {
        if (assignment == null)
            throw new ArgumentException("Assignment cannot be null.");
        _submittedAssignments.Add(assignment);
    }

    public void AddPayment(Payment payment)
    {
        if (payment == null)
            throw new ArgumentException("Payment cannot be null.");
        _payments.Add(payment);
    }
}

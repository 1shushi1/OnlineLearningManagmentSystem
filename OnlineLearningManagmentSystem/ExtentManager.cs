using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

public static class ExtentManager
{
    public static void SaveAll(string path = "extents.xml")
    {
        try
        {
            using (StreamWriter file = new StreamWriter(path))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ExtentContainer));
                ExtentContainer container = new ExtentContainer
                {
                    Users = User.GetAllUsers(),
                    Students = Student.GetExtent(),
                    Instructors = Instructor.GetExtent(),
                    Admins = Admin.GetExtent(),
                    Courses = Course.GetExtent(),
                    Lessons = Lesson.GetExtent(),
                    Quizzes = Quiz.GetExtent(),
                    Questions = Question.GetExtent(),
                    Enrollments = Enrollment.GetExtent(),
                    Grades = Grade.GetExtent(),
                    Certificates = Certificate.GetExtent(),
                    SubmittedAssignments = SubmittedAssignment.GetExtent(),
                    Payments = Payment.GetExtent()
                };
                serializer.Serialize(file, container);
                Console.WriteLine("All extents saved successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving extents: {ex.Message}");
        }
    }

    public static void LoadAll(string path = "extents.xml")
    {
        try
        {
            if (!File.Exists(path))
            {
                Console.WriteLine("Extent file not found.");
                return;
            }

            using (StreamReader file = new StreamReader(path))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ExtentContainer));
                ExtentContainer container = (ExtentContainer)serializer.Deserialize(file);

                User.ClearExtent();
                foreach (var user in container.Users) User.AddToExtent(user);

                Student.ClearExtent();
                foreach (var student in container.Students) Student.AddToExtent(student);

                Instructor.ClearExtent();
                foreach (var instructor in container.Instructors) Instructor.AddToExtent(instructor);

                Admin.ClearExtent();
                foreach (var admin in container.Admins) Admin.AddToExtent(admin);

                Course.ClearExtent();
                foreach (var course in container.Courses) Course.AddToExtent(course);

                Lesson.ClearExtent();
                foreach (var lesson in container.Lessons) Lesson.AddToExtent(lesson);

                Quiz.ClearExtent();
                foreach (var quiz in container.Quizzes) Quiz.AddToExtent(quiz);

                Question.ClearExtent();
                foreach (var question in container.Questions) Question.AddToExtent(question);

                Enrollment.ClearExtent();
                foreach (var enrollment in container.Enrollments) Enrollment.AddToExtent(enrollment);

                Grade.ClearExtent();
                foreach (var grade in container.Grades) Grade.AddToExtent(grade);

                Certificate.ClearExtent();
                foreach (var certificate in container.Certificates) Certificate.AddToExtent(certificate);

                SubmittedAssignment.ClearExtent();
                foreach (var submission in container.SubmittedAssignments) SubmittedAssignment.AddToExtent(submission);

                Payment.ClearExtent();
                foreach (var payment in container.Payments) Payment.AddToExtent(payment);

                Console.WriteLine("All extents loaded successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading extents: {ex.Message}");
        }
    }
}

// Container class for XML serialization
[Serializable]
public class ExtentContainer
{
    public List<User> Users { get; set; } = new List<User>();
    public List<Student> Students { get; set; } = new List<Student>();
    public List<Instructor> Instructors { get; set; } = new List<Instructor>();
    public List<Admin> Admins { get; set; } = new List<Admin>();
    public List<Course> Courses { get; set; } = new List<Course>();
    public List<Lesson> Lessons { get; set; } = new List<Lesson>();
    public List<Quiz> Quizzes { get; set; } = new List<Quiz>();
    public List<Question> Questions { get; set; } = new List<Question>();
    public List<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public List<Grade> Grades { get; set; } = new List<Grade>();
    public List<Certificate> Certificates { get; set; } = new List<Certificate>();
    public List<SubmittedAssignment> SubmittedAssignments { get; set; } = new List<SubmittedAssignment>();
    public List<Payment> Payments { get; set; } = new List<Payment>();
}

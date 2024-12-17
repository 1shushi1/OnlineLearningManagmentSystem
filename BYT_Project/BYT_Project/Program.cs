using System;

namespace BYT_Project
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {

                // User creation
                var user = new User(1, "John Doe", "john.doe@example.com", "securePassword123");
                Console.WriteLine($"User Created: {user.Name}, Email: {user.Email}");

                // Student creation
                var student = new Student(101);
                Console.WriteLine($"Student Created: ID {student.StudentID}");

                // Instructor creation
                var instructor = new Instructor(201, "Computer Science", "MWF 2-4PM");
                Console.WriteLine($"Instructor Created: Expertise in {instructor.Expertise}, Office Hours: {instructor.OfficeHours}");

                // TeachingAssistant creation
                var ta = new TeachingAssistant(301, 2);
                Console.WriteLine($"Teaching Assistant Created: ID {ta.TeachingAssistantID}, Experience: {ta.Experience} years");

                // Admin creation
                var admin = new Admin(401, new List<string> { "Manage Users", "Manage Courses" });
                Console.WriteLine($"Admin Created: ID {admin.AdminID}, Permissions: {string.Join(", ", admin.Permissions)}");

                // Course creation
                var course = new Course(501, "C# Programming", "An introductory course to C# and .NET", 100);
                Console.WriteLine($"Course Created: {course.Title} - {course.Description}, Max Enrollment: {course.MaxEnrollment}");

                // Lesson creation
                var lesson = new Lesson(601, "Introduction to C#", "https://example.com/video", "Basic concepts of C# programming.");
                Console.WriteLine($"Lesson Created: {lesson.LessonTitle}, Video URL: {lesson.VideoURL}, Description: {lesson.LessonDescription}");

                // Assignment creation
                var assignment = new Assignment(701, "Homework 1", "Complete exercises 1-5", DateTime.Now.AddDays(7), 100);
                Console.WriteLine($"Assignment Created: {assignment.Title}, Due Date: {assignment.DueDate}, Max Score: {assignment.MaxScore}");

                // Quiz creation
                var quiz = new Quiz(801, "Quiz 1", 50, 30);
                Console.WriteLine($"Quiz Created: {quiz.Title}, Total Score: {quiz.TotalScore}, Pass Mark: {quiz.PassMark}");

                // Question creation
                var question = new Question(901, "What is 2 + 2?", new List<string> { "1", "2", "3", "4" }, "4", "MultipleChoice");
                Console.WriteLine($"Question Created: {question.Text}, Options: {string.Join(", ", question.Options)}, Correct Answer: {question.CorrectAnswer}");

                // Enrollment creation
                //var enrollment = new Enrollment(_student, _course, 1001, DateTime.Now, "Active", 85);
                //Console.WriteLine($"Enrollment Created: ID {enrollment.EnrollmentID}, Status: {enrollment.Status}, Total Score: {enrollment.TotalScore}, Grade: {enrollment.GradeLetter}");

                // Timetable creation
                var timetable = new Timetable(1101, DateTime.Now, DateTime.Now.AddMonths(1), new List<string> { "MWF 9-11AM" });
                Console.WriteLine($"Timetable Created: Start Date: {timetable.StartDate}, End Date: {timetable.EndDate}, Schedule: {string.Join(", ", timetable.Schedule)}");

                // Payment creation
                var payment = new Payment(1201, 299.99, DateTime.Now);
                Console.WriteLine($"Payment Created: Amount: ${payment.Amount}, Date: {payment.PaymentDate}");

                // Certificate creation
                var certificate = new Certificate(1301, DateTime.Now, "Course Completion Certificate");
                Console.WriteLine($"Certificate Created: Completion Date: {certificate.CompletionDate}, Description: {certificate.CertificateDescription}");

                // SubmittedAssignment creation
                //var submittedAssignment = new SubmittedAssignment(_student, _assignment, 1401, DateTime.Now);
                //Console.WriteLine($"Submitted Assignment Created: ID {submittedAssignment.SubmissionID}, Submission Date: {submittedAssignment.SubmissionDate}");

                // Saving and Loading Objects
                Console.WriteLine("\nSaving and Loading Objects...");

                // Save and Load Users
                User.SaveUsers("users.xml");
                User.LoadUsers("users.xml");
                Console.WriteLine($"Loaded Users: {User.UsersList.Count}");

                // Save and Load Students
                Student.SaveStudents("students.xml");
                Student.LoadStudents("students.xml");
                Console.WriteLine($"Loaded Students: {Student.StudentsList.Count}");

                // Save and Load Instructors
                Instructor.SaveInstructors("instructors.xml");
                Instructor.LoadInstructors("instructors.xml");
                Console.WriteLine($"Loaded Instructors: {Instructor.InstructorsList.Count}");

                // Save and Load Teaching Assistants
                TeachingAssistant.SaveTeachingAssistants("teachingAssistants.xml");
                TeachingAssistant.LoadTeachingAssistants("teachingAssistants.xml");
                Console.WriteLine($"Loaded Teaching Assistants: {TeachingAssistant.TeachingAssistantsList.Count}");

                // Save and Load Admins
                Admin.SaveAdmins("admins.xml");
                Admin.LoadAdmins("admins.xml");
                Console.WriteLine($"Loaded Admins: {Admin.AdminsList.Count}");

                // Save and Load Courses
                Course.SaveCourses("courses.xml");
                Course.LoadCourses("courses.xml");
                Console.WriteLine($"Loaded Courses: {Course.CoursesList.Count}");

                // Save and Load Lessons
                Lesson.SaveLessons("lessons.xml");
                Lesson.LoadLessons("lessons.xml");
                Console.WriteLine($"Loaded Lessons: {Lesson.LessonsList.Count}");

                // Save and Load Assignments
                Assignment.SaveAssignments("assignments.xml");
                Assignment.LoadAssignments("assignments.xml");
                Console.WriteLine($"Loaded Assignments: {Assignment.AssignmentsList.Count}");

                // Save and Load Quizzes
                Quiz.SaveQuizzes("quizzes.xml");
                Quiz.LoadQuizzes("quizzes.xml");
                Console.WriteLine($"Loaded Quizzes: {Quiz.QuizzesList.Count}");

                // Save and Load Questions
                Question.SaveQuestions("questions.xml");
                Question.LoadQuestions("questions.xml");
                Console.WriteLine($"Loaded Questions: {Question.QuestionList.Count}");

                // Save and Load Enrollments
                Enrollment.SaveEnrollments("enrollments.xml");
                Enrollment.LoadEnrollments("enrollments.xml");
                Console.WriteLine($"Loaded Enrollments: {Enrollment.EnrollmentsList.Count}");

                // Save and Load Timetables
                Timetable.SaveTimetables("timetables.xml");
                Timetable.LoadTimetables("timetables.xml");
                Console.WriteLine($"Loaded Timetables: {Timetable.TimetableList.Count}");

                // Save and Load Payments
                Payment.SavePayments("payments.xml");
                Payment.LoadPayments("payments.xml");
                Console.WriteLine($"Loaded Payments: {Payment.PaymentsList.Count}");

                // Save and Load Certificates
                Certificate.SaveCertificates("certificates.xml");
                Certificate.LoadCertificates("certificates.xml");
                Console.WriteLine($"Loaded Certificates: {Certificate.CertificatesList.Count}");

                // Save and Load SubmittedAssignments
                SubmittedAssignment.SaveSubmissions("submittedAssignments.xml");
                SubmittedAssignment.LoadSubmissions("submittedAssignments.xml");
                Console.WriteLine($"Loaded Submitted Assignments: {SubmittedAssignment.SubmissionsList.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}

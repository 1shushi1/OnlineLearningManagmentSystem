using BYT_Project;

namespace Project_Tests.Relation_Tests
{
    [TestFixture]
    public class SubmittedAssignmentAssociationWithAttributeTests
    {
        private Student _student;
        private Assignment _assignment;

        [SetUp]
        public void Setup()
        {
            typeof(SubmittedAssignment)
                .GetField("submissionsList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, new List<SubmittedAssignment>());

            typeof(Student)
                .GetField("studentsList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, new List<Student>());

            typeof(Assignment)
                .GetField("coursesList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, new List<Assignment>());

            _student = new Student(1);
            _assignment = new Assignment(1, "Math Homework", "Solve equations", DateTime.Now.AddDays(1), 100);
        }
    }
}

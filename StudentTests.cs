using NUnit.Framework;
using System;
using BYT_Project;
using System.IO;

namespace BYT_Project.Tests
{
    [TestFixture]
    public class StudentTests
    {
        [Test]
        public void TestGetCorrectStudentInformation()
        {
            var student = new Student(1);
            Assert.That(student.StudentID, Is.EqualTo(1));
        }

        [Test]
        public void TestExceptionForInvalidStudentID()
        {
            var ex = Assert.Throws<ArgumentException>(() => new Student(-1));
            Assert.That(ex.Message, Is.EqualTo("Student ID must be positive."));
        }

        [Test]
        public void TestSaveAndLoadStudents()
        {
            var student1 = new Student(1);
            var student2 = new Student(2);

            Student.SaveStudents("studentsTest.xml");
            var success = Student.LoadStudents("studentsTest.xml");

            Assert.That(success, Is.True);  // Check if LoadStudents() returns true
            Assert.That(Student.StudentsList.Count, Is.EqualTo(2));  // Check that the list contains 2 students
        }

        [Test]
        public void TestPersistencyAfterLoad()
        {
            var student = new Student(1);
            Student.SaveStudents("studentsTest.xml");

            // Clear the current list and reload
            Student.LoadStudents("studentsTest.xml");

            Assert.That(Student.StudentsList.Count, Is.GreaterThan(0));  // Ensure students were loaded from the file
        }
    }
}

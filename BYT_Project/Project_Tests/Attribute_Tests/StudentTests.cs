using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using BYT_Project;

namespace BYT_Project.Tests
{
    [TestFixture]
    public class StudentTests
    {
        [SetUp]
        public void SetUp()
        {
            typeof(Student)
                .GetField("studentsList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, new List<Student>());

            if (File.Exists("student.xml"))
            {
                File.Delete("student.xml");
            }
        }

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
            Assert.That(Student.StudentsList.Count, Is.EqualTo(2));

            Student.SaveStudents("student.xml");

            typeof(Student)
                .GetField("studentsList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, new List<Student>());

            var success = Student.LoadStudents("student.xml");

            Assert.That(success, Is.True);
            Assert.That(Student.StudentsList.Count, Is.EqualTo(2));
        }
    }
}

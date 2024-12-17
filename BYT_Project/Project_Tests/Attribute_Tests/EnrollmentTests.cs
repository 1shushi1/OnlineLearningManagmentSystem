using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using BYT_Project;

namespace BYT_Project.Tests
{
    [TestFixture]
    public class EnrollmentTests
    {
        private Student _student;
        private Course _course;

        [SetUp]
        public void SetUp()
        {
            typeof(Enrollment)
                .GetField("enrollmentsList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, new List<Enrollment>());

            typeof(Student)
                .GetField("studentsList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, new List<Student>());

            typeof(Course)
                .GetField("coursesList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, new List<Course>());

            if (File.Exists("enrollment.xml"))
            {
                File.Delete("enrollment.xml");
            }

            _student = new Student(1);
            _course = new Course(101, "Math 101", "Basic Math", 30);
        }

        [Test]
        public void TestGetCorrectEnrollmentInformation()
        {
            var enrollmentDate = DateTime.Now;
            var enrollment = new Enrollment(_student, _course, 1, enrollmentDate, "Active", 85);

            Assert.That(enrollment.Student, Is.EqualTo(_student));
            Assert.That(enrollment.Course, Is.EqualTo(_course));
            Assert.That(enrollment.EnrollmentID, Is.EqualTo(1));
            Assert.That(enrollment.EnrollmentDate, Is.EqualTo(enrollmentDate));
            Assert.That(enrollment.Status, Is.EqualTo("Active"));
            Assert.That(enrollment.TotalScore, Is.EqualTo(85));
            Assert.That(enrollment.GradeLetter, Is.EqualTo("B"));
        }

        [Test]
        public void TestExceptionForInvalidEnrollmentID()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new Enrollment(null, _course, 1, DateTime.Now, "Active", 85);
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                new Enrollment(_student, null, 1, DateTime.Now, "Active", 85);
            });
        }

        [Test]
        public void TestExceptionForInvalidTotalScore()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new Enrollment(_student, _course, 1,DateTime.Now, "Active", 150);
            });

            Assert.Throws<ArgumentException>(() =>
            {
                new Enrollment(_student, _course, 1,DateTime.Now, "Active", -10);
            });
        }

        [Test]
        public void TestSaveAndLoadEnrollments()
        {
            var enrollment = new Enrollment(_student, _course, 1, DateTime.Now, "Active", 85);

            Assert.That(Enrollment.EnrollmentsList.Count, Is.EqualTo(1));

            Enrollment.SaveEnrollments("enrollment.xml");

            typeof(Enrollment)
                .GetField("enrollmentsList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, new List<Enrollment>());

            var success = Enrollment.LoadEnrollments("enrollment.xml");

            Assert.That(success, Is.True);
            Assert.That(Enrollment.EnrollmentsList.Count, Is.EqualTo(1));
        }
    }
}

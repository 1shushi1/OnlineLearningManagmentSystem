using NUnit.Framework;
using System;
using System.Collections.Generic;
using BYT_Project;

namespace Project_Tests.Relation_Tests
{
    [TestFixture]
    public class EnrollmentAssociationWithAttributeTests
    {
        private Student _student;
        private Course _course;

        [SetUp]
        public void Setup()
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

            _student = new Student(1);
            _course = new Course(101, "Math 101", "Basic Math", 30);
        }

        [Test]
        public void TestEnrollmentCreation()
        {
            var enrollment = new Enrollment(_student, _course, 1, DateTime.Now, "Active", 85);

            Assert.Multiple(() =>
            {
                Assert.That(_student.Courses.Contains(_course), "Student should reference the enrolled course.");
                Assert.That(_course.Students.Contains(_student), "Course should reference the enrolled student.");
            });
        }

        [Test]
        public void TestDuplicateEnrollmentThrowsException()
        {
            new Enrollment(_student, _course, 1, DateTime.Now, "Active", 85);

            var ex = Assert.Throws<ArgumentException>(() =>
            {
                new Enrollment(_student, _course, 1, DateTime.Now, "Active", 90);
            });
            Assert.That(ex.Message, Is.EqualTo("This student is already enrolled in the course."));
        }

        [Test]
        public void TestEnrollmentReverseConnection()
        {
            var enrollment = new Enrollment(_student, _course, 1, DateTime.Now, "Active", 85);

            Assert.Multiple(() =>
            {
                Assert.That(_student.Courses.Contains(_course), "Student should reference the enrolled course.");
                Assert.That(_course.Students.Contains(_student), "Course should reference the enrolled student.");
            });
        }

        [Test]
        public void TestRemovingCourseFromEnrollment()
        {
            var enrollment = new Enrollment(_student, _course, 1, DateTime.Now, "Active", 85);
            Enrollment.RemoveEnrollment(enrollment);

            Assert.Multiple(() =>
            {
                Assert.That(!_student.Courses.Contains(_course), "Student should no longer reference the course.");
                Assert.That(!_course.Students.Contains(_student), "Course should no longer reference the student.");
                Assert.That(!Enrollment.EnrollmentsList.Contains(enrollment), "Enrollment should be removed from the global list.");
            });
        }

        [Test]
        public void TestRemovingEnrollmentFromGlobalList()
        {
            var enrollment = new Enrollment(_student, _course, 1,DateTime.Now, "Active", 85);

            Enrollment.RemoveEnrollment(enrollment);

            Assert.That(!Enrollment.EnrollmentsList.Contains(enrollment), "Enrollment should be removed from the global list.");
        }

        [Test]
        public void TestEnrollmentSerialization()
        {
            var student = new Student(1);
            var course = new Course(101, "Math", "Math Course", 30);
            var enrollment = new Enrollment(student, course, 1,DateTime.Now, "Active", 85);

            Enrollment.SaveEnrollments("enrollment.xml");

            // Clear the enrollments list to simulate a fresh load
            typeof(Enrollment)
                .GetField("enrollmentsList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, new List<Enrollment>());

            var success = Enrollment.LoadEnrollments("enrollment.xml");

            Assert.Multiple(() =>
            {
                Assert.That(success, Is.True, "Enrollments should be successfully loaded.");
                Assert.That(Enrollment.EnrollmentsList.Count, Is.EqualTo(1), "One enrollment should be loaded.");
            });
        }
    }
}

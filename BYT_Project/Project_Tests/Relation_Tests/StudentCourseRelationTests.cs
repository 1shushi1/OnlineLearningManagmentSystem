using BYT_Project;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Project_Tests.Relation_Tests
{
    [TestFixture]
    public class StudentCourseRelationTests
    {
        [SetUp]
        public void Setup()
        {
            typeof(Student)
                .GetField("studentsList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, new List<Student>());

            typeof(Course)
                .GetField("coursesList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, new List<Course>());
        }

        [Test]
        public void TestAddingCourseToStudent()
        {
            var student = new Student(1);
            var course = new Course(1, "Math 101", "Basic Math Course", 30);

            student.AddCourse(course);

            Assert.That(student.Courses.Contains(course), "Course not added to student.");
            Assert.That(course.Students.Contains(student), "Student not added to course.");
        }

        [Test]
        public void TestRemovingCourseFromStudent()
        {
            var student = new Student(1);
            var course = new Course(1, "Math 101", "Basic Math Course", 30);

            student.AddCourse(course);
            student.RemoveCourse(course);

            Assert.That(!student.Courses.Contains(course), "Course not removed from student.");
            Assert.That(!course.Students.Contains(student), "Student not removed from course.");
        }

        [Test]
        public void TestReverseConnectionIntegrity()
        {
            var student = new Student(1);
            var course = new Course(1, "Math 101", "Basic Math Course", 30);

            student.AddCourse(course);
            course.RemoveStudent(student);

            Assert.That(!student.Courses.Contains(course), "Course still connected to student.");
            Assert.That(!course.Students.Contains(student), "Student still connected to course.");
        }

        [Test]
        public void TestUpdatingCourseForStudent()
        {
            var student = new Student(1);
            var oldCourse = new Course(1, "Math 101", "Basic Math Course", 30);
            var newCourse = new Course(2, "Science 101", "Basic Science Course", 25);

            student.AddCourse(oldCourse);
            student.UpdateCourse(oldCourse, newCourse);

            Assert.Multiple(() =>
            {
                Assert.That(!student.Courses.Contains(oldCourse), "Old course still associated with student.");
                Assert.That(student.Courses.Contains(newCourse), "New course not associated with student.");
                Assert.That(!oldCourse.Students.Contains(student), "Student still connected to old course.");
                Assert.That(newCourse.Students.Contains(student), "Student not connected to new course.");
            });
        }

        [Test]
        public void TestUpdatingStudentForCourse()
        {
            var course = new Course(1, "Math 101", "Basic Math Course", 30);
            var oldStudent = new Student(1);
            var newStudent = new Student(2);

            course.AddStudent(oldStudent);
            course.UpdateStudent(oldStudent, newStudent);

            Assert.Multiple(() =>
            {
                Assert.That(!course.Students.Contains(oldStudent), "Old student still associated with course.");
                Assert.That(course.Students.Contains(newStudent), "New student not associated with course.");
                Assert.That(!oldStudent.Courses.Contains(course), "Course still connected to old student.");
                Assert.That(newStudent.Courses.Contains(course), "Course not connected to new student.");
            });
        }

        [Test]
        public void TestErrorHandlingForNullInUpdateMethods()
        {
            var student = new Student(1);
            var course = new Course(1, "Math 101", "Basic Math Course", 30);

            var ex1 = Assert.Throws<ArgumentException>(() => student.UpdateCourse(course, null));
            Assert.That(ex1.Message, Is.EqualTo("Both old and new courses must be provided."));

            var ex2 = Assert.Throws<ArgumentException>(() => course.UpdateStudent(null, student));
            Assert.That(ex2.Message, Is.EqualTo("Both old and new students must be provided."));
        }

        [Test]
        public void TestRemovingCourseAfterUpdate()
        {
            var student = new Student(1);
            var oldCourse = new Course(1, "Math 101", "Basic Math Course", 30);
            var newCourse = new Course(2, "Science 101", "Basic Science Course", 25);

            student.AddCourse(oldCourse);
            student.UpdateCourse(oldCourse, newCourse);
            student.RemoveCourse(newCourse);

            Assert.Multiple(() =>
            {
                Assert.That(student.Courses.Count, Is.EqualTo(0), "Student still associated with a course after removal.");
                Assert.That(newCourse.Students.Count, Is.EqualTo(0), "Course still connected to a student after removal.");
            });
        }

        [Test]
        public void TestErrorWhenAddingNullCourseToStudent()
        {
            var student = new Student(1);

            var ex = Assert.Throws<ArgumentException>(() => student.AddCourse(null));
            Assert.That(ex.Message, Is.EqualTo("Course cannot be null."));
        }

        [Test]
        public void TestErrorWhenAddingDuplicateCourseToStudent()
        {
            var student = new Student(1);
            var course = new Course(1, "Math 101", "Basic Math Course", 30);

            student.AddCourse(course);

            var ex = Assert.Throws<ArgumentException>(() => student.AddCourse(course));
            Assert.That(ex.Message, Is.EqualTo("Course is already added to this student."));
        }

        [Test]
        public void TestErrorWhenRemovingNonExistingCourseFromStudent()
        {
            var student = new Student(1);
            var course = new Course(1, "Math 101", "Basic Math Course", 30);

            var ex = Assert.Throws<ArgumentException>(() => student.RemoveCourse(course));
            Assert.That(ex.Message, Is.EqualTo("Course is not added to this student."));
        }

        [Test]
        public void TestErrorWhenAssigningCourseToDifferentStudent()
        {
            var student1 = new Student(1);
            var student2 = new Student(2);
            var course = new Course(1, "Math 101", "Basic Math Course", 30);

            student1.AddCourse(course);

            var ex = Assert.Throws<ArgumentException>(() => student2.AddCourse(course));
            Assert.That(ex.Message, Is.EqualTo("Course is already assigned to a student."));
        }

        [Test]
        public void TestAddingCourseToStudentReverseCheck()
        {
            var student = new Student(1);
            var course = new Course(1, "Math 101", "Basic Math Course", 30);

            student.AddCourse(course);

            Assert.Multiple(() =>
            {
                Assert.That(course.Students.Contains(student), "Reverse connection is not correctly established.");
                Assert.That(student.Courses.Contains(course), "Course is not associated with student.");
            });
        }

        [Test]
        public void TestRemovingStudentFromCourseReverseCheck()
        {
            var student = new Student(1);
            var course = new Course(1, "Math 101", "Basic Math Course", 30);

            student.AddCourse(course);
            student.RemoveCourse(course);

            Assert.Multiple(() =>
            {
                Assert.That(!course.Students.Contains(student), "Course still connected to student.");
                Assert.That(!student.Courses.Contains(course), "Student still associated with course.");
            });
        }
    }
}

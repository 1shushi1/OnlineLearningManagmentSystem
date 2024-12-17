using BYT_Project;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Project_Tests.Relation_Tests
{
    [TestFixture]
    public class StudentAssignmentRelationTests
    {
        [SetUp]
        public void Setup()
        {
            typeof(Student)
                .GetField("studentsList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, new List<Student>());

            typeof(Assignment)
                .GetField("assignmentsList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, new List<Assignment>());
        }

        [Test]
        public void TestAddingAssignmentToStudent()
        {
            var student = new Student(1);
            var assignment = new Assignment(1, "Math Homework", "Solve equations", DateTime.Now.AddDays(7), 100);

            student.AddAssignment(assignment);

            Assert.That(student.Assignments.Contains(assignment), "Assignment not added to student.");
            Assert.That(assignment.Students.Contains(student), "Student not added to assignment.");
        }

        [Test]
        public void TestRemovingAssignmentFromStudent()
        {
            var student = new Student(1);
            var assignment = new Assignment(1, "Math Homework", "Solve equations", DateTime.Now.AddDays(7), 100);

            student.AddAssignment(assignment);
            student.RemoveAssignment(assignment);

            Assert.That(!student.Assignments.Contains(assignment), "Assignment not removed from student.");
            Assert.That(!assignment.Students.Contains(student), "Student not removed from assignment.");
        }

        [Test]
        public void TestReverseConnectionIntegrity()
        {
            var student = new Student(1);
            var assignment = new Assignment(1, "Math Homework", "Solve equations", DateTime.Now.AddDays(7), 100);

            student.AddAssignment(assignment);

            assignment.RemoveStudent(student);

            Assert.That(!student.Assignments.Contains(assignment), "Assignment still connected to student.");
            Assert.That(!assignment.Students.Contains(student), "Student still connected to assignment.");
        }

        [Test]
        public void TestUpdatingAssignmentForStudent()
        {
            var student = new Student(1);
            var oldAssignment = new Assignment(1, "Math Homework", "Solve equations", DateTime.Now.AddDays(7), 100);
            var newAssignment = new Assignment(2, "Science Homework", "Write a lab report", DateTime.Now.AddDays(7), 100);

            student.AddAssignment(oldAssignment);
            student.UpdateAssignment(oldAssignment, newAssignment);

            Assert.Multiple(() =>
            {
                Assert.That(!student.Assignments.Contains(oldAssignment), "Old assignment still associated with student.");
                Assert.That(student.Assignments.Contains(newAssignment), "New assignment not associated with student.");
                Assert.That(!oldAssignment.Students.Contains(student), "Student still connected to old assignment.");
                Assert.That(newAssignment.Students.Contains(student), "Student not connected to new assignment.");
            });
        }

        [Test]
        public void TestUpdatingStudentForAssignment()
        {
            var assignment = new Assignment(1, "Math Homework", "Solve equations", DateTime.Now.AddDays(7), 100);
            var oldStudent = new Student(1);
            var newStudent = new Student(2);

            assignment.AddStudent(oldStudent);
            assignment.UpdateStudent(oldStudent, newStudent);

            Assert.Multiple(() =>
            {
                Assert.That(!assignment.Students.Contains(oldStudent), "Old student still associated with assignment.");
                Assert.That(assignment.Students.Contains(newStudent), "New student not associated with assignment.");
                Assert.That(!oldStudent.Assignments.Contains(assignment), "Assignment still connected to old student.");
                Assert.That(newStudent.Assignments.Contains(assignment), "Assignment not connected to new student.");
            });
        }

        [Test]
        public void TestErrorHandlingForNullInUpdateMethods()
        {
            var student = new Student(1);
            var assignment = new Assignment(1, "Math Homework", "Solve equations", DateTime.Now.AddDays(7), 100);

            var ex1 = Assert.Throws<ArgumentException>(() => student.UpdateAssignment(assignment, null));
            Assert.That(ex1.Message, Is.EqualTo("Both old and new assignments must be provided."));

            var ex2 = Assert.Throws<ArgumentException>(() => assignment.UpdateStudent(null, student));
            Assert.That(ex2.Message, Is.EqualTo("Both old and new students must be provided."));
        }

        [Test]
        public void TestRemovingAssignmentAfterUpdate()
        {
            var student = new Student(1);
            var oldAssignment = new Assignment(1, "Math Homework", "Solve equations", DateTime.Now.AddDays(7), 100);
            var newAssignment = new Assignment(2, "Science Homework", "Write a lab report", DateTime.Now.AddDays(7), 100);

            student.AddAssignment(oldAssignment);
            student.UpdateAssignment(oldAssignment, newAssignment);
            student.RemoveAssignment(newAssignment);

            Assert.Multiple(() =>
            {
                Assert.That(student.Assignments.Count, Is.EqualTo(0), "Student still associated with an assignment after removal.");
                Assert.That(newAssignment.Students.Count, Is.EqualTo(0), "Assignment still connected to a student after removal.");
            });
        }

        [Test]
        public void TestErrorWhenAddingNullAssignmentToStudent()
        {
            var student = new Student(1);

            var ex = Assert.Throws<ArgumentException>(() => student.AddAssignment(null));
            Assert.That(ex.Message, Is.EqualTo("Assignment cannot be null."));
        }

        [Test]
        public void TestErrorWhenAddingNullStudentToAssignment()
        {
            var assignment = new Assignment(1, "Math Homework", "Solve equations", DateTime.Now.AddDays(7), 100);

            var ex = Assert.Throws<ArgumentException>(() => assignment.AddStudent(null));
            Assert.That(ex.Message, Is.EqualTo("Student cannot be null."));
        }

        [Test]
        public void TestErrorWhenRemovingNonExistingAssignmentFromStudent()
        {
            var student = new Student(1);
            var assignment = new Assignment(1, "Math Homework", "Solve equations", DateTime.Now.AddDays(7), 100);

            var ex = Assert.Throws<ArgumentException>(() => student.RemoveAssignment(assignment));
            Assert.That(ex.Message, Is.EqualTo("Assignment is not added to this student."));
        }

        [Test]
        public void TestErrorWhenRemovingNonExistingStudentFromAssignment()
        {
            var student = new Student(1);
            var assignment = new Assignment(1, "Math Homework", "Solve equations", DateTime.Now.AddDays(7), 100);

            var ex = Assert.Throws<ArgumentException>(() => assignment.RemoveStudent(student));
            Assert.That(ex.Message, Is.EqualTo("Student is not added to this assignment."));
        }

        [Test]
        public void TestErrorWhenAddingDuplicateAssignmentToStudent()
        {
            var student = new Student(1);
            var assignment = new Assignment(1, "Math Homework", "Solve equations", DateTime.Now.AddDays(7), 100);

            student.AddAssignment(assignment);

            var ex = Assert.Throws<ArgumentException>(() => student.AddAssignment(assignment));
            Assert.That(ex.Message, Is.EqualTo("Assignment is already added to this student."));
        }

        [Test]
        public void TestErrorWhenAddingDuplicateStudentToAssignment()
        {
            var student = new Student(1);
            var assignment = new Assignment(1, "Math Homework", "Solve equations", DateTime.Now.AddDays(7), 100);

            assignment.AddStudent(student);

            var ex = Assert.Throws<ArgumentException>(() => assignment.AddStudent(student));
            Assert.That(ex.Message, Is.EqualTo("Student is already added to this assignment."));
        }

        [Test]
        public void TestErrorWhenUpdatingAssignmentWithNull()
        {
            var student = new Student(1);
            var assignment = new Assignment(1, "Math Homework", "Solve equations", DateTime.Now.AddDays(7), 100);

            student.AddAssignment(assignment);

            var ex = Assert.Throws<ArgumentException>(() => student.UpdateAssignment(assignment, null));
            Assert.That(ex.Message, Is.EqualTo("Both old and new assignments must be provided."));
        }

        [Test]
        public void TestErrorWhenUpdatingStudentWithNull()
        {
            var assignment = new Assignment(1, "Math Homework", "Solve equations", DateTime.Now.AddDays(7), 100);
            var student = new Student(1);

            assignment.AddStudent(student);

            var ex = Assert.Throws<ArgumentException>(() => assignment.UpdateStudent(student, null));
            Assert.That(ex.Message, Is.EqualTo("Both old and new students must be provided."));
        }

        [Test]
        public void TestErrorWhenUpdatingNonExistingAssignmentForStudent()
        {
            var student = new Student(1);
            var oldAssignment = new Assignment(1, "Math Homework", "Solve equations", DateTime.Now.AddDays(7), 100);
            var newAssignment = new Assignment(2, "Science Homework", "Write a lab report", DateTime.Now.AddDays(7), 100);

            var ex = Assert.Throws<ArgumentException>(() => student.UpdateAssignment(oldAssignment, newAssignment));
            Assert.That(ex.Message, Is.EqualTo("Assignment is not added to this student."));
        }

        [Test]
        public void TestErrorWhenUpdatingNonExistingStudentForAssignment()
        {
            var assignment = new Assignment(1, "Math Homework", "Solve equations", DateTime.Now.AddDays(7), 100);
            var oldStudent = new Student(1);
            var newStudent = new Student(2);

            var ex = Assert.Throws<ArgumentException>(() => assignment.UpdateStudent(oldStudent, newStudent));
            Assert.That(ex.Message, Is.EqualTo("Student is not added to this assignment."));
        }
    }
}

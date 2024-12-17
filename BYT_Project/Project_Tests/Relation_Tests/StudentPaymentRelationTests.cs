using BYT_Project;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Project_Tests.Relation_Tests
{
    [TestFixture]
    public class StudentPaymentRelationTests
    {
        [SetUp]
        public void Setup()
        {
            typeof(Student)
                .GetField("studentsList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, new List<Student>());

            typeof(Payment)
                .GetField("paymentsList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, new List<Payment>());
        }

        [Test]
        public void TestAddingPaymentToStudent()
        {
            var student = new Student(1);
            var payment = new Payment(1, 500.0, DateTime.Now);

            student.AddPayment(payment);

            Assert.That(student.Payments.Contains(payment), "Payment not added to student.");
            Assert.That(payment.Student == student, "Student not added to payment.");
        }

        [Test]
        public void TestRemovingPaymentFromStudent()
        {
            var student = new Student(1);
            var payment = new Payment(1, 500.0, DateTime.Now);

            student.AddPayment(payment);
            student.RemovePayment(payment);

            Assert.That(!student.Payments.Contains(payment), "Payment not removed from student.");
            Assert.That(payment.Student == null, "Student not removed from payment.");
        }

        [Test]
        public void TestReverseConnectionIntegrity()
        {
            var student = new Student(1);
            var payment = new Payment(1, 500.0, DateTime.Now);

            student.AddPayment(payment);
            payment.RemoveStudent();

            Assert.That(!student.Payments.Contains(payment), "Payment still connected to student.");
            Assert.That(payment.Student == null, "Student still connected to payment.");
        }

        [Test]
        public void TestUpdatingPaymentForStudent()
        {
            var student = new Student(1);
            var oldPayment = new Payment(1, 500.0, DateTime.Now);
            var newPayment = new Payment(2, 300.0, DateTime.Now);

            student.AddPayment(oldPayment);
            student.UpdatePayment(oldPayment, newPayment);

            Assert.Multiple(() =>
            {
                Assert.That(!student.Payments.Contains(oldPayment), "Old payment still associated with student.");
                Assert.That(student.Payments.Contains(newPayment), "New payment not associated with student.");
                Assert.That(oldPayment.Student == null, "Student still connected to old payment.");
                Assert.That(newPayment.Student == student, "Student not connected to new payment.");
            });
        }

        [Test]
        public void TestErrorHandlingForNullInUpdateMethods()
        {
            var student = new Student(1);
            var payment = new Payment(1, 500.0, DateTime.Now);

            var ex1 = Assert.Throws<ArgumentException>(() => student.UpdatePayment(payment, null));
            Assert.That(ex1.Message, Is.EqualTo("Both old and new payments must be provided."));

            var ex2 = Assert.Throws<ArgumentException>(() => payment.AssignToStudent(null));
            Assert.That(ex2.Message, Is.EqualTo("Student cannot be null."));
        }

        [Test]
        public void TestRemovingPaymentAfterUpdate()
        {
            var student = new Student(1);
            var oldPayment = new Payment(1, 500.0, DateTime.Now);
            var newPayment = new Payment(2, 300.0, DateTime.Now);

            student.AddPayment(oldPayment);
            student.UpdatePayment(oldPayment, newPayment);
            student.RemovePayment(newPayment);

            Assert.Multiple(() =>
            {
                Assert.That(student.Payments.Count, Is.EqualTo(0), "Student still associated with a payment after removal.");
                Assert.That(newPayment.Student == null, "Payment still connected to a student after removal.");
            });
        }

        [Test]
        public void TestErrorWhenAddingNullPaymentToStudent()
        {
            var student = new Student(1);

            var ex = Assert.Throws<ArgumentException>(() => student.AddPayment(null));
            Assert.That(ex.Message, Is.EqualTo("Payment cannot be null."));
        }

        [Test]
        public void TestErrorWhenAddingDuplicatePaymentToStudent()
        {
            var student = new Student(1);
            var payment = new Payment(1, 500.0, DateTime.Now);

            student.AddPayment(payment);

            var ex = Assert.Throws<ArgumentException>(() => student.AddPayment(payment));
            Assert.That(ex.Message, Is.EqualTo("Payment is already added to this student."));
        }

        [Test]
        public void TestErrorWhenRemovingNonExistingPaymentFromStudent()
        {
            var student = new Student(1);
            var payment = new Payment(1, 500.0, DateTime.Now);

            var ex = Assert.Throws<ArgumentException>(() => student.RemovePayment(payment));
            Assert.That(ex.Message, Is.EqualTo("Payment is not added to this student."));
        }

        [Test]
        public void TestErrorWhenAssigningPaymentToDifferentStudent()
        {
            var student1 = new Student(1);
            var student2 = new Student(2);
            var payment = new Payment(1, 500.0, DateTime.Now);

            student1.AddPayment(payment);

            var ex = Assert.Throws<ArgumentException>(() => student2.AddPayment(payment));
            Assert.That(ex.Message, Is.EqualTo("Payment is already assigned to a student."));
        }


        [Test]
        public void TestErrorWhenUpdatingPaymentWithNull()
        {
            var student = new Student(1);
            var payment = new Payment(1, 500.0, DateTime.Now);

            student.AddPayment(payment);

            var ex = Assert.Throws<ArgumentException>(() => student.UpdatePayment(payment, null));
            Assert.That(ex.Message, Is.EqualTo("Both old and new payments must be provided."));
        }

        [Test]
        public void TestErrorWhenUpdatingNonExistingPaymentForStudent()
        {
            var student = new Student(1);
            var oldPayment = new Payment(1, 500.0, DateTime.Now);
            var newPayment = new Payment(2, 300.0, DateTime.Now);

            var ex = Assert.Throws<ArgumentException>(() => student.UpdatePayment(oldPayment, newPayment));
            Assert.That(ex.Message, Is.EqualTo("Payment is not added to this student."));
        }

        [Test]
        public void TestAddingPaymentToStudentReverseCheck()
        {
            var student = new Student(1);
            var payment = new Payment(1, 500.0, DateTime.Now);

            student.AddPayment(payment);

            Assert.Multiple(() =>
            {
                Assert.That(payment.Student, Is.EqualTo(student), "Reverse connection is not correctly established.");
                Assert.That(student.Payments.Contains(payment), "Payment is not associated with student.");
            });
        }

        [Test]
        public void TestRemovingStudentFromPaymentReverseCheck()
        {
            var student = new Student(1);
            var payment = new Payment(1, 500.0, DateTime.Now);

            student.AddPayment(payment);
            student.RemovePayment(payment);

            Assert.Multiple(() =>
            {
                Assert.That(payment.Student, Is.Null, "Payment still connected to student.");
                Assert.That(!student.Payments.Contains(payment), "Student still associated with payment.");
            });
        }
    }
}

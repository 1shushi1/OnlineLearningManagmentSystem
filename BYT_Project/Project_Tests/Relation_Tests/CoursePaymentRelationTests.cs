using BYT_Project;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Project_Tests.Relation_Tests
{
    [TestFixture]
    public class CoursePaymentRelationTests
    {
        [SetUp]
        public void Setup()
        {
            // Reset static fields to ensure clean tests
            typeof(Course)
                .GetField("coursesList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, new List<Course>());

            typeof(Payment)
                .GetField("paymentsList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, new List<Payment>());
        }

        [Test]
        public void TestAddingPaymentToCourse()
        {
            var course = new Course(1, "Mathematics", "Basic math course", 30);
            var payment = new Payment(1, 100.0, DateTime.Now);

            course.AddPayment(payment);

            Assert.Multiple(() =>
            {
                Assert.That(course.Payments.Contains(payment), "Payment not added to course.");
                Assert.That(payment.Course == course, "Course not set in payment.");
            });
        }

        [Test]
        public void TestRemovingPaymentFromCourse()
        {
            var course = new Course(1, "Mathematics", "Basic math course", 30);
            var payment = new Payment(1, 100.0, DateTime.Now);

            course.AddPayment(payment);
            course.RemovePayment(payment);

            Assert.Multiple(() =>
            {
                Assert.That(!course.Payments.Contains(payment), "Payment not removed from course.");
                Assert.That(payment.Course == null, "Course not removed from payment.");
            });
        }

        [Test]
        public void TestReverseConnectionIntegrity()
        {
            var course = new Course(1, "Mathematics", "Basic math course", 30);
            var payment = new Payment(1, 100.0, DateTime.Now);

            course.AddPayment(payment);
            course.RemovePayment(payment);

            Assert.That(payment.Course == null, "Reverse connection not properly removed.");
        }

        [Test]
        public void TestUpdatingPaymentInCourse()
        {
            var course = new Course(1, "Mathematics", "Basic math course", 30);
            var payment1 = new Payment(1, 100.0, DateTime.Now);
            var payment2 = new Payment(2, 200.0, DateTime.Now);

            course.AddPayment(payment1);
            course.UpdatePayment(payment1, payment2);

            Assert.Multiple(() =>
            {
                Assert.That(!course.Payments.Contains(payment1), "Old payment still associated with course.");
                Assert.That(course.Payments.Contains(payment2), "New payment not added to course.");
                Assert.That(payment2.Course == course, "Reverse connection not updated in payment.");
            });
        }

        [Test]
        public void TestEncapsulationOnPayments()
        {
            var course = new Course(1, "Mathematics", "Basic math course", 30);
            var payment = new Payment(1, 100.0, DateTime.Now);

            course.AddPayment(payment);
            var payments = course.Payments;

            Assert.Multiple(() =>
            {
                Assert.Throws<NotSupportedException>(() =>
                {
                    ((ICollection<Payment>)payments).Add(new Payment(2, 200.0, DateTime.Now));
                }, "Payments collection is modifiable using Add.");

                Assert.Throws<NotSupportedException>(() =>
                {
                    ((ICollection<Payment>)payments).Remove(payment);
                }, "Payments collection is modifiable using Remove.");
            });
        }

        [Test]
        public void TestErrorWhenAddingDuplicatePayment()
        {
            var course = new Course(1, "Mathematics", "Basic math course", 30);
            var payment = new Payment(1, 100.0, DateTime.Now);

            course.AddPayment(payment);

            var ex = Assert.Throws<ArgumentException>(() => course.AddPayment(payment));
            Assert.That(ex.Message, Is.EqualTo("Payment is already added to this course."));
        }
    }
}

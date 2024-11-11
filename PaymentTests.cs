using NUnit.Framework;
using System;
using BYT_Project;

namespace BYT_Project.Tests
{
    [TestFixture]
    public class PaymentTests
    {
        [Test]
        public void TestGetCorrectPaymentInformation()
        {
            var payment = new Payment(1, 100.50, DateTime.Now);
            Assert.That(payment.PaymentID, Is.EqualTo(1));
            Assert.That(payment.Amount, Is.EqualTo(100.50));
            Assert.That(payment.PaymentDate, Is.EqualTo(DateTime.Today).Within(TimeSpan.FromSeconds(1)));
        }

        [Test]
        public void TestExceptionForInvalidPaymentID()
        {
            var ex = Assert.Throws<ArgumentException>(() => new Payment(-1, 100.50, DateTime.Now));
            Assert.That(ex.Message, Is.EqualTo("Payment ID must be positive."));
        }

        [Test]
        public void TestExceptionForNegativeAmount()
        {
            var ex = Assert.Throws<ArgumentException>(() => new Payment(1, -100.50, DateTime.Now));
            Assert.That(ex.Message, Is.EqualTo("Amount must be positive."));
        }

        [Test]
        public void TestExceptionForFuturePaymentDate()
        {
            var ex = Assert.Throws<ArgumentException>(() => new Payment(1, 100.50, DateTime.Now.AddDays(1)));
            Assert.That(ex.Message, Is.EqualTo("Payment date cannot be in the future."));
        }

        [Test]
        public void TestSaveAndLoadPayments()
        {
            var payment1 = new Payment(1, 100.50, DateTime.Now);
            var payment2 = new Payment(2, 200.75, DateTime.Now.AddMonths(1));

            Payment.SavePayments();
            var success = Payment.LoadPayments();  // Load the payments

            Assert.That(success, Is.True);
            Assert.That(Payment.PaymentsList.Count, Is.EqualTo(2)); // Check that 2 payments are loaded
        }
    }
}

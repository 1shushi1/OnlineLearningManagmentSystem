using NUnit.Framework;
using System;
using BYT_Project;

namespace BYT_Project.Tests
{
    [TestFixture]
    public class CertificateTests
    {
        [Test]
        public void TestGetCorrectCertificateInformation()
        {
            var certificate = new Certificate(1, DateTime.Now, "Completed the course with distinction.");
            Assert.That(certificate.CertificateID, Is.EqualTo(1));
            Assert.That(certificate.CompletionDate, Is.EqualTo(DateTime.Today).Within(TimeSpan.FromSeconds(1)));
            Assert.That(certificate.CertificateDescription, Is.EqualTo("Completed the course with distinction."));
        }

        [Test]
        public void TestExceptionForInvalidCertificateID()
        {
            var ex = Assert.Throws<ArgumentException>(() => new Certificate(-1, DateTime.Now, "Description"));
            Assert.That(ex.Message, Is.EqualTo("Certificate ID must be positive."));
        }

        [Test]
        public void TestExceptionForEmptyCertificateDescription()
        {
            var ex = Assert.Throws<ArgumentException>(() => new Certificate(1, DateTime.Now, ""));
            Assert.That(ex.Message, Is.EqualTo("Certificate description cannot be empty."));
        }

        [Test]
        public void TestExceptionForFutureCompletionDate()
        {
            var ex = Assert.Throws<ArgumentException>(() => new Certificate(1, DateTime.Now.AddDays(1), "Description"));
            Assert.That(ex.Message, Is.EqualTo("Completion date cannot be in the future."));
        }

        [Test]
        public void TestSaveAndLoadCertificates()
        {
            var certificate1 = new Certificate(1, DateTime.Now, "Completed the course with distinction.");
            var certificate2 = new Certificate(2, DateTime.Now.AddMonths(1), "Completed the course.");

            Certificate.SaveCertificates();
            var success = Certificate.LoadCertificates();  // Load the certificates

            Assert.That(success, Is.True);
            Assert.That(Certificate.CertificatesList.Count, Is.EqualTo(2)); // Check that 2 certificates are loaded
        }
    }
}

using NUnit.Framework;
using System;
using BYT_Project;

namespace BYT_Project.Tests
{
    [TestFixture]
    public class CertificateTests
    {
        [SetUp]
        public void SetUp()
        {
            typeof(Certificate)
                 .GetField("certificatesList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                 ?.SetValue(null, new List<Certificate>());

            if (File.Exists("certificate.xml"))
            {
                File.Delete("certificate.xml");
            }
        }

        [Test]
        public void TestGetCorrectCertificateInformation()
        {
            var certificate = new Certificate(1, DateTime.Now.AddDays(-1), "Completion of C# Basics");

            Assert.That(certificate.CertificateID, Is.EqualTo(1));
            Assert.That(certificate.CompletionDate, Is.EqualTo(DateTime.Now.AddDays(-1)).Within(TimeSpan.FromMinutes(1)));
            Assert.That(certificate.CertificateDescription, Is.EqualTo("Completion of C# Basics"));
        }

        [Test]
        public void TestExceptionForInvalidCertificateID()
        {
            var ex = Assert.Throws<ArgumentException>(() => new Certificate(-1, DateTime.Now.AddDays(-1), "Completion of C# Basics"));
            Assert.That(ex.Message, Is.EqualTo("Certificate ID must be positive."));
        }

        [Test]
        public void TestExceptionForFutureCompletionDate()
        {
            var ex = Assert.Throws<ArgumentException>(() => new Certificate(1, DateTime.Now.AddDays(1), "Completion of C# Basics"));
            Assert.That(ex.Message, Is.EqualTo("Completion date cannot be in the future."));
        }

        [Test]
        public void TestSaveAndLoadCertificates()
        {
            var certificate = new Certificate(1, DateTime.Now.AddDays(-1), "Completion of C# Basics");

            Certificate.SaveCertificates("certificate.xml");

            typeof(Certificate)
                .GetField("certificatesList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, new List<Certificate>());

            var success = Certificate.LoadCertificates("certificate.xml");

            Assert.That(success, Is.True);
            Assert.That(Certificate.CertificatesList.Count, Is.EqualTo(1));
        }
    }
}

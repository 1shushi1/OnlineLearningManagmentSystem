using BYT_Project;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Project_Tests.Relation_Tests
{
    [TestFixture]
    public class EnrollmentCertificateRelationTests
    {
        [SetUp]
        public void Setup()
        {
            // Reset static fields to ensure clean tests
            typeof(Enrollment)
                .GetField("enrollmentsList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, new List<Enrollment>());

            typeof(Certificate)
                .GetField("certificatesList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, new List<Certificate>());
        }

        [Test]
        public void TestAddingCertificateToEnrollment()
        {
            var enrollment = new Enrollment(1, DateTime.Now, "Active", 85);
            var certificate = new Certificate(1, DateTime.Now, "Completion Certificate");

            enrollment.SetCertificate(certificate);

            Assert.Multiple(() =>
            {
                Assert.That(enrollment.Certificate, Is.EqualTo(certificate), "Certificate not set in enrollment.");
                Assert.That(certificate.Enrollments.Contains(enrollment), "Enrollment not added to certificate.");
            });
        }

        [Test]
        public void TestRemovingCertificateFromEnrollment()
        {
            var enrollment = new Enrollment(1, DateTime.Now, "Active", 85);
            var certificate = new Certificate(1, DateTime.Now, "Completion Certificate");

            enrollment.SetCertificate(certificate);
            enrollment.RemoveCertificate();

            Assert.Multiple(() =>
            {
                Assert.That(enrollment.Certificate, Is.Null, "Certificate not removed from enrollment.");
                Assert.That(!certificate.Enrollments.Contains(enrollment), "Enrollment not removed from certificate.");
            });
        }

        [Test]
        public void TestReverseConnectionIntegrity()
        {
            var enrollment = new Enrollment(1, DateTime.Now, "Active", 85);
            var certificate = new Certificate(1, DateTime.Now, "Completion Certificate");

            enrollment.SetCertificate(certificate);
            enrollment.RemoveCertificate();

            Assert.That(!certificate.Enrollments.Contains(enrollment), "Reverse connection not properly removed.");
        }

        [Test]
        public void TestErrorWhenAddingDuplicateEnrollmentToCertificate()
        {
            var certificate = new Certificate(1, DateTime.Now, "Completion Certificate");
            var enrollment = new Enrollment(1, DateTime.Now, "Active", 95);

            certificate.AddEnrollment(enrollment);

            var ex = Assert.Throws<ArgumentException>(() => certificate.AddEnrollment(enrollment));
            Assert.That(ex.Message, Is.EqualTo("Enrollment is already associated with this certificate."));
        }

        [Test]
        public void TestErrorWhenRemovingNonExistingEnrollmentFromCertificate()
        {
            var certificate = new Certificate(1, DateTime.Now, "Completion Certificate");
            var enrollment = new Enrollment(1, DateTime.Now, "Active", 85);

            var ex = Assert.Throws<ArgumentException>(() => certificate.RemoveEnrollment(enrollment));
            Assert.That(ex.Message, Is.EqualTo("Enrollment is not associated with this certificate."));
        }



        [Test]
        public void TestUpdatingCertificateForEnrollment()
        {
            var certificate1 = new Certificate(1, DateTime.Now, "Completion Certificate 1");
            var certificate2 = new Certificate(2, DateTime.Now, "Completion Certificate 2");
            var enrollment = new Enrollment(1, DateTime.Now, "Active", 95);

            certificate1.AddEnrollment(enrollment); // Initial association

            // Update the certificate for the enrollment
            enrollment.SetCertificate(certificate2);

            Assert.Multiple(() =>
            {
                Assert.That(!certificate1.Enrollments.Contains(enrollment), "Enrollment still associated with the old certificate.");
                Assert.That(certificate2.Enrollments.Contains(enrollment), "Enrollment not associated with the new certificate.");
                Assert.That(enrollment.Certificate, Is.EqualTo(certificate2), "Certificate reference in enrollment not updated correctly.");
            });
        }





        [Test]
        public void TestEncapsulationOnCertificateEnrollments()
        {
            var certificate = new Certificate(1, DateTime.Now, "Completion Certificate");
            var enrollment = new Enrollment(1, DateTime.Now, "Active", 95);

            certificate.AddEnrollment(enrollment);

            var enrollments = certificate.Enrollments;

            Assert.Multiple(() =>
            {
                Assert.Throws<NotSupportedException>(() =>
                {
                    ((ICollection<Enrollment>)enrollments).Add(new Enrollment(2, DateTime.Now, "Active", 80));
                }, "Enrollments collection is modifiable using Add.");

                Assert.Throws<NotSupportedException>(() =>
                {
                    ((ICollection<Enrollment>)enrollments).Remove(enrollment);
                }, "Enrollments collection is modifiable using Remove.");

                Assert.That(enrollments.Contains(enrollment), "Enrollments collection does not return correct values.");
            });
        }


        [Test]
        public void TestAssignEnrollmentDirectlyToCertificate()
        {
            var certificate = new Certificate(1, DateTime.Now, "Completion Certificate");
            var enrollment = new Enrollment(1, DateTime.Now, "Active", 85);

            certificate.AddEnrollment(enrollment);

            Assert.Multiple(() =>
            {
                Assert.That(enrollment.Certificate, Is.EqualTo(certificate), "Enrollment not assigned to certificate via direct addition.");
                Assert.That(certificate.Enrollments.Contains(enrollment), "Enrollment not added to certificate via direct addition.");
            });
        }

        [Test]
        public void TestErrorWhenEnrollmentAlreadyAssignedToAnotherCertificate()
        {
            var certificate1 = new Certificate(1, DateTime.Now, "Completion Certificate 1");
            var certificate2 = new Certificate(2, DateTime.Now, "Completion Certificate 2");
            var enrollment = new Enrollment(1, DateTime.Now, "Active", 95);

            certificate1.AddEnrollment(enrollment);

            var ex = Assert.Throws<ArgumentException>(() => certificate2.AddEnrollment(enrollment));
            Assert.That(ex.Message, Is.EqualTo("Enrollment is already associated with another certificate."));
        }

    }
}

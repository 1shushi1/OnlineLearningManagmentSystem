using NUnit.Framework;
using System;
using System.Collections.Generic;
using BYT_Project;

namespace Project_Tests.Relation_Tests
{
    [TestFixture]
    public class EnrollmentCertificateRelationTests
    {
        private Student _student;
        private Course _course;

        [SetUp]
        public void Setup()
        {
            typeof(Enrollment)
                .GetField("enrollmentsList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, new List<Enrollment>());

            typeof(Certificate)
                .GetField("certificatesList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, new List<Certificate>());

            _student = new Student(1);
            _course = new Course(101, "Math 101", "Basic Math", 30);
        }

        [Test]
        public void TestAddingCertificateToEnrollment()
        {
            var enrollment = new Enrollment(_student, _course, 1, DateTime.Now, "Active", 85);
            var certificate = new Certificate(1, DateTime.Now, "Completion Certificate");

            enrollment.SetCertificate(certificate);

            Assert.That(enrollment.Certificate, Is.EqualTo(certificate));
            Assert.That(certificate.Enrollments.Contains(enrollment));
        }

        [Test]
        public void TestRemovingCertificateFromEnrollment()
        {
            var enrollment = new Enrollment(_student, _course, 1, DateTime.Now, "Active", 85);
            var certificate = new Certificate(1, DateTime.Now, "Completion Certificate");

            enrollment.SetCertificate(certificate);
            enrollment.RemoveCertificate();

            Assert.That(enrollment.Certificate, Is.Null);
            Assert.That(!certificate.Enrollments.Contains(enrollment));
        }

        [Test]
        public void TestReverseConnectionIntegrity()
        {
            var enrollment = new Enrollment(_student, _course, 1, DateTime.Now, "Active", 85);
            var certificate = new Certificate(1, DateTime.Now, "Completion Certificate");

            enrollment.SetCertificate(certificate);
            enrollment.RemoveCertificate();

            Assert.That(!certificate.Enrollments.Contains(enrollment));
        }

        [Test]
        public void TestErrorWhenAddingDuplicateEnrollmentToCertificate()
        {
            var certificate = new Certificate(1, DateTime.Now, "Completion Certificate");
            var enrollment = new Enrollment(_student, _course, 1, DateTime.Now, "Active", 95);

            certificate.AddEnrollment(enrollment);

            var ex = Assert.Throws<ArgumentException>(() => certificate.AddEnrollment(enrollment));
            Assert.That(ex.Message, Is.EqualTo("Enrollment is already associated with this certificate."));
        }

        [Test]
        public void TestErrorWhenRemovingNonExistingEnrollmentFromCertificate()
        {
            var certificate = new Certificate(1, DateTime.Now, "Completion Certificate");
            var enrollment = new Enrollment(_student, _course, 1, DateTime.Now, "Active", 85);

            var ex = Assert.Throws<ArgumentException>(() => certificate.RemoveEnrollment(enrollment));
            Assert.That(ex.Message, Is.EqualTo("Enrollment is not associated with this certificate."));
        }

        [Test]
        public void TestUpdatingCertificateForEnrollment()
        {
            var certificate1 = new Certificate(1, DateTime.Now, "Completion Certificate 1");
            var certificate2 = new Certificate(2, DateTime.Now, "Completion Certificate 2");
            var enrollment = new Enrollment(_student, _course, 1, DateTime.Now, "Active", 95);

            certificate1.AddEnrollment(enrollment);
            enrollment.SetCertificate(certificate2);

            Assert.That(!certificate1.Enrollments.Contains(enrollment));
            Assert.That(certificate2.Enrollments.Contains(enrollment));
            Assert.That(enrollment.Certificate, Is.EqualTo(certificate2));
        }

        [Test]
        public void TestEncapsulationOnCertificateEnrollments()
        {
            var certificate = new Certificate(1, DateTime.Now, "Completion Certificate");
            var enrollment = new Enrollment(_student, _course, 1, DateTime.Now, "Active", 95);

            certificate.AddEnrollment(enrollment);

            var enrollments = certificate.Enrollments;

            Assert.Throws<NotSupportedException>(() =>
            {
                ((ICollection<Enrollment>)enrollments).Add(new Enrollment(_student, _course, 1, DateTime.Now, "Active", 80));
            });

            Assert.Throws<NotSupportedException>(() =>
            {
                ((ICollection<Enrollment>)enrollments).Remove(enrollment);
            });

            Assert.That(enrollments.Contains(enrollment));
        }

        [Test]
        public void TestAssignEnrollmentDirectlyToCertificate()
        {
            var certificate = new Certificate(1, DateTime.Now, "Completion Certificate");
            var enrollment = new Enrollment(_student, _course, 1, DateTime.Now, "Active", 85);

            certificate.AddEnrollment(enrollment);

            Assert.That(enrollment.Certificate, Is.EqualTo(certificate));
            Assert.That(certificate.Enrollments.Contains(enrollment));
        }

        [Test]
        public void TestErrorWhenEnrollmentAlreadyAssignedToAnotherCertificate()
        {
            var certificate1 = new Certificate(1, DateTime.Now, "Completion Certificate 1");
            var certificate2 = new Certificate(2, DateTime.Now, "Completion Certificate 2");
            var enrollment = new Enrollment(_student, _course, 1, DateTime.Now, "Active", 95);

            certificate1.AddEnrollment(enrollment);

            var ex = Assert.Throws<ArgumentException>(() => certificate2.AddEnrollment(enrollment));
            Assert.That(ex.Message, Is.EqualTo("Enrollment is already associated with another certificate."));
        }
    }
}

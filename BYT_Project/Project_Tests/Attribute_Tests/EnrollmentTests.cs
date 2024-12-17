﻿using NUnit.Framework;
using System;
using BYT_Project;

namespace BYT_Project.Tests
{
    [TestFixture]
    public class EnrollmentTests
    {
        [SetUp]
        public void SetUp()
        {
            typeof(Enrollment)
                 .GetField("enrollmentsList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                 ?.SetValue(null, new List<Enrollment>());

            if (File.Exists("enrollment.xml"))
            {
                File.Delete("enrollment.xml");
            }
        }

        [Test]
        public void TestGetCorrectEnrollmentInformation()
        {
            var enrollment = new Enrollment(1, DateTime.Now, "Active", 85);

            Assert.That(enrollment.EnrollmentID, Is.EqualTo(1));
            Assert.That(enrollment.Status, Is.EqualTo("Active"));
            Assert.That(enrollment.TotalScore, Is.EqualTo(85));
            Assert.That(enrollment.GradeLetter, Is.EqualTo("B"));
        }

        [Test]
        public void TestExceptionForInvalidEnrollmentID()
        {
            var ex = Assert.Throws<ArgumentException>(() => new Enrollment(-1, DateTime.Now, "Active", 85));
            Assert.That(ex.Message, Is.EqualTo("Enrollment ID must be positive."));
        }

        [Test]
        public void TestExceptionForInvalidTotalScore()
        {
            var ex = Assert.Throws<ArgumentException>(() => new Enrollment(1, DateTime.Now, "Active", 150));
            Assert.That(ex.Message, Is.EqualTo("Total score must be between 0 and 100."));
        }

        [Test]
        public void TestSaveAndLoadEnrollments()
        {
            var enrollment = new Enrollment(1, DateTime.Now, "Active", 85);
            
            Assert.That(Enrollment.EnrollmentsList.Count, Is.EqualTo(1));

            Enrollment.SaveEnrollments("enrollment.xml");

            typeof(Enrollment)
                 .GetField("enrollmentsList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                 ?.SetValue(null, new List<Enrollment>());
            
            var success = Enrollment.LoadEnrollments("enrollment.xml");

            Assert.That(success, Is.True);

            Assert.That(Enrollment.EnrollmentsList.Count, Is.EqualTo(1)); 
        }

    }
}
﻿using NUnit.Framework;
using System;
using BYT_Project;

namespace BYT_Project.Tests
{
    [TestFixture]
    public class SubmittedAssignmentTests
    {
        [Test]
        public void TestGetCorrectSubmissionInformation()
        {
            var submittedAssignment = new SubmittedAssignment(1, DateTime.Now);

            Assert.That(submittedAssignment.SubmissionID, Is.EqualTo(1));
            Assert.That(submittedAssignment.SubmissionDate, Is.EqualTo(DateTime.Today).Within(TimeSpan.FromSeconds(1)));
        }

        [Test]
        public void TestExceptionForInvalidSubmissionID()
        {
            var ex = Assert.Throws<ArgumentException>(() => new SubmittedAssignment(-1, DateTime.Now));
            Assert.That(ex.Message, Is.EqualTo("Submission ID must be positive."));
        }

        [Test]
        public void TestExceptionForFutureSubmissionDate()
        {
            var ex = Assert.Throws<ArgumentException>(() => new SubmittedAssignment(1, DateTime.Now.AddDays(1)));
            Assert.That(ex.Message, Is.EqualTo("Submission date cannot be in the future."));
        }

        [Test]
        public void TestSaveAndLoadSubmissions()
        {
            var submittedAssignment = new SubmittedAssignment(1, DateTime.Now);

            SubmittedAssignment.SaveSubmissions();
            var success = SubmittedAssignment.LoadSubmissions();

            Assert.That(success, Is.True);
            Assert.That(SubmittedAssignment.SubmissionsList.Count, Is.EqualTo(1)); // Check if submission is saved
        }
    }
}
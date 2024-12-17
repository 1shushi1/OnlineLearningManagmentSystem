using NUnit.Framework;
using System;
using BYT_Project;

namespace BYT_Project.Tests
{
    [TestFixture]
    public class SubmittedAssignmentTests
    {
       
        [SetUp]
        public void SetUp()
        {
            typeof(SubmittedAssignment)
               .GetField("submissionsList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
               ?.SetValue(null, new List<SubmittedAssignment>());

            if (File.Exists("submission.xml"))
            {
                File.Delete("submission.xml");
            }
        }

        [Test]
        public void TestGetCorrectSubmissionInformation()
        {
            var submittedAssignment = new SubmittedAssignment(1, DateTime.Now);

            Assert.That(submittedAssignment.SubmissionID, Is.EqualTo(1));
            Assert.That(submittedAssignment.SubmissionDate.Date, Is.EqualTo(DateTime.Today));
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

            Assert.That(SubmittedAssignment.SubmissionsList.Count, Is.EqualTo(1));

            SubmittedAssignment.SaveSubmissions("submissions.xml");

            typeof(SubmittedAssignment)
               .GetField("submissionsList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
               ?.SetValue(null, new List<SubmittedAssignment>());

            var success = SubmittedAssignment.LoadSubmissions("submissions.xml");

            Assert.That(success, Is.True);  
            Assert.That(SubmittedAssignment.SubmissionsList.Count, Is.EqualTo(1)); 
        }
    }
}

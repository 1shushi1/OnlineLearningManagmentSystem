using NUnit.Framework;
using System;
using BYT_Project;

namespace BYT_Project.Tests
{
    [TestFixture]
    public class QuizTests
    {
        [SetUp]
        public void SetUp()
        {
            typeof(Quiz)
                 .GetField("quizzesList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                 ?.SetValue(null, new List<Quiz>());

            if (File.Exists("quiz.xml"))
            {
                File.Delete("quiz.xml");
            }
        }

        [Test]
        public void TestGetCorrectQuizInformation()
        {
            var quiz = new Quiz(1, "Math Quiz", 100, 60);

            Assert.That(quiz.QuizID, Is.EqualTo(1));
            Assert.That(quiz.Title, Is.EqualTo("Math Quiz"));
            Assert.That(quiz.TotalScore, Is.EqualTo(100));
            Assert.That(quiz.PassMark, Is.EqualTo(60));
        }

        [Test]
        public void TestExceptionForInvalidQuizID()
        {
            var ex = Assert.Throws<ArgumentException>(() => new Quiz(-1, "Math Quiz", 100, 60));
            Assert.That(ex.Message, Is.EqualTo("Quiz ID must be positive."));
        }

        [Test]
        public void TestExceptionForInvalidPassMark()
        {
            var ex = Assert.Throws<ArgumentException>(() => new Quiz(1, "Math Quiz", 100, 120));
            Assert.That(ex.Message, Is.EqualTo("Pass mark must be within the range of the total score."));
        }

        [Test]
        public void TestSaveAndLoadQuizzes()
        {
            var quiz = new Quiz(1, "Math Quiz", 100, 60);

            Assert.That(Quiz.QuizzesList.Count, Is.EqualTo(1));

            Quiz.SaveQuizzes("quiz.xml");

            typeof(Quiz)
                 .GetField("quizzesList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                 ?.SetValue(null, new List<Quiz>());

            var success = Quiz.LoadQuizzes("quiz.xml");

            Assert.That(success, Is.True);
            Assert.That(Quiz.QuizzesList.Count, Is.EqualTo(1));
        }
    }
}

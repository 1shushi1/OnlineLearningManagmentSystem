using NUnit.Framework;
using System;
using System.Collections.Generic;
using BYT_Project;

namespace BYT_Project.Tests
{
    [TestFixture]
    public class QuestionTests
    {
        [Test]
        public void TestGetCorrectQuestionInformation()
        {
            var options = new List<string> { "A", "B", "C", "D" };
            var question = new Question(1, "What is 2 + 2?", options, "B", "MultipleChoice");

            Assert.That(question.QuestionID, Is.EqualTo(1));
            Assert.That(question.Text, Is.EqualTo("What is 2 + 2?"));
            Assert.That(question.Options, Is.EqualTo(options));
            Assert.That(question.CorrectAnswer, Is.EqualTo("B"));
            Assert.That(question.QuestionType, Is.EqualTo("MultipleChoice"));
        }

        [Test]
        public void TestExceptionForInvalidQuestionID()
        {
            var ex = Assert.Throws<ArgumentException>(() => new Question(-1, "What is 2 + 2?", new List<string> { "A", "B", "C", "D" }, "B", "MultipleChoice"));
            Assert.That(ex.Message, Is.EqualTo("Question ID must be positive."));
        }

        [Test]
        public void TestExceptionForInvalidOptionsCount()
        {
            var ex = Assert.Throws<ArgumentException>(() => new Question(1, "What is 2 + 2?", new List<string> { "A", "B" }, "B", "MultipleChoice"));
            Assert.That(ex.Message, Is.EqualTo("There must be exactly 4 options."));
        }

        [Test]
        public void TestExceptionForInvalidCorrectAnswer()
        {
            var ex = Assert.Throws<ArgumentException>(() => new Question(1, "What is 2 + 2?", new List<string> { "A", "B", "C", "D" }, "X", "MultipleChoice"));
            Assert.That(ex.Message, Is.EqualTo("Correct answer must be one of the options."));
        }

        [Test]
        public void TestSaveAndLoadQuestions()
        {
            var options = new List<string> { "A", "B", "C", "D" };
            var question = new Question(1, "What is 2 + 2?", options, "B", "MultipleChoice");

            Question.SaveQuestions();
            var success = Question.LoadQuestions();

            Assert.That(success, Is.True);
            Assert.That(Question.QuestionsList.Count, Is.EqualTo(1)); // Ensure question is saved and loaded
        }
    }
}

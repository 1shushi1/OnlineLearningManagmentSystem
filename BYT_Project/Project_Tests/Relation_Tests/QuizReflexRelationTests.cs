using BYT_Project;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Project_Tests.Relation_Tests
{
    [TestFixture]
    public class QuizQuizRelationTests
    {
        [SetUp]
        public void Setup()
        {
            typeof(Quiz)
                .GetField("quizzesList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, new List<Quiz>());
        }

        [Test]
        public void TestAddingRelatedQuiz()
        {
            var quiz1 = new Quiz(1, "Quiz 1", 100, 60);
            var quiz2 = new Quiz(2, "Quiz 2", 80, 50);

            quiz1.AddRelatedQuiz(quiz2);

            Assert.That(quiz1.RelatedQuizzes.Contains(quiz2), "Quiz 2 not added as a related quiz to Quiz 1.");
            Assert.That(quiz2.RelatedQuizzes.Contains(quiz1), "Quiz 1 not added as a related quiz to Quiz 2.");
        }

        [Test]
        public void TestRemovingRelatedQuiz()
        {
            var quiz1 = new Quiz(1, "Quiz 1", 100, 60);
            var quiz2 = new Quiz(2, "Quiz 2", 80, 50);

            quiz1.AddRelatedQuiz(quiz2);
            quiz1.RemoveRelatedQuiz(quiz2);

            Assert.That(!quiz1.RelatedQuizzes.Contains(quiz2), "Quiz 2 not removed from Quiz 1's related quizzes.");
            Assert.That(!quiz2.RelatedQuizzes.Contains(quiz1), "Quiz 1 not removed from Quiz 2's related quizzes.");
        }

        [Test]
        public void TestReverseConnectionIntegrity()
        {
            var quiz1 = new Quiz(1, "Quiz 1", 100, 60);
            var quiz2 = new Quiz(2, "Quiz 2", 80, 50);

            quiz1.AddRelatedQuiz(quiz2);
            quiz1.RemoveRelatedQuiz(quiz2);

            Assert.That(!quiz1.RelatedQuizzes.Contains(quiz2), "Quiz 1 still has a reference to Quiz 2.");
            Assert.That(!quiz2.RelatedQuizzes.Contains(quiz1), "Quiz 2 still has a reference to Quiz 1.");
        }

        [Test]
        public void TestErrorHandlingForAddingDuplicateRelatedQuiz()
        {
            var quiz1 = new Quiz(1, "Quiz 1", 100, 60);
            var quiz2 = new Quiz(2, "Quiz 2", 80, 50);

            quiz1.AddRelatedQuiz(quiz2);

            var ex = Assert.Throws<ArgumentException>(() => quiz1.AddRelatedQuiz(quiz2));
            Assert.That(ex.Message, Is.EqualTo("This quiz is already related."));
        }

        [Test]
        public void TestErrorHandlingForAddingRelatedQuizToItself()
        {
            var quiz = new Quiz(1, "Quiz 1", 100, 60);

            var ex = Assert.Throws<ArgumentException>(() => quiz.AddRelatedQuiz(quiz));
            Assert.That(ex.Message, Is.EqualTo("A quiz cannot be related to itself."));
        }

        [Test]
        public void TestErrorHandlingForRemovingNonExistingRelatedQuiz()
        {
            var quiz1 = new Quiz(1, "Quiz 1", 100, 60);
            var quiz2 = new Quiz(2, "Quiz 2", 80, 50);

            var ex = Assert.Throws<ArgumentException>(() => quiz1.RemoveRelatedQuiz(quiz2));
            Assert.That(ex.Message, Is.EqualTo("This quiz is not related."));
        }
    }
}

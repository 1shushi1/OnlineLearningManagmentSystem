using BYT_Project;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Project_Tests.Relation_Tests
{
    [TestFixture]
    public class QuizQuestionRelationTests
    {
        [SetUp]
        public void Setup()
        {
            typeof(Quiz)
                .GetField("quizzesList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, new List<Quiz>());

            typeof(Question)
                .GetField("questionList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, new List<Question>());
        }

        [Test]
        public void TestAddingQuestionToQuiz()
        {
            var quiz = new Quiz(1, "Quiz 1", 100, 60);
            var question = new Question(1, "What is 2 + 2?", new List<string> { "1", "2", "3", "4" }, "4", "MCQ");

            quiz.AddQuestion("Math", question);

            Assert.Multiple(() =>
            {
                Assert.That(quiz.Questions.ContainsKey("Math"), "Question not added to quiz.");
                Assert.That(quiz.Questions["Math"], Is.EqualTo(question), "Incorrect question added.");
                Assert.That(question.Quiz, Is.EqualTo(quiz), "Quiz not set in question.");
                Assert.That(question.Qualifier, Is.EqualTo("Math"), "Qualifier not set in question.");
            });
        }

        [Test]
        public void TestRemovingQuestionFromQuiz()
        {
            var quiz = new Quiz(1, "Quiz 1", 100, 60);
            var question = new Question(1, "What is 2 + 2?", new List<string> { "1", "2", "3", "4" }, "4", "MCQ");

            quiz.AddQuestion("Math", question);
            quiz.RemoveQuestion("Math");

            Assert.Multiple(() =>
            {
                Assert.That(!quiz.Questions.ContainsKey("Math"), "Question not removed from quiz.");
                Assert.That(question.Quiz, Is.Null, "Quiz reference not removed from question.");
                Assert.That(question.Qualifier, Is.Null, "Qualifier not removed from question.");
            });
        }

        [Test]
        public void TestReverseConnectionIntegrity()
        {
            var quiz = new Quiz(1, "Quiz 1", 100, 60);
            var question = new Question(1, "What is 2 + 2?", new List<string> { "1", "2", "3", "4" }, "4", "MCQ");

            quiz.AddQuestion("Math", question);
            quiz.RemoveQuestion("Math");

            Assert.Multiple(() =>
            {
                Assert.That(!quiz.Questions.ContainsKey("Math"), "Quiz still has a reference to the question.");
                Assert.That(question.Quiz, Is.Null, "Question still has a reference to the quiz.");
            });
        }

        [Test]
        public void TestErrorHandlingForAddingDuplicateQualifier()
        {
            var quiz = new Quiz(1, "Quiz 1", 100, 60);
            var question1 = new Question(1, "What is 2 + 2?", new List<string> { "1", "2", "3", "4" }, "4", "MCQ");
            var question2 = new Question(2, "What is 3 + 3?", new List<string> { "5", "6", "7", "8" }, "6", "MCQ");

            quiz.AddQuestion("Math", question1);

            var ex = Assert.Throws<ArgumentException>(() => quiz.AddQuestion("Math", question2));
            Assert.That(ex.Message, Is.EqualTo("A question with this qualifier already exists."));
        }

        [Test]
        public void TestErrorHandlingForAddingQuestionAlreadyInAnotherQuiz()
        {
            var quiz1 = new Quiz(1, "Quiz 1", 100, 60);
            var quiz2 = new Quiz(2, "Quiz 2", 80, 50);
            var question = new Question(1, "What is 2 + 2?", new List<string> { "1", "2", "3", "4" }, "4", "MCQ");

            quiz1.AddQuestion("Math", question);

            var ex = Assert.Throws<ArgumentException>(() => quiz2.AddQuestion("Science", question));
            Assert.That(ex.Message, Is.EqualTo("Question is already associated with another quiz."));
        }

        [Test]
        public void TestErrorHandlingForRemovingNonExistingQualifier()
        {
            var quiz = new Quiz(1, "Quiz 1", 100, 60);

            var ex = Assert.Throws<ArgumentException>(() => quiz.RemoveQuestion("Math"));
            Assert.That(ex.Message, Is.EqualTo("No question exists for the given qualifier."));
        }

        [Test]
        public void TestErrorHandlingForAddingNullQuestion()
        {
            var quiz = new Quiz(1, "Quiz 1", 100, 60);

            var ex = Assert.Throws<ArgumentException>(() => quiz.AddQuestion("Math", null));
            Assert.That(ex.Message, Is.EqualTo("Question cannot be null."));
        }

        [Test]
        public void TestErrorHandlingForAddingWithNullQualifier()
        {
            var quiz = new Quiz(1, "Quiz 1", 100, 60);
            var question = new Question(1, "What is 2 + 2?", new List<string> { "1", "2", "3", "4" }, "4", "MCQ");

            var ex = Assert.Throws<ArgumentException>(() => quiz.AddQuestion(null, question));
            Assert.That(ex.Message, Is.EqualTo("Qualifier cannot be empty."));
        }

        [Test]
        public void TestErrorHandlingForRemovingFromQuestionWithoutQuiz()
        {
            var question = new Question(1, "What is 2 + 2?", new List<string> { "1", "2", "3", "4" }, "4", "MCQ");

            var ex = Assert.Throws<ArgumentException>(() => question.RemoveQuiz());
            Assert.That(ex.Message, Is.EqualTo("Question is not associated with any quiz."));
        }

        [Test]
        public void TestQualifiedAssociation()
        {
            var quiz = new Quiz(1, "Quiz 1", 100, 60);
            var question1 = new Question(1, "What is 2 + 2?", new List<string> { "1", "2", "3", "4" }, "4", "MCQ");
            var question2 = new Question(2, "What is 3 + 3?", new List<string> { "5", "6", "7", "8" }, "6", "MCQ");

            quiz.AddQuestion("Math", question1);
            quiz.AddQuestion("Science", question2);

            Assert.Multiple(() =>
            {
                Assert.That(quiz.Questions.ContainsKey("Math"), "Math qualifier not found in quiz.");
                Assert.That(quiz.Questions.ContainsKey("Science"), "Science qualifier not found in quiz.");
                Assert.That(quiz.Questions["Math"], Is.EqualTo(question1), "Incorrect question for Math qualifier.");
                Assert.That(quiz.Questions["Science"], Is.EqualTo(question2), "Incorrect question for Science qualifier.");
            });
        }

        [Test]
        public void TestAggregationBehavior()
        {
            var quiz = new Quiz(1, "Quiz 1", 100, 60);
            var question = new Question(1, "What is 2 + 2?", new List<string> { "1", "2", "3", "4" }, "4", "MCQ");

            quiz.AddQuestion("Math", question);

            Quiz.QuizzesList.Remove(quiz);

            Assert.That(question.Quiz, Is.EqualTo(quiz), "Question's quiz reference should still exist after quiz deletion.");
        }
    }
}

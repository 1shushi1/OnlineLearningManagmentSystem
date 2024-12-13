using BYT_Project;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Project_Tests.Relation_Tests
{
    [TestFixture]
    public class LessonQuizRelationTests
    {
        [SetUp]
        public void Setup()
        {
            typeof(Lesson)
                .GetField("lessonsList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, new List<Lesson>());

            typeof(Quiz)
                .GetField("quizzesList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.SetValue(null, new List<Quiz>());
        }

        [Test]
        public void TestAddingQuizToLesson()
        {
            var lesson = new Lesson(1, "Math Lesson", "http://example.com", "Basic math lesson");
            var quiz = new Quiz(1, "Math Quiz", 100, 60);

            lesson.AddQuiz(quiz);

            Assert.Multiple(() =>
            {
                Assert.That(lesson.Quizzes.Contains(quiz), "Quiz not added to lesson.");
                Assert.That(quiz.Lesson == lesson, "Lesson not set in quiz.");
            });
        }

        [Test]
        public void TestRemovingQuizFromLesson()
        {
            var lesson = new Lesson(1, "Math Lesson", "http://example.com", "Basic math lesson");
            var quiz = new Quiz(1, "Math Quiz", 100, 60);

            lesson.AddQuiz(quiz);
            lesson.RemoveQuiz(quiz);

            Assert.Multiple(() =>
            {
                Assert.That(!lesson.Quizzes.Contains(quiz), "Quiz not removed from lesson.");
                Assert.That(quiz.Lesson == null, "Lesson not removed from quiz.");
            });
        }

        [Test]
        public void TestReverseConnectionIntegrity()
        {
            var lesson = new Lesson(1, "Math Lesson", "http://example.com", "Basic math lesson");
            var quiz = new Quiz(1, "Math Quiz", 100, 60);

            lesson.AddQuiz(quiz);
            lesson.RemoveQuiz(quiz);

            Assert.That(quiz.Lesson == null, "Reverse connection not properly removed.");
        }

        [Test]
        public void TestErrorWhenAddingDuplicateQuizToLesson()
        {
            var lesson = new Lesson(1, "Math Lesson", "http://example.com", "Basic math lesson");
            var quiz = new Quiz(1, "Math Quiz", 100, 60);

            lesson.AddQuiz(quiz);

            var ex = Assert.Throws<ArgumentException>(() => lesson.AddQuiz(quiz));
            Assert.That(ex.Message, Is.EqualTo("Quiz is already added to this lesson."));
        }

        [Test]
        public void TestErrorWhenRemovingNonExistingQuizFromLesson()
        {
            var lesson = new Lesson(1, "Math Lesson", "http://example.com", "Basic math lesson");
            var quiz = new Quiz(1, "Math Quiz", 100, 60);

            var ex = Assert.Throws<ArgumentException>(() => lesson.RemoveQuiz(quiz));
            Assert.That(ex.Message, Is.EqualTo("Quiz is not associated with this lesson."));
        }

        [Test]
        public void TestDeletingLessonAlsoDeletesQuizzes()
        {
            var lesson = new Lesson(1, "Math Lesson", "http://example.com", "Basic math lesson");
            var quiz1 = new Quiz(1, "Math Quiz 1", 100, 60);
            var quiz2 = new Quiz(2, "Math Quiz 2", 150, 75);

            lesson.AddQuiz(quiz1);
            lesson.AddQuiz(quiz2);

            Lesson.DeleteLesson(lesson);

            Assert.Multiple(() =>
            {
                Assert.That(quiz1.Lesson, Is.Null, "Quiz 1 not dissociated from deleted lesson.");
                Assert.That(quiz2.Lesson, Is.Null, "Quiz 2 not dissociated from deleted lesson.");
                Assert.That(lesson.Quizzes.Count, Is.EqualTo(0), "Quizzes not deleted when lesson is deleted.");
            });
        }

        [Test]
        public void TestUpdatingQuizLesson()
        {
            var lesson1 = new Lesson(1, "Math Lesson 1", "http://example.com/1", "Basic math lesson 1");
            var lesson2 = new Lesson(2, "Math Lesson 2", "http://example.com/2", "Basic math lesson 2");
            var quiz = new Quiz(1, "Math Quiz", 100, 60);

            lesson1.AddQuiz(quiz);

            quiz.AssignToLesson(lesson2);

            Assert.Multiple(() =>
            {
                Assert.That(!lesson1.Quizzes.Contains(quiz), "Quiz still associated with the old lesson.");
                Assert.That(lesson2.Quizzes.Contains(quiz), "Quiz not added to the new lesson.");
                Assert.That(quiz.Lesson, Is.EqualTo(lesson2), "Quiz lesson reference not updated correctly.");
            });
        }

        [Test]
        public void TestEncapsulationOnQuizzes()
        {
            var lesson = new Lesson(1, "Math Lesson", "http://example.com", "Basic math lesson");
            var quiz = new Quiz(1, "Math Quiz", 100, 60);

            lesson.AddQuiz(quiz);

            var quizzes = lesson.Quizzes;

            Assert.Multiple(() =>
            {
                Assert.Throws<NotSupportedException>(() =>
                {
                    ((ICollection<Quiz>)quizzes).Add(new Quiz(2, "New Quiz", 50, 30));
                }, "Quizzes collection is modifiable using Add.");

                Assert.Throws<NotSupportedException>(() =>
                {
                    ((ICollection<Quiz>)quizzes).Remove(quiz);
                }, "Quizzes collection is modifiable using Remove.");

                Assert.That(quizzes.Contains(quiz), "Quizzes collection is not returning the correct values.");
            });
        }



        [Test]
        public void TestAssignQuizDirectlyToLesson()
        {
            var lesson = new Lesson(1, "Math Lesson", "http://example.com", "Basic math lesson");
            var quiz = new Quiz(1, "Math Quiz", 100, 60);

            quiz.AssignToLesson(lesson);

            Assert.Multiple(() =>
            {
                Assert.That(lesson.Quizzes.Contains(quiz), "Quiz not added to lesson via direct assignment.");
                Assert.That(quiz.Lesson == lesson, "Lesson not set correctly via direct assignment.");
            });
        }

        [Test]
        public void TestErrorWhenQuizAlreadyAssignedToAnotherLesson()
        {
            var lesson1 = new Lesson(1, "Math Lesson 1", "http://example.com/1", "Basic math lesson 1");
            var lesson2 = new Lesson(2, "Math Lesson 2", "http://example.com/2", "Basic math lesson 2");
            var quiz = new Quiz(1, "Math Quiz", 100, 60);

            lesson1.AddQuiz(quiz);

            var ex = Assert.Throws<ArgumentException>(() => lesson2.AddQuiz(quiz));
            Assert.That(ex.Message, Is.EqualTo("Quiz is already associated with another lesson."));
        }
    }
}

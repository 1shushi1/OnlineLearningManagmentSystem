using NUnit.Framework;
using System;
using BYT_Project;

namespace BYT_Project.Tests
{
    [TestFixture]
    public class LessonTests
    {
        [Test]
        public void TestGetCorrectLessonInformation()
        {
            var lesson = new Lesson(1, "Lesson 1", "https://video.url", "This is the first lesson");

            Assert.That(lesson.LessonID, Is.EqualTo(1));
            Assert.That(lesson.LessonTitle, Is.EqualTo("Lesson 1"));
            Assert.That(lesson.VideoURL, Is.EqualTo("https://video.url"));
            Assert.That(lesson.LessonDescription, Is.EqualTo("This is the first lesson"));
        }

        [Test]
        public void TestExceptionForInvalidLessonID()
        {
            var ex = Assert.Throws<ArgumentException>(() => new Lesson(-1, "Lesson 1", "https://video.url", "This is the first lesson"));
            Assert.That(ex.Message, Is.EqualTo("Lesson ID must be positive."));
        }

        [Test]
        public void TestExceptionForEmptyLessonTitle()
        {
            var ex = Assert.Throws<ArgumentException>(() => new Lesson(1, "", "https://video.url", "This is the first lesson"));
            Assert.That(ex.Message, Is.EqualTo("Lesson title cannot be empty."));
        }

        [Test]
        public void TestSaveAndLoadLessons()
        {
            var lesson = new Lesson(1, "Lesson 1", "https://video.url", "This is the first lesson");

            Lesson.SaveLessons();
            var success = Lesson.LoadLessons();

            Assert.That(success, Is.True);
            Assert.That(Lesson.LessonsList.Count, Is.EqualTo(1)); // Ensure lesson is saved and loaded
        }
    }
}

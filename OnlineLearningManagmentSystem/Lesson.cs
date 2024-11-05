using System;
using System.Collections.Generic;

public class Lesson
{
    public int LessonID { get; private set; }
    private string _lessonTitle;
    private string _lessonDescription;
    private List<Quiz> _quizzes = new List<Quiz>();
    private List<Assignment> _assignments = new List<Assignment>();

    public string LessonTitle
    {
        get => _lessonTitle;
        set
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Lesson title cannot be empty.");
            _lessonTitle = value;
        }
    }

    public string LessonDescription
    {
        get => _lessonDescription;
        set
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Lesson description cannot be empty.");
            _lessonDescription = value;
        }
    }

    public IReadOnlyList<Quiz> Quizzes => _quizzes.AsReadOnly();
    public IReadOnlyList<Assignment> Assignments => _assignments.AsReadOnly();

    private static List<Lesson> _lessons = new List<Lesson>();

    public Lesson(int lessonID, string title, string description)
    {
        LessonID = lessonID;
        LessonTitle = title;
        LessonDescription = description;
        AddToExtent(this);
    }

    public void AddQuiz(Quiz quiz)
    {
        _quizzes.Add(quiz ?? throw new ArgumentException("Quiz cannot be null."));
    }

    public static void AddToExtent(Lesson lesson)
    {
        _lessons.Add(lesson ?? throw new ArgumentException("Lesson cannot be null."));
    }

    public static IReadOnlyList<Lesson> GetExtent() => _lessons.AsReadOnly();

    public static void ClearExtent() => _lessons.Clear();
}

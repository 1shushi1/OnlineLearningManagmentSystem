using System;
using System.Collections.Generic;

public class Quiz
{
    public int QuizID { get; private set; }
    private string _title;
    private List<Question> _questions = new List<Question>();

    public string Title
    {
        get => _title;
        set
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Quiz title cannot be empty.");
            _title = value;
        }
    }

    public int TotalScore => _questions.Count * 10; 

    public IReadOnlyList<Question> Questions => _questions.AsReadOnly();

    private static List<Quiz> _quizzes = new List<Quiz>();

    public Quiz(int quizID, string title)
    {
        QuizID = quizID;
        Title = title;
        AddToExtent(this);
    }

    public void AddQuestion(Question question)
    {
        if (question == null)
            throw new ArgumentException("Question cannot be null.");
        _questions.Add(question);
    }

    public static void AddToExtent(Quiz quiz)
    {
        _quizzes.Add(quiz ?? throw new ArgumentException("Quiz cannot be null."));
    }

    public static IReadOnlyList<Quiz> GetExtent() => _quizzes.AsReadOnly();

    public static void ClearExtent() => _quizzes.Clear();
}

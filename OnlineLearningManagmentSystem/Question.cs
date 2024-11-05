using System;
using System.Collections.Generic;

public class Question
{
    public int QuestionID { get; private set; }
    private string _text;
    private string _correctAnswer;
    private List<string> _options = new List<string>();

    public string Text
    {
        get => _text;
        set
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Question text cannot be empty.");
            _text = value;
        }
    }

    public string CorrectAnswer
    {
        get => _correctAnswer;
        set
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Correct answer cannot be empty.");
            _correctAnswer = value;
        }
    }

    public IReadOnlyList<string> Options => _options.AsReadOnly();

    private static List<Question> _questions = new List<Question>();

    public Question(int questionID, string text, string correctAnswer)
    {
        QuestionID = questionID;
        Text = text;
        CorrectAnswer = correctAnswer;
        AddToExtent(this);
    }

    public void AddOption(string option)
    {
        if (string.IsNullOrEmpty(option))
            throw new ArgumentException("Option cannot be empty.");
        _options.Add(option);
    }

    public static void AddToExtent(Question question)
    {
        _questions.Add(question ?? throw new ArgumentException("Question cannot be null."));
    }

    public static IReadOnlyList<Question> GetExtent() => _questions.AsReadOnly();

    public static void ClearExtent() => _questions.Clear();
}

using System;
using System.Collections.Generic;

public class Grade
{
    public int GradeID { get; private set; }
    public int TotalScore { get; private set; }

    public string GradeLetter
    {
        get
        {
            if (TotalScore >= 90) return "A";
            if (TotalScore >= 80) return "B";
            if (TotalScore >= 70) return "C";
            if (TotalScore >= 60) return "D";
            return "F";
        }
    }

    private static List<Grade> _grades = new List<Grade>();

    public Grade(int gradeID, int totalScore)
    {
        if (totalScore < 0)
            throw new ArgumentException("Total score cannot be negative.");

        GradeID = gradeID;
        TotalScore = totalScore;
        AddToExtent(this);
    }

    public static void AddToExtent(Grade grade)
    {
        _grades.Add(grade ?? throw new ArgumentException("Grade cannot be null."));
    }

    public static IReadOnlyList<Grade> GetExtent() => _grades.AsReadOnly();

    public static void ClearExtent() => _grades.Clear();
}

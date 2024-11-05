using System;
using System.Collections.Generic;

public class SubmittedAssignment
{
    public int SubmissionID { get; private set; }
    public DateTime SubmissionDate { get; private set; }
    public int Score { get; private set; }

    public SubmittedAssignment(int submissionID, int score)
    {
        SubmissionID = submissionID;
        SubmissionDate = DateTime.Now;
        Score = score;
        AddToExtent(this);
    }

    private static List<SubmittedAssignment> _submissions = new List<SubmittedAssignment>();

    public static void AddToExtent(SubmittedAssignment submission)
    {
        _submissions.Add(submission ?? throw new ArgumentException("Submission cannot be null."));
    }

    public static IReadOnlyList<SubmittedAssignment> GetExtent() => _submissions.AsReadOnly();

    public static void ClearExtent() => _submissions.Clear();
}

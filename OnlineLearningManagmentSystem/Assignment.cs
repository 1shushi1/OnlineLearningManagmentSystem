using System;
using System.Collections.Generic;

public class Assignment
{
    public int AssignmentID { get; private set; }
    private string _title;
    private string _description;
    private List<SubmittedAssignment> _submittedAssignments = new List<SubmittedAssignment>();

    public string Title
    {
        get => _title;
        set
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Assignment title cannot be empty.");
            _title = value;
        }
    }

    public string Description
    {
        get => _description;
        set
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Assignment description cannot be empty.");
            _description = value;
        }
    }

    private static List<Assignment> _assignments = new List<Assignment>();

    public Assignment(int assignmentID, string title, string description)
    {
        AssignmentID = assignmentID;
        Title = title;
        Description = description;
        AddToExtent(this);
    }

    public void AddSubmittedAssignment(SubmittedAssignment submission)
    {
        if (submission == null)
            throw new ArgumentException("Submitted assignment cannot be null.");
        _submittedAssignments.Add(submission);
    }

    public static void AddToExtent(Assignment assignment)
    {
        _assignments.Add(assignment ?? throw new ArgumentException("Assignment cannot be null."));
    }

    public static IReadOnlyList<Assignment> GetExtent() => _assignments.AsReadOnly();

    public static void ClearExtent() => _assignments.Clear();
}

public class Timetable
{
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    private List<DateTime> _scheduleEntries = new List<DateTime>();

    public Timetable(DateTime startDate, DateTime endDate)
    {
        if (endDate <= startDate)
            throw new ArgumentException("End date must be after start date.");
        StartDate = startDate;
        EndDate = endDate;
    }

    public void AddSession(DateTime session)
    {
        if (session < StartDate || session > EndDate)
            throw new ArgumentException("Session must be within the timetable range.");
        _scheduleEntries.Add(session);
    }

    public IReadOnlyList<DateTime> GetScheduleEntries()
    {
        return _scheduleEntries.AsReadOnly();
    }
}

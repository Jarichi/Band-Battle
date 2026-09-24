public class ActiveNote 
{
    public int Index { get; set; }
    public BeatmapNote Note { get; set; }
    public NoteState State { get; set; }
}

public enum NoteState
{
    Spawned,
    EarlyRange,
    PerfectRange,
    LateRange,
    Missed,
}
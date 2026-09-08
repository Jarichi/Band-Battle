using System.Collections.Generic;

public struct Beatmap
{
    public double BPM;
    public List<BeatmapTrack> Tracks;
}

public struct BeatmapTrack
{
    // instrument id

    public List<BeatmapNote> Notes;
}

public struct BeatmapNote
{
    public int Column;
    public double Beat;
}
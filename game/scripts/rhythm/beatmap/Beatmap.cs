using System.Collections.Generic;

public static class BeatmapLoader
{
	public static Beatmap LoadFromFile(string filename)
	{
		var file = Godot.FileAccess.Open("res://assets/beatmaps/" + filename, Godot.FileAccess.ModeFlags.Read);
		var json = file.GetAsText();
		var options = new System.Text.Json.JsonSerializerOptions
		{
			IncludeFields = true
		};
		return System.Text.Json.JsonSerializer.Deserialize<Beatmap>(json, options);
	}
}

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

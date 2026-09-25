Usage:
- Enable the plugin in Project -> Project Settings -> Plugins.
- Call the `Convert(midiPath, outPath)` method from an EditorScript or add a simple UI.
- `midiPath` and `outPath` can be `res://` paths.

Notes:
- Requires the Melanchall.DryWetMidi NuGet package referenced in game.csproj.
- This is a minimal port of an old Unity editor tool to Godot EditorPlugin in C#.

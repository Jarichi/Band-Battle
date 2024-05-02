make sure the score is collected by the trigger line thingy, and when the song ends, give this to parent.player as some integer for the total score.

score starts at 0 and goes up when notes are hit.

OnTriggerEnter() {IncrementScore(); Destroy(note);}

TODO: note spawn locations
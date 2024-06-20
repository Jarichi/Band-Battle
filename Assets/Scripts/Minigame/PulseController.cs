using UnityEngine;

public class PulseController : MonoBehaviour
{
    private double bpm;
    private float secondPerBeat;
    private int speedPerBeat = 2;
    private float t;
    private Vector3 targetPosition;
    private Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        bpm = Game.Instance.Rhythm.BPM;
        secondPerBeat = 60 / (float)bpm;
        secondPerBeat *= speedPerBeat;

        startPosition = transform.localPosition;
        targetPosition = transform.localPosition + new Vector3(0f, -6f, 0f);


    }

    private void FixedUpdate()
    {
        if (gameObject != null)
        {
            MoveObj();
        }
    }

    private bool IsTriggerLine(Collider2D collision, out TriggerLine triggerLine)
    {
        triggerLine = null;
        if (collision.tag != "minigame_TriggerLine") return false;
        return collision.TryGetComponent(out triggerLine);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsTriggerLine(other, out var triggerLine))
        {
            Destroy(gameObject);
        }
    }

    private void MoveObj()
    {
        t += Time.fixedDeltaTime / secondPerBeat;
        transform.localPosition = Vector3.Lerp(startPosition, targetPosition, t);

    }

}

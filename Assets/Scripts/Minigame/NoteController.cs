using UnityEngine;
using static Rhythm;

public class Note : MonoBehaviour
{
    public float velocity;

    //temp
    Vector3 targetPosition;
    Vector3 startPosition;
    [SerializeField]
    private NoteDirection direction = NoteDirection.Right;
    private bool destroyed;

    private double bpm;
    private float secondPerBeat;
    float t;
    [SerializeField]
    private int noteSpeedInBeats = 2;
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    public void SetDirection(NoteDirection _direction)
    {
        direction = _direction;
    }


    private void Start()
    {
        startPosition = transform.localPosition;
        targetPosition = transform.localPosition + new Vector3(0f, -6f, 0f);
        destroyed = false;

        bpm = Game.Instance.Rhythm.BPM;

        secondPerBeat = 60 / (float)bpm;
        secondPerBeat *= noteSpeedInBeats;
    }

    //@50fps
    private void FixedUpdate()
    {
        if (gameObject != null)
        {
            Move();
        }
    }

    private void Move()
    {
        t += Time.fixedDeltaTime / secondPerBeat;
        transform.localPosition = Vector3.Lerp(startPosition, targetPosition, t);
    }

    public void Despawn()
    {
        destroyed = true;
        Destroy(gameObject);
    }

    public NoteDirection GetDirection() => this.direction;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (ColliderBelongsToTriggerLine(collision, out var triggerLine))
        {
            triggerLine.OnNoteCollision(this, collision);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (destroyed) return;

        if (ColliderBelongsToTriggerLine(collision, out var triggerLine))
        {
            triggerLine.OnNoteExit(this);
        }
    }

    private bool ColliderBelongsToTriggerLine(Collider2D collision, out TriggerLine triggerLine)
    {
        triggerLine = null;
        if (!collision.CompareTag("minigame_TriggerLine")) return false;
        return collision.TryGetComponent(out triggerLine);
    }

    public void SetColor(Color color)
    {
        spriteRenderer.color = color;
    }
}

     
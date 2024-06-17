using UnityEngine;
using static Rhythm;

public class NoteController : MonoBehaviour
{
    [SerializeField]
    private float lifetime;

    private float timeStart;
    private float currentTime;
    private float timeDelta;

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
    private SpriteRenderer spriteRenderer;

    public void SetDirection(NoteDirection _direction)
    {
        direction = _direction;
    }
    
    private void Start()
    {
        timeStart = Time.time;
        startPosition = transform.localPosition;
        targetPosition = transform.localPosition + new Vector3(0f, -6f, 0f);
        destroyed = false;

        bpm = Game.Instance.Rhythm.BPM;

        secondPerBeat = 60 / (float)bpm;
        secondPerBeat *= noteSpeedInBeats;

        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    //@50fps
    private void FixedUpdate()
    {
        if (gameObject != null)
        {
            MoveNote();
            Lifetime();
        }
    }

    private void Lifetime()
    {
        currentTime = Time.time;
        timeDelta = currentTime - timeStart;

        if (timeDelta > lifetime)
        {
            Destroy(gameObject);
        }
    }

    private void MoveNote()
    {
        t += Time.fixedDeltaTime / secondPerBeat;
        transform.localPosition = Vector3.Lerp(startPosition, targetPosition, t);



        //var step = velocity * Time.fixedDeltaTime;
        //transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, step);
    }

    public void Despawn()
    {
        destroyed = true;
        Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (IsTriggerLine(collision, out var triggerLine))
        {
            var inputCollider = DirectionToCollider(direction, triggerLine);
            inputCollider.hasCollision = true;
            if (inputCollider.enabled && inputCollider.collider == collision)
            {
                triggerLine.OnHit(this);
                inputCollider.enabled = false;
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (IsTriggerLine(collision, out var triggerLine))
        {
            var inputCollider = DirectionToCollider(direction, triggerLine);
            inputCollider.hasCollision = false;
            if (destroyed) return;
            triggerLine.OnMiss(this);
        }
    }

    private bool IsTriggerLine(Collider2D collision, out TriggerLine triggerLine)
    {
        triggerLine = null;
        if (collision.tag != "minigame_TriggerLine") return false;
        return collision.TryGetComponent(out triggerLine);
    }

    private InputCollider DirectionToCollider(NoteDirection direction, TriggerLine triggerLine)
    {
        int index = 0;
        switch (direction)
        {
            case NoteDirection.Left:
                 index = 0; break;
            case NoteDirection.Right:
                index = 1; break;
            case NoteDirection.Up:
                index = 2; break;
            case NoteDirection.Down:
                index = 3; break;
        }
        return triggerLine.GetCollider(index);
    }

    public void SetColor(Color color)
    {
        spriteRenderer.color = color;
    }
}

     
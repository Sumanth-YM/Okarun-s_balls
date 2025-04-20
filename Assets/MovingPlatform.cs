using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Collider2D))]
public class MovingPlatform : MonoBehaviour
{
    [Tooltip("Horizontal speed in units per second")]
    public float speed = 5f;

    private Rigidbody2D   _rb;
    private float         _fixedY;
    private Vector2       _currentVelocity;

    // screen clamping
    private float _halfWidth;
    private float _minX, _maxX;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.bodyType = RigidbodyType2D.Kinematic;

        // lock Y
        _fixedY = _rb.position.y;

        // figure out how wide we are in world units:
        var sr = GetComponent<SpriteRenderer>();
        _halfWidth = sr.bounds.extents.x;

        // get camera edges:
        Camera cam = Camera.main;
        Vector3 leftEdge  = cam.ViewportToWorldPoint(new Vector3(0f, 0f, cam.nearClipPlane));
        Vector3 rightEdge = cam.ViewportToWorldPoint(new Vector3(1f, 0f, cam.nearClipPlane));

        // inset by half our width
        _minX = leftEdge.x  + _halfWidth;
        _maxX = rightEdge.x - _halfWidth;
    }

    void FixedUpdate()
    {
        // read input
        float h = Input.GetAxisRaw("Horizontal");
        _currentVelocity = new Vector2(h * speed, 0f);

        // desired new position
        Vector2 newPos = _rb.position + _currentVelocity * Time.fixedDeltaTime;
        newPos.y = _fixedY;                // keep Y locked
        newPos.x = Mathf.Clamp(newPos.x,    // clamp X to screen
                               _minX,
                               _maxX);

        // apply movement
        _rb.MovePosition(newPos);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ball"))
        {
            Rigidbody2D ballRb = collision.collider.attachedRigidbody;
            if (ballRb != null)
            {
                Vector2 v = ballRb.linearVelocity;
                v.x += _currentVelocity.x/2;
                ballRb.linearVelocity = v;
            }
        }
    }
}

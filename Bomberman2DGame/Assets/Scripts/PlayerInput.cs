using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    private Vector2 _direction = Vector2.down;
    public float _moveSpeed = 4f;
    [SerializeField] private float _maxSpeed = 12f;

    [SerializeField] private KeyCode _inputUp = KeyCode.W;
    [SerializeField] private KeyCode _inputDown = KeyCode.S;
    [SerializeField] private KeyCode _inputLeft = KeyCode.A;
    [SerializeField] private KeyCode _inputRight = KeyCode.D;

    [SerializeField] private Animations _spriteRendererUp;
    [SerializeField] private Animations _spriteRendererDown;
    [SerializeField] private Animations _spriteRendererLeft;
    [SerializeField] private Animations _spriteRendererRight;
    [SerializeField] private Animations _spriteRendererDeath;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKey(_inputUp))
        {
            SetDirection(Vector2.up);
        } else if (Input.GetKey(_inputDown))
        {
            SetDirection(Vector2.down);
        } else if (Input.GetKey(_inputLeft))
        {
            SetDirection(Vector2.left);
        } else if (Input.GetKey(_inputRight))
        {
            SetDirection(Vector2.right);
        } else
        {
            SetDirection(Vector2.zero);
        }
    }

    private void FixedUpdate()
    {
        Vector2 velocity = _direction * _moveSpeed;

        //Limiteer snelheid
        velocity = Vector2.ClampMagnitude(velocity, _maxSpeed);

        _rb.linearVelocity = velocity;
    }

    private void SetDirection(Vector2 newDirection)
    {
        _direction = newDirection;

        //...
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {
            Death();
        }
    }

    private void Death()
    {
        enabled = false;
        GetComponent<BombController>().enabled = false;

        _spriteRendererUp.enabled = false;
        _spriteRendererDown.enabled = false;
        _spriteRendererLeft.enabled = false;
        _spriteRendererRight.enabled = false;
        _spriteRendererDeath.enabled = true;

        Invoke(nameof(OnDeath), 1.25f);
    }

    private void OnDeath()
    {
        gameObject.SetActive(false);
    }
}
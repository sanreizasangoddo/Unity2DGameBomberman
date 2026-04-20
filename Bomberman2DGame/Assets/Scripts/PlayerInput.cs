using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    private Vector2 _direction = Vector2.down;
    public float moveSpeed = 4f;
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
    private Animations _activeSpriteRenderer;

    // Wordt één keer aangeroepen bij het starten.
    private void Awake()
    {
        // Haalt Rigidbody op en zet standaard animatie.
        _rb = GetComponent<Rigidbody2D>();
        _activeSpriteRenderer = _spriteRendererDown;
    }

    // Wordt elke frame aangeroepen.
    private void Update()
    {
        // Verwerkt input en bepaalt richting + animatie.
        if (Input.GetKey(_inputUp))
        {
            SetDirection(Vector2.up, _spriteRendererUp);
        } else if (Input.GetKey(_inputDown))
        {
            SetDirection(Vector2.down, _spriteRendererDown);
        } else if (Input.GetKey(_inputLeft))
        {
            SetDirection(Vector2.left, _spriteRendererLeft);
        } else if (Input.GetKey(_inputRight))
        {
            SetDirection(Vector2.right, _spriteRendererRight);
        } else
        {
            // Geen input -> idle
            SetDirection(Vector2.zero, _activeSpriteRenderer);
        }
    }

    // Wordt op vaste intervallen aangeroepen (physics update).
    // Past snelheid toe op de Rigidbody.
    private void FixedUpdate()
    {
        // Bereken velocity op basis van richting en snelheid
        Vector2 velocity = _direction * moveSpeed;

        // Beperk snelheid tot maxSpeed
        velocity = Vector2.ClampMagnitude(velocity, _maxSpeed);

        // Past velocity toe
        _rb.linearVelocity = velocity;
    }

    // Zet de bewegingsrichting en wisselt de animatie.
    private void SetDirection(Vector2 newDirection, Animations spriteRenderer)
    {
        _direction = newDirection;

        // Zet alleen de juiste animatie aan
        _spriteRendererUp.enabled = spriteRenderer == _spriteRendererUp;
        _spriteRendererDown.enabled = spriteRenderer == _spriteRendererDown;
        _spriteRendererLeft.enabled = spriteRenderer == _spriteRendererLeft;
        _spriteRendererRight.enabled = spriteRenderer == _spriteRendererRight;

        // Update actieve animatie
        _activeSpriteRenderer = spriteRenderer;

        // Zet idle als speler niet beweegt
        _activeSpriteRenderer.idle = _direction == Vector2.zero;
    }

    // Detecteert collision met een explosie.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {
            Death();
        }
    }

    // Wordt aangeroepen wanneer de speler doodgaat.
    // Zet beweging en input uit en speelt death animatie.
    private void Death()
    {
        enabled = false;
        GetComponent<BombController>().enabled = false;

        _rb.linearVelocity = Vector2.zero;

        _spriteRendererUp.enabled = false;
        _spriteRendererDown.enabled = false;
        _spriteRendererLeft.enabled = false;
        _spriteRendererRight.enabled = false;
        _spriteRendererDeath.enabled = true;

        Invoke(nameof(OnDeath), 1.25f);
    }

    // Wordt aangeroepen na death animatie.
    // Verwijdert speler en checkt win conditie.
    private void OnDeath()
    {
        gameObject.SetActive(false);
        FindFirstObjectByType<GameManager>().CheckWinState();
    }
}
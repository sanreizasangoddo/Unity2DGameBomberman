using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    private Vector2 _direction = Vector2.down;
    private float _moveSpeed = 4f;

    [SerializeField] private KeyCode _inputUp = KeyCode.W;
    [SerializeField] private KeyCode _inputDown = KeyCode.S;
    [SerializeField] private KeyCode _inputLeft = KeyCode.A;
    [SerializeField] private KeyCode _inputRight = KeyCode.D;

    [SerializeField] private Animations _spriteRendererUp;
    [SerializeField] private Animations _spriteRendererDown;
    [SerializeField] private Animations _spriteRendererLeft;
    [SerializeField] private Animations _spriteRendererRight;

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
        Vector2 position = _rb.position;
        Vector2 movement = _moveSpeed * Time.fixedDeltaTime * _direction;

        _rb.MovePosition(position + movement);
    }

    private void SetDirection(Vector2 newDirection)
    {
        _direction = newDirection;

        //...
    }
}

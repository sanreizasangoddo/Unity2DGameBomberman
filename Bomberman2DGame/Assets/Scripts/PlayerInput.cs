using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    private Vector2 _direction = Vector2.down;
    [SerializeField] private float _moveSpeed = 5f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            SetDirection(Vector2.up);
        } else if (Input.GetKey(KeyCode.S))
        {
            SetDirection(Vector2.down);
        } else if (Input.GetKey(KeyCode.A))
        {
            SetDirection(Vector2.left);
        } else if (Input.GetKey(KeyCode.D))
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

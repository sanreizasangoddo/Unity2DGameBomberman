using UnityEngine;

public class Animations : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    [SerializeField] private Sprite _idleSprite;
    [SerializeField] private Sprite[] _animationSprites;

    [SerializeField] private float _animationTime = 0.25f;
    private int _animationFrame;

    [SerializeField] private bool _loop = true;
    public bool idle = true;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        _spriteRenderer.enabled = true;
    }

    private void OnDisable()
    {
        _spriteRenderer.enabled = false;
    }

    private void Start()
    {
        InvokeRepeating(nameof(NextFrame), _animationTime, _animationTime);
    }

    private void NextFrame()
    {
        _animationFrame++;

        if (_loop && _animationFrame >= _animationSprites.Length)
        {
            _animationFrame = 0;
        }

        if (idle)
        {
            _spriteRenderer.sprite = _idleSprite;
        } else if (_animationFrame >= 0 && _animationFrame < _animationSprites.Length)
        {
            _spriteRenderer.sprite = _animationSprites[_animationFrame];
        }
    }
}
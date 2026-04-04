using UnityEngine;

public class Animations : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    [SerializeField] private Sprite _idleSprite;
    [SerializeField] private Sprite[] _animationSprites;

    [SerializeField] private float _animationTime = 0.25f;
    private int _animationFrame;

    [SerializeField] private bool _loop = true;
    public bool idle = true; // Of het object idle is (true = idle sprite tonen)

    private void Awake()
    {
        // Haalt de SpriteRenderer component op.
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Wordt aangeroepen wanneer het object geactiveerd wordt.
    private void OnEnable()
    {
        _spriteRenderer.enabled = true;
    }

    // Wordt aangeroepen wanneer het object gedeactiveerd wordt.
    private void OnDisable()
    {
        _spriteRenderer.enabled = false;
    }

    // Roept elke _animationTime seconden NextFrame aan.
    private void Start()
    {
        InvokeRepeating(nameof(NextFrame), _animationTime, _animationTime);
    }

    // Behandelt looping en idle toestand.
    private void NextFrame()
    {
        _animationFrame++;

        // Als looping aanstaat en we zijn aan het einde -> reset naar begin
        if (_loop && _animationFrame >= _animationSprites.Length)
        {
            _animationFrame = 0;
        }

        // Als idle -> toon idle sprite
        if (idle)
        {
            _spriteRenderer.sprite = _idleSprite;
        }
        // Anders toon animatie frame (als binnen bounds)
        else if (_animationFrame >= 0 && _animationFrame < _animationSprites.Length)
        {
            _spriteRenderer.sprite = _animationSprites[_animationFrame];
        }
    }
}
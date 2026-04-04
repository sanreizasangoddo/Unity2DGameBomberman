using System.Collections;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BombController : MonoBehaviour
{
    [Header("Bomb")]
    [SerializeField] private KeyCode _bombInput = KeyCode.Space;
    [SerializeField] private GameObject _bombPrefab;
    [SerializeField] private LayerMask _bombLayer;
    private float _bombFuseTime = 3f;
    public int bombAmount = 1;
    private int _bombsRemaining;
    private int _maxBombAmount = 8;

    [Header("Explosion")]
    [SerializeField] private Explosion _explosionPrefab;
    [SerializeField] private LayerMask _explosionLayerMask;
    private float _explosionDuration = 0.5f;
    public int _explosionRadius = 2;
    [SerializeField] private int _maxExplosionRadius = 8;

    [Header("Destructible")]
    [SerializeField] private Destructibles _destructiblePrefab;
    [SerializeField] private Tilemap _destructibleTiles;

    private void OnEnable()
    {
        _bombsRemaining = bombAmount;
    }

    private void Update()
    {
        if (_bombsRemaining > 0 && Input.GetKeyDown(_bombInput))
        {
            StartCoroutine(PlaceBomb());
        }
    }

    float RoundToHalfNoIntegers(float value)
    {
        return Mathf.Floor(value) + 0.5f;
    }

    private IEnumerator PlaceBomb()
    {
        Vector2 position = transform.position;

        position.x = RoundToHalfNoIntegers(position.x);
        position.y = RoundToHalfNoIntegers(position.y);

        // Check of er al een bom ligt
        Collider2D hit = Physics2D.OverlapBox(position, Vector2.one * 0.4f, 0f, _bombLayer);

        if (hit != null)
        {
            yield break; // stop -> geen bom plaatsen
        }

        GameObject bomb = Instantiate(_bombPrefab, position, Quaternion.identity);

        Bomb bombScript = bomb.GetComponent<Bomb>();
        bombScript.Init(this);

        _bombsRemaining--;

        yield return new WaitForSeconds(_bombFuseTime);

        if (bomb != null)
        {
            bombScript.ExplodeNow();
        }
    }

    private void Explode(Vector2 position, Vector2 direction, int length)
    {
        if (length <= 0) return;

        position += direction;

        Collider2D hit = Physics2D.OverlapBox(position, Vector2.one / 2f, 0f);

        if (hit != null)
        {
            // Check of het een bom is
            Bomb bomb = hit.GetComponent<Bomb>();
            if (bomb != null)
            {
                StartCoroutine(DelayedExplosion(bomb, 0.1f)); // Chain Reaction
                return; // STOP explosie hier
            }

            // Destructible
            if (((1 << hit.gameObject.layer) & _explosionLayerMask) != 0)
            {
                ClearDestructible(position);
                return; // STOP explosie
            }
        }

        // Geen obstakel -> ga door
        Explosion explosion = Instantiate(_explosionPrefab, position, Quaternion.identity);
        explosion.SetActiveRenderer(length > 1 ? explosion.middleExplosion : explosion.endExplosion);
        explosion.SetDirection(direction);
        explosion.DestroyAfter(_explosionDuration);

        Explode(position, direction, length - 1);
    }

    private void ClearDestructible(Vector2 position)
    {
        Vector3Int cell = _destructibleTiles.WorldToCell(position);
        TileBase tile = _destructibleTiles.GetTile(cell);

        if (tile != null)
        {
            Instantiate(_destructiblePrefab, position, Quaternion.identity);
            _destructibleTiles.SetTile(cell, null);
        }
    }

    public void AddBomb()
    {
       if (bombAmount < _maxBombAmount)
        {
            bombAmount++;
            _bombsRemaining++;
        }
    }

    public void IncreaseExplosionRadius()
    {
        if (_explosionRadius < _maxExplosionRadius)
        {
            _explosionRadius++;
        }
    }

    public void TriggerExplosion(Vector2 position)
    {
        position.x = RoundToHalfNoIntegers(position.x);
        position.y = RoundToHalfNoIntegers(position.y);

        Explosion explosion = Instantiate(_explosionPrefab, position, Quaternion.identity);
        explosion.SetActiveRenderer(explosion.startExplosion);
        explosion.DestroyAfter(_explosionDuration);

        Explode(position, Vector2.up, _explosionRadius);
        Explode(position, Vector2.down, _explosionRadius);
        Explode(position, Vector2.left, _explosionRadius);
        Explode(position, Vector2.right, _explosionRadius);

        _bombsRemaining++;
    }

    private IEnumerator DelayedExplosion(Bomb bomb, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (bomb != null)
        {
            bomb.ExplodeNow();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Bomb"))
        {
            Collider2D bombCollider = collision;

            // Zet de bom solid zodra speler eruit is
            bombCollider.isTrigger = false;
        }
    }
}
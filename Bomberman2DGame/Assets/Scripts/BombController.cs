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

    // Reset beschikbare bommen wanneer object actief wordt
    private void OnEnable()
    {
        _bombsRemaining = bombAmount;
    }

    // Checkt input om bom te plaatsen
    private void Update()
    {
        if (_bombsRemaining > 0 && Input.GetKeyDown(_bombInput))
        {
            StartCoroutine(PlaceBomb());
        }
    }

    // Rondt positie af naar .5 grid (bijv. 1.2 -> 1.5)
    float RoundToHalfNoIntegers(float value)
    {
        return Mathf.Floor(value) + 0.5f;
    }

    // Plaatst een bom op de grid positie van de speler
    private IEnumerator PlaceBomb()
    {
        Vector2 position = transform.position;

        position.x = RoundToHalfNoIntegers(position.x);
        position.y = RoundToHalfNoIntegers(position.y);

        // Check of er al een bom ligt op deze tile
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

    // Recursive functie die explosie in een richting uitbreidt
    private void Explode(Vector2 position, Vector2 direction, int length)
    {
        if (length <= 0) return;

        position += direction;

        Collider2D hit = Physics2D.OverlapBox(position, Vector2.one / 2f, 0f);

        if (hit != null)
        {
            // Chain reaction met een andere bom
            Bomb bomb = hit.GetComponent<Bomb>();
            if (bomb != null)
            {
                StartCoroutine(DelayedExplosion(bomb, 0.1f));
                return; // Stop explosie hier
            }

            // Item geraakt
            if (hit.TryGetComponent(out ItemPickup item))
            {
                item.HitByExplosion(); // effect + destroy
                return; // stop explosie
            }

            //Destructible block geraakt
            if (((1 << hit.gameObject.layer) & _explosionLayerMask) != 0)
            {
                ClearDestructible(position);
                return; // stop explosie
            }
        }

        // Geen obstakel -> spawn explosie en ga door
        Explosion explosion = Instantiate(_explosionPrefab, position, Quaternion.identity);
        explosion.SetActiveRenderer(length > 1 ? explosion.middleExplosion : explosion.endExplosion);
        explosion.SetDirection(direction);
        explosion.DestroyAfter(_explosionDuration);

        Explode(position, direction, length - 1);
    }

    // Verwijdert destructible tile en spawn effect
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

    // Start een explosie op een specifieke positie
    public void TriggerExplosion(Vector2 position)
    {
        position.x = RoundToHalfNoIntegers(position.x);
        position.y = RoundToHalfNoIntegers(position.y);

        // Midden explosie
        Explosion explosion = Instantiate(_explosionPrefab, position, Quaternion.identity);
        explosion.SetActiveRenderer(explosion.startExplosion);
        explosion.DestroyAfter(_explosionDuration);

        Explode(position, Vector2.up, _explosionRadius);
        Explode(position, Vector2.down, _explosionRadius);
        Explode(position, Vector2.left, _explosionRadius);
        Explode(position, Vector2.right, _explosionRadius);

        _bombsRemaining++;
    }

    // Kleine delay voor chain reactions
    private IEnumerator DelayedExplosion(Bomb bomb, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (bomb != null)
        {
            bomb.ExplodeNow();
        }
    }

    // Zorgt dat bom solid wordt nadat speler eruit loopt
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
using System.Collections;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class BombController : MonoBehaviour
{
    [Header("Bomb")]
    [SerializeField] private KeyCode _bombInput = KeyCode.Space;
    [SerializeField] private GameObject _bombPrefab;
    private float _bombFuseTime = 3f;
    private int _bombAmount = 1;
    private int _bombsRemaining;

    [Header("Explosion")]
    [SerializeField] private Explosion _explosionPrefab;
    [SerializeField] private LayerMask _explosionLayerMask;
    private float _explosionDuration = 1f;
    private int _explosionRadius = 2;

    private void OnEnable()
    {
        _bombsRemaining = _bombAmount;
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

        GameObject bomb = Instantiate(_bombPrefab, position, Quaternion.identity);
        _bombsRemaining--;

        yield return new WaitForSeconds(_bombFuseTime);

        position = bomb.transform.position;
        position.x = RoundToHalfNoIntegers(position.x);
        position.y = RoundToHalfNoIntegers(position.y);

        Explosion explosion = Instantiate(_explosionPrefab, position, Quaternion.identity);
        explosion.SetActiveRenderer(explosion._startExplosion);
        Destroy(explosion.gameObject, _explosionDuration);

        Explode(position, Vector2.up, _explosionRadius);
        Explode(position, Vector2.down, _explosionRadius);
        Explode(position, Vector2.left, _explosionRadius);
        Explode(position, Vector2.right, _explosionRadius);

        Destroy(bomb);
        _bombsRemaining++;
    }

    private void Explode(Vector2 position, Vector2 direction, int length)
    {
        if (length <= 0)
        {
            return;
        }

        position += direction;

        if (Physics2D.OverlapBox(position, Vector2.one / 2f, 0f, _explosionLayerMask))
        {
            return;
        }

        Explosion explosion = Instantiate(_explosionPrefab, position, Quaternion.identity);
        explosion.SetActiveRenderer(length > 1 ? explosion._middleExplosion : explosion._endExplosion);
        explosion.SetDirection(direction);
        Destroy(explosion.gameObject, _explosionDuration);

        Explode(position, direction, length - 1);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bomb"))
        {
            collision.isTrigger = false;
        }
    }
}

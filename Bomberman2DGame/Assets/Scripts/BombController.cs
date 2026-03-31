using System.Collections;
using UnityEngine;

public class BombController : MonoBehaviour
{
    [SerializeField] private KeyCode _bombInput = KeyCode.Space;
    [SerializeField] private GameObject _bombPrefab;
    private float _bombFuseTime = 3f;
    private int _bombAmount = 1;
    private int _bombsRemaining;

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

        Destroy(bomb);
        _bombsRemaining++;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bomb"))
        {
            collision.isTrigger = false;
        }
    }
}

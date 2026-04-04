using UnityEngine;

public class Destructibles : MonoBehaviour
{
    private float _destructionTime = 0.7f;

    [Range(0f, 1f)]
    [SerializeField] private float _itemSpawnChance = 0.2f;
    [SerializeField] private GameObject[] _spawnableItems;

    private void Start()
    {
        Destroy(gameObject, _destructionTime);
    }

    private void OnDestroy()
    {
        // Check of er items zijn en of spawn kans gehaald wordt
        if (_spawnableItems.Length > 0 && Random.value < _itemSpawnChance)
        {
            // Kies random item uit array
            int randomIndex = Random.Range(0, _spawnableItems.Length);

            // Spawn item op dezelfde positie
            Instantiate(_spawnableItems[randomIndex], transform.position, Quaternion.identity);
        }
    }
}
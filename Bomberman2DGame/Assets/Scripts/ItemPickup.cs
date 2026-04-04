using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public enum ItemType
    {
        BlastRadius,
        ExtraBomb,
        SpeedIncrease
    }

    [SerializeField] private ItemType _type;
    [SerializeField] private Explosion _explosionPrefab;
    [SerializeField] private float _explosionDuration = 0.5f;

    // Wordt aangeroepen wanneer speler item oppakt
    private void OnItemPickup(GameObject player)
    {
        switch (_type)
        {
            case ItemType.BlastRadius:
                player.GetComponent<BombController>().IncreaseExplosionRadius();
                break;

            case ItemType.ExtraBomb:
                player.GetComponent<BombController>().AddBomb();
                break;

            case ItemType.SpeedIncrease:
                player.GetComponent<PlayerInput>()._moveSpeed++;
                break;
        }

        Destroy(gameObject);
    }

    // Wordt aangeroepen wanneer item geraakt wordt door explosie
    public void HitByExplosion()
    {
        // Spawn visuele explosie op item positie
        Explosion explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        explosion.SetActiveRenderer(explosion.startExplosion);
        explosion.DestroyAfter(_explosionDuration);

        // Daarna item verwijderen
        Destroy(gameObject);
    }

    // Detecteert wanneer speler het item oppakt
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnItemPickup(collision.gameObject);
        }
    }
}
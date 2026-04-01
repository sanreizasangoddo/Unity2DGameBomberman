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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnItemPickup(collision.gameObject);
        }
    }
}
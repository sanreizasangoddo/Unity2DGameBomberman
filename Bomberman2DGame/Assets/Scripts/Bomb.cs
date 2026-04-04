using UnityEngine;

public class Bomb : MonoBehaviour
{
    private BombController _controller;
    private bool _exploded = false;

    // Initialiseert de bom met een controller
    public void Init(BombController controller)
    {
        _controller = controller;
    }

    // Laat de bom direct exploderen
    // (bijv. na fuse of door chain reaction)
    public void ExplodeNow()
    {
        // Voorkom dubbele explosies
        if (_exploded) return;

        _exploded = true;
        _controller.TriggerExplosion(transform.position);
        Destroy(gameObject);
    }
}
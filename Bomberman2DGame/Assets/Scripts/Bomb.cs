using UnityEngine;

public class Bomb : MonoBehaviour
{
    private BombController _controller;
    private bool _exploded = false;

    public void Init(BombController controller)
    {
        _controller = controller;
    }

    public void ExplodeNow()
    {
        if (_exploded) return;

        _exploded = true;
        _controller.TriggerExplosion(transform.position);
        Destroy(gameObject);
    }
}

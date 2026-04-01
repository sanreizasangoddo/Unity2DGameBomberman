using UnityEngine;

public class Explosion : MonoBehaviour
{
    public Animations _startExplosion;
    public Animations _middleExplosion;
    public Animations _endExplosion;

    public void SetActiveRenderer(Animations renderer)
    {
        _startExplosion.enabled = renderer == _startExplosion;
        _middleExplosion.enabled = renderer == _middleExplosion;
        _endExplosion.enabled = renderer == _endExplosion;
    }

    public void SetDirection(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x);
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
    }

    public void DestroyAfter(float seconds)
    {
        Destroy(gameObject, seconds);
    }
}
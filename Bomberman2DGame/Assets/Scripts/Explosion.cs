using UnityEngine;

public class Explosion : MonoBehaviour
{
    public Animations startExplosion;
    public Animations middleExplosion;
    public Animations endExplosion;

    public void SetActiveRenderer(Animations renderer)
    {
        startExplosion.enabled = renderer == startExplosion;
        middleExplosion.enabled = renderer == middleExplosion;
        endExplosion.enabled = renderer == endExplosion;
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
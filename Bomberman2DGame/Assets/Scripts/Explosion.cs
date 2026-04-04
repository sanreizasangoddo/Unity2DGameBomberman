using UnityEngine;

public class Explosion : MonoBehaviour
{
    public Animations startExplosion;
    public Animations middleExplosion;
    public Animations endExplosion;

    // Zet alleen de juiste animatie actief.
    public void SetActiveRenderer(Animations renderer)
    {
        startExplosion.enabled = renderer == startExplosion;
        middleExplosion.enabled = renderer == middleExplosion;
        endExplosion.enabled = renderer == endExplosion;
    }

    // Draait de explosie in de juiste richting (up, down, left, right).
    public void SetDirection(Vector2 direction)
    {
        // Bereken hoek op basis van richting
        float angle = Mathf.Atan2(direction.y, direction.x);

        // Zet rotatie om naar graden en pas toe
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
    }

    // Vernietigt de explosie na een aantal seconden.
    public void DestroyAfter(float seconds)
    {
        Destroy(gameObject, seconds);
    }
}
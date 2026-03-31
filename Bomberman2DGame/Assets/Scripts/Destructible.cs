using UnityEngine;

public class Destructibles : MonoBehaviour
{
    private float _destructionTime = 1f;

    private void Start()
    {
        Destroy(gameObject, _destructionTime);
    }
}

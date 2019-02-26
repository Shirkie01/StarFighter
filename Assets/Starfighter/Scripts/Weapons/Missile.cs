using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField]
    protected float damage;

    [SerializeField]
    private float timeToLive = 10;

    // Start is called before the first frame update
    private void Start()
    {
        Destroy(gameObject, timeToLive);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(nameof(OnCollisionEnter));
        var v = collision.collider.GetComponent<Vitality>();
        if (v != null)
        {
            v.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
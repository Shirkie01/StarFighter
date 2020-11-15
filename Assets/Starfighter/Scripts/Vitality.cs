using Mirror;
using UnityEngine;

public class Vitality : NetworkBehaviour
{
    [SerializeField]
    private float maxHealth;

    private float health;

    public float HealthRatio => health / maxHealth;

    [SerializeField]
    private GameObject destroyedPrefab;

    private void Start()
    {
        if (maxHealth <= 0)
        {
            Debug.Log($"{name} has a vitality component, but zero or negative health!");
        }
        health = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        CmdTakeDamage(amount);
        Debug.Log($"{name} takes {amount} damage: {health}/{maxHealth}");
    }

    [Server]
    private void CmdTakeDamage(float amount)
    {
        health -= amount;

        if (health <= 0)
        {
            health = 0;
            SendMessage("OnDestroyed", SendMessageOptions.DontRequireReceiver);
            if (destroyedPrefab != null)
            {
                InstantiateDestroyedPrefab();
                RpcDestroyed();
            }
            NetworkServer.Destroy(gameObject);
        }
    }

    [ClientRpc]
    private void RpcDestroyed()
    {
        InstantiateDestroyedPrefab();
    }

    private void InstantiateDestroyedPrefab()
    {
        GameObject go = Instantiate(destroyedPrefab, transform.position, transform.rotation);
    }

    private void OnCollisionEnter(Collision collision)
    {
        CmdTakeDamage(10);
    }
}
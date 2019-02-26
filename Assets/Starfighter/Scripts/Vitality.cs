using UnityEngine;
using UnityEngine.Networking;

public class Vitality : NetworkBehaviour
{
    [SerializeField]
    private float _health = 2250;

    public void TakeDamage(float amount)
    {
        CmdTakeDamage(amount);
    }

    [Server]
    private void CmdTakeDamage(float amount)
    {
        _health -= amount;

        if (_health <= 0)
        {
            _health = 0;
            SendMessage("OnDestroyed", SendMessageOptions.DontRequireReceiver);
            NetworkServer.Destroy(gameObject);
        }
    }
}
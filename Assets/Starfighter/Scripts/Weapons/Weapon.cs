using UnityEngine.Networking;

public abstract class Weapon : NetworkBehaviour
{
    public abstract void Fire();
}
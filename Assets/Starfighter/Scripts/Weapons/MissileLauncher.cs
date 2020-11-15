using UnityEngine;
using Mirror;

public class MissileLauncher : Weapon
{
    [SerializeField]
    protected GameObject missilePrefab;

    [SerializeField]
    protected Transform spawnTransform;

    public override void Fire()
    {
        CmdFire();
    }

    [Server]
    protected virtual void CmdFire()
    {
        GameObject go = Instantiate(missilePrefab, spawnTransform.position, spawnTransform.rotation);
        var rb = go.GetComponent<Rigidbody>();
        rb.AddRelativeForce(Vector3.forward * GetComponentInParent<Starfighter>().Speed * 3f, ForceMode.VelocityChange);
        NetworkServer.Spawn(go);
    }
}
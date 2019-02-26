using UnityEngine;
using UnityEngine.Networking;

public class MissileLauncher : Weapon
{
    [SerializeField]
    protected GameObject missilePrefab;

    [SerializeField]
    protected Transform spawnTransform;

    public override void Fire()
    {
        Debug.Log(nameof(Fire));
        CmdFire();
    }

    [Server]
    protected virtual void CmdFire()
    {
        Debug.Log(nameof(CmdFire));
        GameObject go = Instantiate(missilePrefab, spawnTransform.position, spawnTransform.rotation);
        var rb = go.GetComponent<Rigidbody>();
        rb.AddRelativeForce(Vector3.forward * GetComponentInParent<Starfighter>().Speed * 5f, ForceMode.VelocityChange);
        NetworkServer.Spawn(go);
    }
}
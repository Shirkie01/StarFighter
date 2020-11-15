using UnityEngine;
using Mirror;

public class Laser : Weapon
{
    [SerializeField]
    private GameObject laserBolt;

    [SerializeField]
    private Transform spawnTransform;

    public override void Fire()
    {
        GetComponent<AudioSource>().PlayOneShot(fireClip);
        CmdFire();
    }

    [Server]
    private void CmdFire()
    {
        GameObject go = Instantiate(laserBolt, spawnTransform.position, spawnTransform.rotation);
        var rb = go.GetComponent<Rigidbody>();
        rb.AddRelativeForce(Vector3.forward * GetComponentInParent<Starfighter>().Speed * 10f, ForceMode.VelocityChange);
        NetworkServer.Spawn(go);
    }
}
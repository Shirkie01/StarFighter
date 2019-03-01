using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(AudioSource))]
public abstract class Weapon : NetworkBehaviour
{
    [SerializeField]
    protected AudioClip fireClip;

    public abstract void Fire();
}
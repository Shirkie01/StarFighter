using Mirror;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class Weapon : NetworkBehaviour
{
    [SerializeField]
    protected AudioClip fireClip;

    public abstract void Fire();
}
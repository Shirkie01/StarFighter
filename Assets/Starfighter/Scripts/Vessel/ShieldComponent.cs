using Mirror;
using UnityEngine;

public class ShieldComponent : VesselComponent
{
    [SerializeField]
    private float maxStrength;

    [SyncVar(hook = nameof(OnShieldStrengthChanged))]
    private float strength;

    public override void OnStartServer()
    {
        base.OnStartServer();
        strength = maxStrength;
    }

    protected void OnShieldStrengthChanged(float oldValue, float newValue)
    {
    }
}
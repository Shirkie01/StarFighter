using Mirror;

public abstract class VesselComponent : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnPowerChanged))]
    private float power = 1;

    public float Power => power;

    private void OnPowerChanged(float oldValue, float newValue)
    {
    }
}
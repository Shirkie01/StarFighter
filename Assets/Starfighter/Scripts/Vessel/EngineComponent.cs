using UnityEngine;

public class EngineComponent : VesselComponent
{
    [SerializeField]
    private float velocity;

    [SerializeField]
    private float velocityMultiplier;

    public float MaximumVelocity => velocity + (Power - 1) * velocityMultiplier;
}
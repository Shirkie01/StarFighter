using UnityEngine;
using UnityEngine.Networking;

public class Starfighter : NetworkBehaviour
{
    [SyncVar]
    public Color color;

    private float acceleration = 50;
    private float minSpeed = 35;
    private float midSpeed = 70;
    private float maxSpeed = 95;

    public float Speed { get; private set; }

    private float rotationRate = 60; // degrees per second

    private Vector3 input;

    private bool inverted;

    [SerializeField]
    private Weapon primaryWeapon;

    [SerializeField]
    private Weapon secondaryWeapon;

    // Use this for initialization
    private void Start()
    {
        Debug.Log("Client: Start");
        ChangeColor(color);

        if (!hasAuthority)
        {
            enabled = false;
            return;
        }

        var cam = Camera.main;
        cam.transform.parent = transform;
        cam.transform.localPosition = new Vector3(0, 5, -20);
        cam.transform.localRotation = Quaternion.identity;
    }

    private void ChangeColor(Color color)
    {
        Renderer[] rends = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rends)
            r.material.color = color;
    }

    // Update is called once per frame
    private void Update()
    {
        // Rotation on the x-axis is pitch, rotation on the y-axis is yaw, rotation on the z-axis is roll
        input = new Vector3(Input.GetAxis("Vertical") * (inverted ? -1 : 1), Input.GetAxis("Horizontal"), Input.GetAxis("Roll")) * rotationRate;

        // Currently instantaneous transition. Should be modified to use acceleration parameter.
        Speed = Input.GetKey(KeyCode.LeftShift) ? maxSpeed : midSpeed;

        // If user presses 'O', look at the origin. This is meant to help find other ships until
        // there are recognizable landmarks
        if (Input.GetKeyDown(KeyCode.O))
        {
            transform.LookAt(Vector3.zero);
        }

        if (Input.GetMouseButtonDown(0))
        {
            FirePrimaryWeapon();
        }

        if (Input.GetMouseButtonDown(1))
        {
            FireSecondaryWeapon();
        }
    }

    private void LateUpdate()
    {
        transform.Rotate(Vector3.right, input.x * Time.fixedDeltaTime);
        transform.Rotate(Vector3.up, input.y * Time.fixedDeltaTime);
        transform.Rotate(Vector3.forward, input.z * Time.fixedDeltaTime);
        transform.Translate(Vector3.forward * Speed * Time.fixedDeltaTime);
    }

    private void OnGUI()
    {
        GUILayout.Label(transform.position.ToString());
    }

    private void FirePrimaryWeapon()
    {
        Debug.Log(nameof(FirePrimaryWeapon));
        primaryWeapon.Fire();
    }

    private void FireSecondaryWeapon()
    {
        Debug.Log(nameof(FireSecondaryWeapon));
        secondaryWeapon.Fire();
    }
}
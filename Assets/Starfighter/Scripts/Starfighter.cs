using UnityEngine;
using UnityEngine.Networking;

public class Starfighter : NetworkBehaviour
{
    [SyncVar]
    public Color color;

    private float rotationRate = 60; // degrees per second
    private float maxSpeed = 100; // meters per second

    private Vector3 input;

    private float throttle;

    public float Throttle
    {
        get
        {
            return throttle;
        }
        set
        {
            throttle = Mathf.Clamp01(value);
        }
    }

    private bool inverted;

    // Use this for initialization
    private void Start()
    {
        ChangeColor(color);

        if (!isLocalPlayer)
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
        UpdateThrottle();

        // If user presses 'O', look at the origin. This is meant to help find other ships until
        // there are recognizable landmarks
        if (Input.GetKeyDown(KeyCode.O))
        {
            transform.LookAt(Vector3.zero);
        }
    }

    private void UpdateThrottle()
    {
        // Punch it!
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Throttle = 1;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            Throttle += Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            Throttle -= Time.deltaTime;
        }
    }

    private void LateUpdate()
    {
        transform.Rotate(Vector3.right, input.x * Time.fixedDeltaTime);
        transform.Rotate(Vector3.up, input.y * Time.fixedDeltaTime);
        transform.Rotate(Vector3.forward, input.z * Time.fixedDeltaTime);
        transform.Translate(Vector3.forward * throttle * maxSpeed * Time.fixedDeltaTime);
    }

    private void OnGUI()
    {
        GUILayout.Label(string.Format("Throttle : {0:P1}", Throttle));
    }
}
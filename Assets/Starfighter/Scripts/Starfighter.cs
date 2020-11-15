using System.Collections;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

[RequireComponent(typeof(Vitality))]
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
    private Weapon[] primaryWeapons;

    private int primaryWeaponIndex = 0;
    private bool canFire = true;
    private float cooldown = 0.3f;

    [SerializeField]
    private Weapon[] secondaryWeapons;

    private int secondaryWeaponIndex = 0;

    private Vitality vitality;
    private Slider healthSlider;

    public PlayerConnection playerConnection;

    // Use this for initialization
    private void Start()
    {
        // Do this on all clients
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

        if (primaryWeapons == null)
        {
            Debug.Log($"{name} does not have any primary weapons! Good luck I guess");
        }

        vitality = GetComponent<Vitality>();
        healthSlider = GameObject.Find("HealthSlider").GetComponent<Slider>();
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            vitality.TakeDamage(100);
        }

        if (Input.GetMouseButton(0))
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

        healthSlider.value = vitality.HealthRatio;
    }

    private void OnGUI()
    {
        GUILayout.Label(transform.position.ToString());
    }

    private void FirePrimaryWeapon()
    {
        if (!canFire)
        {
            return;
        }
        StartCoroutine(Cooldown(cooldown));

        if (primaryWeaponIndex >= primaryWeapons.Length)
        {
            primaryWeaponIndex = 0;
        }
        primaryWeapons[primaryWeaponIndex++].Fire();
    }

    private void FireSecondaryWeapon()
    {
        if (secondaryWeaponIndex >= secondaryWeapons.Length)
        {
            secondaryWeaponIndex = 0;
        }
        secondaryWeapons[secondaryWeaponIndex++].Fire();
    }

    private void OnDestroyed()
    {
        Camera.main.transform.parent = null;
        StartCoroutine(playerConnection.Respawn());
    }

    private IEnumerator Cooldown(float cooldownTime)
    {
        canFire = false;
        yield return new WaitForSeconds(cooldownTime);
        canFire = true;
    }
}
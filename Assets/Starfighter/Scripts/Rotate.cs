using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField]
    private float rotationRate; // degrees per second

    [SerializeField]
    private Vector3 rotationAxis;

    // Update is called once per frame
    private void Update()
    {
        transform.Rotate(rotationAxis, rotationRate * Time.deltaTime);
    }
}
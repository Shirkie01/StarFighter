using Mirror;
using System.Linq;
using UnityEngine;

public class NetworkColor : NetworkBehaviour
{
    [SyncVar(hook = nameof(SetColor))]
    private Color playerColor = Color.black;

    // Unity makes a clone of the Material every time GetComponent<Renderer>().material is used.
    // Cache it here and Destroy it in OnDestroy to prevent a memory leak.
    private Material[] cachedMaterials;

    public override void OnStartServer()
    {
        base.OnStartServer();
        playerColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
    }

    private void SetColor(Color oldColor, Color newColor)
    {
        if (cachedMaterials == null)
            cachedMaterials = GetComponentsInChildren<Renderer>().Select(r => r.material).ToArray();

        foreach (var m in cachedMaterials)
        {
            m.color = newColor;
        }
    }

    private void OnDestroy()
    {
        if (cachedMaterials != null)
        {
            foreach (var m in cachedMaterials)
            {
                Destroy(m);
            }
        }
    }
}
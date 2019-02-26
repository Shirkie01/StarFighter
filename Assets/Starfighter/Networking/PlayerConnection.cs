using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerConnection : NetworkBehaviour
{
    [SerializeField]
    private GameObject player;

    /// <summary>
    /// This is a hook that is invoked when the client is started.
    /// </summary>
    public override void OnStartClient()
    {
        Debug.Log(nameof(OnStartClient));
    }

    /// <summary>
    /// Called when the local player object has been set up.
    ///
    /// This happens after OnStartClient(), as it is triggered by an ownership message from the
    /// server.This is an appropriate place to activate components or functionality that should only
    /// be active for the local player, such as cameras and input.
    /// </summary>
    public override void OnStartLocalPlayer()
    {
        Debug.Log(nameof(OnStartLocalPlayer));
        CmdSpawnPlayer();

        player.GetComponent<Starfighter>().enabled = true;
    }

    [Server]
    private void CmdSpawnPlayer()
    {
        if (!connectionToClient.isReady)
        {
            StartCoroutine(WaitForReady(() => CmdSpawnPlayer()));
            return;
        }

        if (player == null)
        {
            Debug.Log("Server: 'Player' was not assigned");
        }

        GameObject go = Instantiate(player);
        go.transform.position = UnityEngine.Random.insideUnitSphere * 500;
        if (!NetworkServer.SpawnWithClientAuthority(go, connectionToClient))
        {
            Debug.Log("Server: Failed to spawn player");
        }
    }

    private IEnumerator WaitForReady(Action action)
    {
        while (!connectionToClient.isReady)
        {
            yield return null;
        }

        action();
    }
}
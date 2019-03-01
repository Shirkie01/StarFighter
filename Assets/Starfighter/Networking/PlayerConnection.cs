using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerConnection : NetworkBehaviour
{
    [SerializeField]
    private GameObject[] playerPrefabs;

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
    }

    [ClientRpc]
    private void RpcSpawnPlayerCallback()
    {
        if (hasAuthority)
        {
            player.GetComponent<Starfighter>().enabled = true;
        }
    }

    [Server]
    private void CmdSpawnPlayer()
    {
        if (!connectionToClient.isReady)
        {
            StartCoroutine(WaitForReady(() => CmdSpawnPlayer()));
            return;
        }

        if (playerPrefabs == null)
        {
            Debug.Log("Server: 'PlayerPrefabs' were not assigned");
        }

        if (player != null)
        {
            NetworkServer.Destroy(player);
        }

        player = Instantiate(playerPrefabs[Random.Range(0, playerPrefabs.Length - 1)]);
        player.transform.position = Random.insideUnitSphere * 500;
        player.GetComponent<Starfighter>().playerConnection = this;
        if (!NetworkServer.SpawnWithClientAuthority(player, connectionToClient))
        {
            Debug.Log("Server: Failed to spawn player");
        }

        RpcSpawnPlayerCallback();
    }

    private IEnumerator WaitForReady(System.Action action)
    {
        while (!connectionToClient.isReady)
        {
            yield return null;
        }

        action();
    }

    public IEnumerator Respawn()
    {
        Debug.Log("Respawn started...");
        yield return new WaitForSeconds(3);
        CmdSpawnPlayer();
    }
}
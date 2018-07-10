using Prototype.NetworkLobby;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkLobbyHook : LobbyHook
{
    /*
    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        // Original Unity Code

        LobbyPlayer lobby = lobbyPlayer.GetComponent<LobbyPlayer>();
        NetworkSpaceship spaceship = gamePlayer.GetComponent<NetworkSpaceship>();

        spaceship.name = lobby.name;
        spaceship.color = lobby.playerColor;
        spaceship.score = 0;
        spaceship.lifeCount = 3;
    }
    */

    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        LobbyPlayer lobby = lobbyPlayer.GetComponent<LobbyPlayer>();
        Starfighter starfighter = gamePlayer.GetComponent<Starfighter>();

        starfighter.name = lobby.name;
        starfighter.color = lobby.playerColor;

        starfighter.transform.position = Random.insideUnitSphere * 100;
        starfighter.transform.rotation = Random.rotation;
    }
}
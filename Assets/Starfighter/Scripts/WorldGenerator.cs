using UnityEngine;
using Mirror;

public class WorldGenerator : NetworkBehaviour
{
    [SerializeField]
    private int worldSize = 1024;

    [SerializeField]
    private GameObject[] asteroids;

    [SerializeField]
    private int numAsteroids = 100;

    [SerializeField]
    private int maxScale = 100;

    public override void OnStartServer()
    {
        for (int i = 0; i < numAsteroids; i++)
        {
            var asteroid = asteroids[Random.Range(0, asteroids.Length - 1)];
            GameObject go = Instantiate(asteroid, Random.insideUnitSphere * worldSize, Random.rotation, transform);
            go.transform.localScale = Vector3.one * Random.Range(1, maxScale);

            if (!NetworkManager.singleton.spawnPrefabs.Contains(go))
            {
                NetworkManager.singleton.spawnPrefabs.Add(go);
            }

            NetworkServer.Spawn(go);
        }
    }
}
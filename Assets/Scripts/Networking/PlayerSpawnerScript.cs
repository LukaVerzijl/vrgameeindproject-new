using Fusion;
using UnityEngine;
using Cinemachine;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    public GameObject PlayerPrefab;



    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {

            Runner.Spawn(PlayerPrefab, new Vector3(0, 1, 0), Quaternion.identity);
        }
    }
}
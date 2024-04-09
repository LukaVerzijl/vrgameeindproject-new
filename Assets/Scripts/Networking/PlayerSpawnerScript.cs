using Fusion;
using UnityEngine;
using Cinemachine;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    public GameObject VRRig;
    public GameObject PCRig;

    //Bools
    public bool isPCSelected;
    public bool isVRSelected;



    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            if(isVRSelected)
            {
                Runner.Spawn(VRRig, new Vector3(0, 1, 0), Quaternion.identity);
            } 
            else if(isPCSelected)
            {
                Runner.Spawn(PCRig, new Vector3(0, 1, 0), Quaternion.identity);
            }

        }
    }
}
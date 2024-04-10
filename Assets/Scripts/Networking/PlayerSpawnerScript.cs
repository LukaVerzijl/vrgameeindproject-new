using Fusion;
using UnityEngine;
using Cinemachine;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    public GameObject VRRig;
    public GameObject PCRig;

    //Bools
    public bool isPCSelected = false;
    public bool isVRSelected = false;
    bool NeedToUpdate = false;
    string tagToDisable;
    GameObject[] objectsWithTag;

    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            if(isVRSelected)
            {
                Runner.Spawn(VRRig, new Vector3(0, 1, 0), Quaternion.identity);

                tagToDisable = "PCCam";
            } 
            else if(isPCSelected)
            {
                Runner.Spawn(PCRig, new Vector3(0, 1, 0), Quaternion.identity);
                tagToDisable = "VRCam";
                print("pc is geselecteerd");
            }

        }
    }

       void Update()
    {
        if (!NeedToUpdate)
        {
            objectsWithTag = GameObject.FindGameObjectsWithTag(tagToDisable);
            if(objectsWithTag.Length > 0)
            {
                DisableObjects();
                NeedToUpdate = false;
            }

        }
    }

    void DisableObjects()
    {
        // Find all GameObjects with the specified tag


        print("Hij word disabled");
        // Disable each GameObject found
        foreach (GameObject obj in objectsWithTag)
        { 
            obj.SetActive(false);
            print("jooiiii");
            print(tagToDisable);
        }
    }
}
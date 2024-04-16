using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class rocketHandler : NetworkBehaviour
{
    [Header("prefabs")]
    public GameObject explosionParticleSystemPrefab;

    [Header("Collision detection")]
    public Transform checkForImpactPoint;
    public LayerMask collisionLayers;

    // timing
    TickTimer maxLiveDurationTickTimer = TickTimer.None;

    //Rocket info
    int rocketSpeed = 20;

    List<LagCompensatedHit> hits = new List<LagCompensatedHit>();

    //Fired by info
    PlayerRef firedByPlayerRef;
    string firedByPlayerName;
    NetworkObject firedNetworkObject;

    //Other components
    NetworkObject networkObject;

    public void Fire(PlayerRef firedByPlayerRef, NetworkObject firedNetworkObject, string firedByPlayerName)
    {
        this.firedByPlayerRef = firedByPlayerRef;
        this.firedByPlayerName = firedByPlayerName;
        this.firedNetworkObject = firedNetworkObject;

        networkObject = GetComponent<NetworkObject>();

        maxLiveDurationTickTimer = TickTimer.CreateFromSeconds(Runner, 10);
    }

    public override void FixedUpdateNetwork()
    {
        transform.position += transform.forward * Runner.DeltaTime * rocketSpeed;

        if (Object.HasInputAuthority)
        {
            if (maxLiveDurationTickTimer.Expired(Runner))
            {
                Runner.Despawn(networkObject);

                return;
            }

            //Check if the rocket hit anything
            int hitCount = Runner.LagCompensation.OverlapSphere(checkForImpactPoint.position, 0.5f, firedByPlayerRef, hits, collisionLayers, HitOptions.IncludePhysX);

            bool isValidHit = false;

            if (hitCount > 0)
                isValidHit = true;


            for (int i = 0; i < hitCount; i++)
            {
                if (hits[i].Hitbox != null)
                {
                    if (hits[i].Hitbox.Root.GetBehaviour<NetworkObject>() == firedNetworkObject)
                        isValidHit = false;
                }
            }

            if (isValidHit)
            {
                hitCount = Runner.LagCompensation.OverlapSphere(checkForImpactPoint.position, 4, firedByPlayerRef, hits, collisionLayers, HitOptions.None);

                //Deal famage to it
                for (int i = 0; i < hitCount; i++)
                {

                }

                Runner.Despawn(networkObject);
            }

        }
    }
    //When despawning the rocket object create some visuals
    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        //instatiate the visuals
    }

}

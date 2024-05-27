using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class RocketHandler : NetworkBehaviour
{
    [Header("Prefabs")]
    public GameObject explosionParticleSystemPrefab;

    [Header("Collision Detection")]
    public Transform checkForImpactPoint;
    public LayerMask collisionLayers;

    // Timing
    TickTimer maxLiveDurationTickTimer = TickTimer.None;

    // Rocket Info
    int rocketSpeed = 20;

    // Fired by Info
    PlayerRef firedByPlayerRef;
    string firedByPlayerName;
    NetworkObject firedNetworkObject;

    // Other Components
    NetworkObject networkObject;

    // Array for storing hits
    Collider[] hits = new Collider[10]; // Adjust the size as needed

    public void Fire(PlayerRef firedByPlayerRef, NetworkObject firedNetworkObject, string firedByPlayerName)
    {
        this.firedByPlayerRef = firedByPlayerRef;
        this.firedByPlayerName = firedByPlayerName;
        this.firedNetworkObject = firedNetworkObject;

        networkObject = GetComponent<NetworkObject>();

        if (networkObject == null)
        {
            Debug.LogError("NetworkObject component is missing on the rocket.");
        }

        maxLiveDurationTickTimer = TickTimer.CreateFromSeconds(Runner, 10);
    }

    public override void FixedUpdateNetwork()
    {
        transform.position += transform.forward * Runner.DeltaTime * rocketSpeed;

        if (Object.HasInputAuthority)
        {
            if (maxLiveDurationTickTimer.Expired(Runner))
            {
                if (networkObject != null)
                {
                    Runner.Despawn(networkObject);
                }
                else
                {
                    Debug.LogError("NetworkObject is null when trying to despawn.");
                }
                return;
            }

            // Check if the rocket hit anything using Physics.OverlapSphere
            int hitCount = Physics.OverlapSphereNonAlloc(checkForImpactPoint.position, 0.5f, hits, collisionLayers);

            bool isValidHit = false;

            if (hitCount > 0)
                isValidHit = true;

            for (int i = 0; i < hitCount; i++)
            {
                NetworkObject hitNetworkObject = hits[i].GetComponentInParent<NetworkObject>();
                if (hitNetworkObject != null && hitNetworkObject == firedNetworkObject)
                {
                    isValidHit = false;
                }
            }

            if (isValidHit)
            {
                hitCount = Physics.OverlapSphereNonAlloc(checkForImpactPoint.position, 4f, hits, collisionLayers);

                // Deal damage to the hit objects
                for (int i = 0; i < hitCount; i++)
                {
                    // Implement damage logic here
                }
                networkObject = GetComponent<NetworkObject>();
                if (networkObject != null)
                {
                    Debug.LogError("Hij heeft de bullet gedespawned");
                    Destroy(gameObject);
                    Runner.Despawn(networkObject);
                }
                else
                {
                    Debug.LogError("NetworkObject is null when trying to despawn after hitting something.");
                }
            }
        }
    }

    // When despawning the rocket object, create some visuals
    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        // Instantiate explosion effects
        //Instantiate(explosionParticleSystemPrefab, transform.position, Quaternion.identity);
    }
}

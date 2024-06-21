using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Fusion.Photon;
using Fusion.Addons.Physics;

[RequireComponent(typeof(NetworkRigidbody3D))]
public class NetworkedXRGrabInteractable : XRGrabInteractable
{
    private NetworkRigidbody3D networkRigidbody;

    protected override void Awake()
    {
        base.Awake();
        networkRigidbody = GetComponent<NetworkRigidbody3D>();
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        networkRigidbody.Rigidbody.isKinematic = true;
        // Additional logic for handling networked grab, if needed
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        networkRigidbody.Rigidbody.isKinematic = false;
        // Additional logic for handling networked release, if needed
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);

        if (isSelected && selectingInteractor != null)
        {
            switch (updatePhase)
            {
                case XRInteractionUpdateOrder.UpdatePhase.Fixed:
                    networkRigidbody.Rigidbody.MovePosition(selectingInteractor.transform.position);
                    networkRigidbody.Rigidbody.MoveRotation(selectingInteractor.transform.rotation);
                    break;
                case XRInteractionUpdateOrder.UpdatePhase.Dynamic:
                    // Handle dynamic updates if needed
                    break;
            }
        }
    }
}

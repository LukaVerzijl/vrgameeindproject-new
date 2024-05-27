using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonShooterController : NetworkBehaviour
{
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform debugTransform;
    [SerializeField] private NetworkObject pfBulletProjectile;
    [SerializeField] private GameObject pfBulletProjectile2;
    [SerializeField] private Transform spawnBulletPosition;

    public Camera playerCam;
    private Inventory inventoryScript;

    // Start is called before the first frame update
    void Awake()
    {
        playerCam = GameObject.Find("PlayerCam").GetComponent<Camera>();
        spawnBulletPosition = GameObject.Find("CombatLookAt").transform;
        inventoryScript = GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouseWorldPosition = Vector3.zero;
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = playerCam.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            debugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (inventoryScript.GetItemCount("Rocket") > 0)
            {
                Vector3 aimDir = (mouseWorldPosition - spawnBulletPosition.position).normalized;
                Runner.Spawn(pfBulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));

                inventoryScript.RemoveItem(new Item("Rocket", 1));
            }
            else
            {
                Debug.LogError("You don't have enough rockets");
            }
        }
    }
}

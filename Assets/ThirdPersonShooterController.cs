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
    Camera playerCam;

    // Start is called before the first frame update
    void Start()
    {
        debugTransform = GameObject.Find("debugTransform").transform;
        playerCam = GameObject.Find("PlayerCam").GetComponent<Camera>();

        spawnBulletPosition = GameObject.Find("CombatLookAt").transform;
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

        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector3 aimDir = (mouseWorldPosition - spawnBulletPosition.position).normalized;
            Instantiate(pfBulletProjectile2, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
            Runner.Spawn(pfBulletProjectile, Vector3.zero, Quaternion.identity, Runner.LocalPlayer);
            Debug.Log(Runner.Spawn(pfBulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up)));
        }
    }
}

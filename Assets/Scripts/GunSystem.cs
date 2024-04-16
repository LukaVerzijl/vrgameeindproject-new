using System;
using UnityEngine;

public class GunSystem : MonoBehaviour
{
    // Gun stats
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;

    //bools
    bool shooting, readyToShoot, reloading;

    //Reference
    public Camera tpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;



    private void Awake()
    {
        //Instatiate objects
        tpsCam = GameObject.Find("PlayerCam").GetComponent<Camera>();
        attackPoint = GameObject.Find("CombatLookAt").transform;

        bulletsLeft = magazineSize;
        readyToShoot = true;
    }
    private void Update()
    {
        MyInput();
    }

    private void MyInput()
    {
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();

        if(readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTap;
            Shoot();
        }  
    }

    private void Shoot()
    {
        readyToShoot = false;

        //spread
        float x = UnityEngine.Random.Range(-spread, spread);
        float y = UnityEngine.Random.Range(-spread, spread);

        //Calculate Direction with normal spread
        Vector3 direction = tpsCam.transform.forward + new Vector3(x, y, 0);


        //RayCast

        //Hier aan tweaken want de camera staat niet goed
        if (Physics.Raycast(tpsCam.transform.position, direction, out rayHit, range, whatIsEnemy))
        {
            Debug.Log(rayHit.collider.name);
            Debug.Log("Hit");
            if(rayHit.collider.CompareTag("Enemy"))
            {
                //rayHit.collider.GetComponent<ShootingAi>().TakeDamage(damage);
            }
        }

        bulletsLeft--;
        bulletsShot--;
        Invoke("ResetShot", timeBetweenShooting);

        if(bulletsShot > 0 && bulletsLeft > 0)
        {
            Invoke("Shoot", timeBetweenShots);
        }
        
    }

    private void ResetShot()
    {
        readyToShoot = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GunData gunData;
    [SerializeField] private Transform muzzle;

    [SerializeField] private GameObject projectilePrefab; // The projectile prefab
    [SerializeField] private Transform shootPoint; // The point from which the projectile will be fired
    [SerializeField] private float projectileSpeed = 20f; // Speed of the projectile

    float timeSinceLastShot;

    private void Start()
    {
        PlayerShoot.shootInput += Shoot;
        PlayerShoot.reloadInput += StartReload;

        gunData.reloading = false;

        // Initialize ammo
        ResetAmmo();
    }

    private void ResetAmmo()
    {
        // Initialize currentAmmo from gunData
        gunData.currentAmmo = gunData.magSize;
    }

    public void StartReload()
    {
        Debug.Log("Reload method called.");
        if (!gunData.reloading)
        {
            Debug.Log("Starting reload coroutine");
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {
        Debug.Log("Reloading");
        gunData.reloading = true;
        yield return new WaitForSeconds(gunData.reloadTime);
        gunData.currentAmmo = gunData.magSize;
        gunData.reloading = false;
    }

    private bool CanShoot() => !gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f);

    public void Shoot()
    {
        if (gunData.currentAmmo > 0)
        {
            if (CanShoot())
            {
                if (Physics.Raycast(muzzle.position, transform.forward, out RaycastHit hitInfo, gunData.maxDistance))
                {
                    Debug.Log(hitInfo.transform.name);
                }
            }

            gunData.currentAmmo--;
            timeSinceLastShot = 0;
            OnGunShot();

        }
        else
        {
            Debug.Log("Out of ammo!");
        }
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        Debug.DrawRay(muzzle.position, muzzle.forward);
    }

    private void OnGunShot()
    {
        ThrowProjectile();
    }

    private void ThrowProjectile()
    {
        if (projectilePrefab && shootPoint)
        {
            if (gunData.currentAmmo > 0) // Ensure there is ammo before firing
            {
                // Instantiate the projectile at the shoot point's position and rotation
                GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);

                // Get the Rigidbody component of the projectile
                Rigidbody rb = projectile.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    // Set the velocity of the projectile
                    rb.velocity = shootPoint.forward * projectileSpeed;

                    // Optionally, adjust the projectile's lifespan if needed
                    Destroy(projectile, 5f); // Destroy the projectile after 5 seconds to clean up
                }
                

            }
        }
    }
}

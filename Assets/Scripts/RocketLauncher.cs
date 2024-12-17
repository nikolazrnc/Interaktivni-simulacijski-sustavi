using UnityEngine;

public class RocketLauncher : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform attackPoint; 
    public float bulletSpeed = 20f; 
    public int magazineSize = 5;
    public float reloadTime = 2f;

    private int bulletsLeft;
    private bool reloading;

    private void Start()
    {
        bulletsLeft = magazineSize;
    }

    private void Update()
    {
    
        if (Input.GetKeyDown(KeyCode.Mouse0) && bulletsLeft > 0 && !reloading)
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading)
        {
            Reload();
        }
    }

    private void Shoot()
    {
        bulletsLeft--;

        GameObject bullet = Instantiate(bulletPrefab, attackPoint.position, attackPoint.rotation);

        bullet.transform.forward = attackPoint.right;

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.freezeRotation = true; 

        rb.linearVelocity = attackPoint.forward * bulletSpeed;
    }

    private void Reload()
    {
        reloading = true;
        Invoke(nameof(FinishReload), reloadTime);
    }

    private void FinishReload()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}

using UnityEngine;

public class TankShooting : MonoBehaviour
{
    public GameObject rocketPrefab;
    public Transform rocketSpawnPoint;
    public float rocketSpeed = 2f;
    public float shootInterval = 5f;
    public GameObject firePrefab;

    public AudioClip tankShootSound; 
    private AudioSource audioSource;

    public float tankShootVolume = 3f;
    private bool canShoot = true;

    private void Start()
    {
        InvokeRepeating(nameof(ShootRocket), 0f, shootInterval);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        audioSource.volume=tankShootVolume;
    }

    private void ShootRocket()
    {
        if (!canShoot || rocketPrefab == null || rocketSpawnPoint == null) return;

        GameObject rocket = Instantiate(rocketPrefab, rocketSpawnPoint.position, rocketSpawnPoint.rotation);

        Rigidbody rb = rocket.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(rocket.transform.forward * rocketSpeed, ForceMode.VelocityChange);
        }

        PlaySound(tankShootSound);
    }

    public void DisableTank()
    {
        if (canShoot)
        {
            canShoot = false;
            CancelInvoke(nameof(ShootRocket));

            if (firePrefab != null)
            {
                Instantiate(firePrefab, transform.position, transform.rotation);
            }
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}

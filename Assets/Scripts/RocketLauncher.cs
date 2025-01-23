using UnityEngine;

public class RocketLauncher : MonoBehaviour
{
    public GameObject rocketPrefab;
    public Transform attackPoint;
    public AudioClip explosionSound;    
    public AudioClip playerShootSound;
    public GameObject explosionPrefab;
    public float rocketSpeed = 20f;
    public int magazineSize = 5;
    public float reloadTime = 2f;

    private int rocketsLeft;
    private bool reloading;
    private bool rocketActive; 
    private GameObject currentRocket;
    private GameObject currentExplosion; 

    public Camera playerCamera;  
    public Camera rocketCamera;  
    public Camera scopeCamera; 

    private bool isAiming = false;

    private AudioSource audioSource;

    private void Start()
    {
        rocketsLeft = magazineSize;
        playerCamera.enabled = true;
        rocketCamera.enabled = false;
        scopeCamera.enabled = false;
        
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }  
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && rocketsLeft > 0 && !reloading && !rocketActive)
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R) && rocketsLeft < magazineSize && !reloading)
        {
            Reload();
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            Aim();
        }
        else if (isAiming)
        {
            StopAiming();
        }
    }

    private void Shoot()
    {
        if (rocketPrefab == null || attackPoint == null)
        {
            return;
        }
        rocketsLeft--;
        rocketActive = true;
         
        currentRocket = Instantiate(rocketPrefab, attackPoint.position, attackPoint.rotation);
        currentRocket.transform.forward = attackPoint.right;

        Rigidbody rb = currentRocket.GetComponent<Rigidbody>();
        if (rb == null)
        {
            return;
        }

        rb.freezeRotation = false;
        rb.linearVelocity = attackPoint.forward * rocketSpeed;

        RocketController rocketController = currentRocket.AddComponent<RocketController>();
        rocketController.OnCrash += ResetCameraToPlayer;
        rocketController.rocketCamera = rocketCamera;
        rocketController.explosionPrefab = explosionPrefab;
        
        rocketController.explosionSound = explosionSound;
        Vector3 initialCameraPosition = currentRocket.transform.position - currentRocket.transform.forward * 4f + Vector3.up * 10f;
        rocketCamera.transform.position = initialCameraPosition;

        Vector3 lookTarget = currentRocket.transform.position;
        rocketCamera.transform.rotation = Quaternion.LookRotation(lookTarget - rocketCamera.transform.position);

        playerCamera.enabled = false;
        rocketCamera.enabled = true;
        ToggleAudioListener(rocketCamera);

        PlaySound(playerShootSound);
    }

    private void Reload()
    {
        reloading = true;
        Invoke(nameof(FinishReload), reloadTime);
    }

    private void FinishReload()
    {
        rocketsLeft = magazineSize;
        reloading = false;
    }

    private void ResetCameraToPlayer()
    {  
        if (currentExplosion != null)
        {
            Destroy(currentExplosion, 3f);
        }
        playerCamera.enabled = true;
        rocketCamera.enabled = false;

        rocketActive = false;

        playerCamera.enabled = true;
        rocketCamera.enabled = false;
        ToggleAudioListener(playerCamera);

        rocketActive = false;
    }

    private void Aim()
    {
        isAiming = true;
        scopeCamera.enabled = true;
        playerCamera.enabled = false;  
        ToggleAudioListener(scopeCamera);
    }

    private void StopAiming()
    {
        isAiming = false;
        playerCamera.enabled = true;
        scopeCamera.enabled = false; 
        ToggleAudioListener(playerCamera);
    }

    private void ToggleAudioListener(Camera activeCamera)
    {
        foreach (var listener in FindObjectsOfType<AudioListener>())
        {
            listener.enabled = false;
        }
        AudioListener activeListener = activeCamera.GetComponent<AudioListener>();
        if (activeListener != null)
        {
            activeListener.enabled = true;
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
    public void SetExplosion(GameObject explosion)
    {
        currentExplosion = explosion;
    }
    
}

using UnityEngine;

public class RocketLauncher : MonoBehaviour{
    public GameObject rocketPrefab;
    public Transform attackPoint;

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

    private void Start(){
        rocketsLeft = magazineSize;

        playerCamera.enabled = true;
        rocketCamera.enabled = false;
    }

    private void Update(){
        if (Input.GetKeyDown(KeyCode.Mouse0) && rocketsLeft > 0 && !reloading && !rocketActive){
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R) && rocketsLeft < magazineSize && !reloading){
            Reload();
        }
    }

    private void Shoot(){
        if (rocketPrefab == null || attackPoint == null){
            return;
        }
        rocketsLeft--;
        rocketActive = true;
         
        currentRocket = Instantiate(rocketPrefab, attackPoint.position, attackPoint.rotation);
        currentRocket.transform.forward = attackPoint.right;

        Rigidbody rb = currentRocket.GetComponent<Rigidbody>();
        if (rb == null){
            return;
        }

        rb.freezeRotation = false;
        rb.linearVelocity = attackPoint.forward * rocketSpeed;

        RocketController rocketController = currentRocket.AddComponent<RocketController>();
        rocketController.OnCrash += ResetCameraToPlayer;
        rocketController.rocketCamera = rocketCamera;
        rocketController.explosionPrefab = explosionPrefab;

        Vector3 initialCameraPosition = currentRocket.transform.position - currentRocket.transform.forward * 4f + Vector3.up * 10f;
        rocketCamera.transform.position = initialCameraPosition;

        Vector3 lookTarget = currentRocket.transform.position;
        rocketCamera.transform.rotation = Quaternion.LookRotation(lookTarget - rocketCamera.transform.position);

        playerCamera.enabled = false;
        rocketCamera.enabled = true;
    }

    private void Reload(){
        reloading = true;
        Invoke(nameof(FinishReload), reloadTime);
    }

    private void FinishReload(){
        rocketsLeft = magazineSize;
        reloading = false;
    }

    private void ResetCameraToPlayer(){  
        if (currentExplosion != null){
            Destroy(currentExplosion, 3f);
        }
        playerCamera.enabled = true;
        rocketCamera.enabled = false;

        rocketActive = false;
    }

    public void SetExplosion(GameObject explosion){
        currentExplosion = explosion; 
    }
}

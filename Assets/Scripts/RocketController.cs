using UnityEngine;
using System;

public class RocketController : MonoBehaviour
{
    public event Action OnCrash;

    public Camera rocketCamera;  

    private Rigidbody rb;

    public GameObject explosionPrefab;  

    public float thrustPower = 10f;       
    public float maxThrust = 50f;        
    public float rotationSpeed = 5f;     
    public float lift = 20f;            
    public float rotationDamping = 2f;   
    public float maxSpeed = 50f;         

    private float throttle;              

    private void Start(){
        rb = GetComponent<Rigidbody>();
    }

    private void Update(){
        if (rocketCamera != null){
            Vector3 cameraPosition = transform.position - transform.forward * 4f + Vector3.up * 10f;  
            rocketCamera.transform.position = Vector3.Lerp(rocketCamera.transform.position, cameraPosition, Time.deltaTime * 5f);
            
            Vector3 lookTarget = transform.position;  
            Quaternion cameraRotation = Quaternion.LookRotation(lookTarget - rocketCamera.transform.position);
            rocketCamera.transform.rotation = Quaternion.Lerp(rocketCamera.transform.rotation, cameraRotation, Time.deltaTime * 5f);

        }
        HandleMovement();
        HandleThrottle();
    }

    private void HandleMovement(){
        if (Input.GetKey(KeyCode.W)){
            rb.AddForce(transform.forward * thrustPower * throttle);
        }
        if (Input.GetKey(KeyCode.S)){
            rb.AddTorque(-transform.right * rotationSpeed);
        }
        if (Input.GetKey(KeyCode.W)){
            rb.AddTorque(transform.right * rotationSpeed);
        }
        if (Input.GetKey(KeyCode.A)){
            rb.AddTorque(-transform.up * rotationSpeed);
        }
        if (Input.GetKey(KeyCode.D)){
            rb.AddTorque(transform.up * rotationSpeed);
        }
        if (Input.GetKey(KeyCode.Q)){
            rb.AddTorque(transform.forward * rotationSpeed);
        }
        if (Input.GetKey(KeyCode.E)){
            rb.AddTorque(-transform.forward * rotationSpeed);
        }

        rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, Vector3.zero, rotationDamping * Time.deltaTime);
    }

    private void HandleThrottle(){
        if (Input.GetKey(KeyCode.Space)){
            throttle += 0.01f; 
        }
        else if (Input.GetKey(KeyCode.LeftControl)){
            throttle -= 0.01f;  
        }

        throttle = Mathf.Clamp(throttle, 0f, 1f);
    }

    private void FixedUpdate(){
        Vector3 liftForce = transform.up * lift * Mathf.Clamp(rb.linearVelocity.magnitude, 0f, maxThrust);
        rb.AddForce(liftForce);

        rb.AddForce(transform.forward * maxThrust * throttle);

        rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, maxSpeed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Terrain") || collision.gameObject.CompareTag("Tank")){
            if (explosionPrefab != null){
                GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);

                RocketLauncher rocketLauncher = FindObjectOfType<RocketLauncher>();
                if (rocketLauncher != null){
                    rocketLauncher.SetExplosion(explosion);  
                }
            }

            OnCrash?.Invoke();
            Destroy(gameObject);
        }
    }

}
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public GameObject explosionPrefab; 
    public AudioClip explosionSound;
    public float destroyDelay = 2f; 

    public float explosionVolume = 5f;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(explosion, destroyDelay);

            AudioSource audioSource = explosion.AddComponent<AudioSource>();
            audioSource.clip = explosionSound;
            audioSource.volume = explosionVolume; 
            audioSource.Play();

            Destroy(gameObject);
        }
    }
}

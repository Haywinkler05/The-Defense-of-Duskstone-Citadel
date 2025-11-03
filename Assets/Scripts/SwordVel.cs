using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordVel : MonoBehaviour
{

    public GameObject sword;
    public float velocity = 0f;
    private Vector3 previousPosition = new Vector3(0.0f, 0.0f, 0.0f);

    public ParticleSystem particleEffect;

    public AudioClip collisionSound;
    private AudioSource audioSource;


 
    void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Sword Collided");
        // play sound and particle on enemy collision
        if(collision.gameObject.CompareTag("Enemy")){
            Debug.Log("Sword Collided with enemy");
            audioSource.Play();
/* 
            if (particleEffect != null)
            {
                // play particle
                ParticleSystem part = Instantiate(particleEffect, sword.transform.position, sword.transform.rotation);
                Destroy(part, 0.05f);
            }
             */
        }
        else{
            // play different sound on other collision(ground, wall, etc.)
            // audioSource2.Play();
        }
    }
 
    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = collisionSound;
        
    }

    // Update is called once per frame
    void Update()
    {
        // Find velocity by getting magnitude of current and previous position
        // Debug.Log("PrevPos =" + previousPosition);
        Vector3 currentPosition = sword.transform.position;
        if (previousPosition != Vector3.zero)
        {
            velocity = (currentPosition - previousPosition).magnitude;
            // Debug.Log("Sword Velocity =" + velocity);
        }
        previousPosition = currentPosition;
        // Debug.Log("CurrentPos =" + currentPosition);


        // on collison add particles
        
    }
}

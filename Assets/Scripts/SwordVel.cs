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


/* 
    void OnCollisionEnter(Collision col)
    {
        // play particle on contact.
        if (particleEffect != null){
            // sets particle pos to collision point
            particleEffect.transform.position = collision.contact[0].point;
            particleEffect.Play();
        }


        // play sound on enemy collision.
        if(collision.gameObject.CompareTag("Enemy")){
            audioSource.Play();
        }
        else{
            // play different sound on other collision(ground, wall, etc.)
            // audioSource2.Play();
        }
    }
 */
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

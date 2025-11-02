using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageScript : MonoBehaviour
{


    public float health = 100f; // health of skeleton
    public float attack = 15f; // base damage of sword
    public GameObject skeleton; // reference Skeleton


    private void OnTriggerEnter(Collider other)
    {
        // get component to check if sword
        // Debug.Log("Enter");
        if (!other.CompareTag("Sword")) return;
        SwordVel swordVel = other.GetComponent<SwordVel>();

        // velocity
        float velocity = swordVel.velocity;
        Debug.Log("Velocity  = " + velocity);

        //damage skeleton
        float dmg = attack;
        if (velocity > .2f)
        {
            dmg = velocity * 10f * 2.5f + attack;
        }
        Debug.Log("Damage = " + dmg);

        Damage(dmg);

    }

    private void Damage(float dmg)
    {
        // do damage to skeletons health and if health <= 0 then call death.
        // if possible do velocity based dmg
        health = health - dmg;

        if (health <= 0f)
        {
            Debug.Log("Death");
            Death();
        }
        Debug.Log("Health = " + health);
        
    }
    
    private void Death()
    {
        // make skeleton do death animation and then after a few seconds remove the entity
        // animation just crumbles to bown pile,
        // or if we have time, bones just react to sword and scatter based on collision.



        Debug.Log("Deathwwww");
        Destroy(skeleton);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //
        
    }
}

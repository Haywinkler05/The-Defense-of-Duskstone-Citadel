using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordVel : MonoBehaviour
{

    public GameObject sword;
    public float velocity = 0f;
    private Vector3 previousPosition= new Vector3(0.0f, 0.0f, 0.0f);
    // Start is called before the first frame update
    void Start()
    {
        
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

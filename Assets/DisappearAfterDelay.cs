using UnityEngine;

public class DisappearAfterDelay : MonoBehaviour
{
    [Header("Timing")]
    public float delayBeforeMove = 3f;       // seconds before starting to move
    public float moveDuration = 2f;          // how long the object moves before disappearing
    public float destroyDelay = 0.5f;        // how long to wait before destroying after movement

    [Header("Movement")]
    public Vector3 moveDirection = new Vector3(0, 0, -2f); // direction and distance to move
    public bool destroyAfter = true;        // destroy object or just disable

    private Vector3 startPosition;
    private Vector3 endPosition;
    private float moveStartTime;
    private bool moving = false;

    void Start()
    {
        startPosition = transform.position;
        endPosition = startPosition + moveDirection;
        Invoke(nameof(StartMoving), delayBeforeMove);
    }

    void StartMoving()
    {
        moving = true;
        moveStartTime = Time.time;
    }

    void Update()
    {
        if (moving)
        {
            float t = (Time.time - moveStartTime) / moveDuration;
            transform.position = Vector3.Lerp(startPosition, endPosition, t);

            if (t >= 1f)
            {
                moving = false;

                if (destroyAfter)
                    Destroy(gameObject, destroyDelay);
                else
                    gameObject.SetActive(false);
            }
        }
    }
}

using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    public bool positionLocked = false;
    private Vector2 movement; 
    private Rigidbody2D rb;

    void Start()
    {
        // Find the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!positionLocked)
        {
            // Get input 
            movement.x = Input.GetAxis("Horizontal");
            movement.y = Input.GetAxis("Vertical");
        }
    }

    void FixedUpdate()
    {
        if (!positionLocked)
        {
            // Move player 
            Vector2 newPosition = rb.position + movement.normalized * speed * Time.fixedDeltaTime;
            rb.MovePosition(newPosition);
        }
    }
}

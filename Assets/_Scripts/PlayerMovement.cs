using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    public bool positionLocked = false;
    private Vector2 movement; 
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Find Rigidbody2D component
    }

    void Update() // Get player input
    {
        if (!positionLocked)
        {
            movement.x = Input.GetAxis("Horizontal");
            movement.y = Input.GetAxis("Vertical");
        }
    }

    void FixedUpdate() // Move player
    {
        if (!positionLocked)
        {
            Vector2 newPosition = rb.position + movement.normalized * speed * Time.fixedDeltaTime;
            rb.MovePosition(newPosition);
        }
    }
}

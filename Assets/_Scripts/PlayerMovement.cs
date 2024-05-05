using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    private float speedX, speedY; 
    private Rigidbody2D rb;

    void Start()
    {
        // Find the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get input 
        speedX = Input.GetAxis("Horizontal") * speed;
        speedY = Input.GetAxis("Vertical") * speed; 

        // Move player 
        Vector2 newPosition = rb.position + new Vector2(speedX, speedY) * Time.deltaTime;
        rb.MovePosition(newPosition);
    }
}

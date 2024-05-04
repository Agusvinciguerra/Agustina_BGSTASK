using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    private float speedX, speedY;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        speedX = Input.GetAxis("Horizontal") * speed;
        speedY = Input.GetAxis("Vertical") * speed; 

        rb.velocity = new Vector2(speedX, speedY);
    }
}

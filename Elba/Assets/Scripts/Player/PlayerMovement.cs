using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Capturar input (WASD)
        movement.x = Input.GetAxisRaw("Horizontal"); // A (-1) / D (1)
        movement.y = Input.GetAxisRaw("Vertical");   // S (-1) / W (1)

        movement = movement.normalized;
    }

    void FixedUpdate()
    {
        // Aplicar movimiento físico
        rb.linearVelocity = movement * speed;
    }
}
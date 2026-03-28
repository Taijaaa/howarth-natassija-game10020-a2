using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;
    private PlayerSponge playerSponge;
    private PlayerBattery playerBattery;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerSponge = GetComponent<PlayerSponge>();
        playerBattery = GetComponent<PlayerBattery>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement = movement.normalized;

        if (movement.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
            playerSponge?.SetFacingDirection(Vector2.left);
            playerBattery?.SetFacingDirection(Vector2.left);
        }
        else if (movement.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, -90);
            playerSponge?.SetFacingDirection(Vector2.right);
            playerBattery?.SetFacingDirection(Vector2.right);
        }
        else if (movement.y > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            playerSponge?.SetFacingDirection(Vector2.up);
            playerBattery?.SetFacingDirection(Vector2.up);
        }
        else if (movement.y < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 180);
            playerSponge?.SetFacingDirection(Vector2.down);
            playerBattery?.SetFacingDirection(Vector2.down);
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movement * moveSpeed;
    }
}
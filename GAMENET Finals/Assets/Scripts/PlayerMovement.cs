using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;

    private Rigidbody2D rb;

    Animator animator;

    private Vector2 moveVelocity;

    public float r;
    public float g;
    public float b;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.gameObject.transform.GetChild(0).GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
    }

    private void Movement()
    {
        // Horizontal and Vertical Movement
        float xMovement = Input.GetAxisRaw("Horizontal");
        float yMovement = Input.GetAxisRaw("Vertical");

        Vector2 move = new Vector2(xMovement, yMovement);

        moveVelocity = move.normalized * speed;

        // Player Animation
        if (xMovement == 0 && yMovement == 0)
        {
            animator.SetBool("isRunning", false);
        }
        else
        {
            animator.SetBool("isRunning", true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Tile")
        {
            collision.GetComponent<SpriteRenderer>().color = new Color(r, g, b);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerMovement : MonoBehaviourPunCallbacks
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
        animator = this.gameObject.transform.GetComponent<Animator>();
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

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Tile")
        {
            if (collider.GetComponent<SpriteRenderer>().color == new Color(r, g, b))
            {
                Debug.Log("Color is the same");
                return;
            }
            else
            {
                collider.GetComponent<SpriteRenderer>().color = new Color(r, g, b);
                Debug.Log("New Color");
            }
        }
    }

    public int GetActorNumber()
    {
        return PhotonNetwork.LocalPlayer.ActorNumber;
    }
}

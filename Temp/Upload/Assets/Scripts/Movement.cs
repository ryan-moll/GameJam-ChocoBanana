using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Collision coll;
    public Animator myAnimator;

    [HideInInspector]
    public Rigidbody2D rb;

    [Space]
    [Header("Stats")]
    public float speed = 10;
    public float jumpForce = 50;

    [Space]
    [Header("Booleans")]
    public bool canMove;

    [Space]
    private bool groundTouch;
    private Rigidbody2D myRigidBody;
    private bool facingRight;

    public int side = 1;

    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<Collision>();
        rb = GetComponent<Rigidbody2D>();
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        facingRight = true;
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector2 dir = new Vector2(x, y);

        Walk(dir);
        myAnimator.SetFloat("speed", Mathf.Abs(x));

        if (Input.GetButtonDown("Jump") && coll.onGround)
        {
            myAnimator.SetBool("jump", true);
            StartCoroutine(Jump(Vector2.up));
        }

        if (coll.onGround && !groundTouch)
        {
            GroundTouch();
            groundTouch = true;
        }

        if (!coll.onGround && groundTouch)
        {
            groundTouch = false;
            myAnimator.SetBool("jump", false);
        }
    }

    private void FixedUpdate() {
        Flip(Input.GetAxis("Horizontal"));
    }

    private void Walk(Vector2 dir)
    {
        if (!canMove)
            return;

        rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);
    }

    void GroundTouch()
    {
    }

    private void Flip(float horizontal)
    {
        if (horizontal < 0 && facingRight || horizontal > 0 && !facingRight)
        {
            facingRight = !facingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    IEnumerator Jump(Vector2 dir)
    {
        yield return new WaitForSeconds(0.52f);
        if (groundTouch)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.velocity += dir * jumpForce;
        }
    }
}

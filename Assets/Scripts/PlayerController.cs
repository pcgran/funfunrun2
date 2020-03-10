using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed;
    public float jumpVelocity;
    public LayerMask platformsLayerMask;
    public float jumpTime;
    public Camera camera;
    public bool isBackwards;
    public bool disabledControls;

    public float c;
    public float s;

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider2D;
    private Animator animator;

    private float jumpTimeCounter;
    private bool isWalkingRight;
    private bool canJump;
    private bool hasStoppedPressingJump; // esta variable es para evitar el rebote en los saltos

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        jumpTimeCounter = jumpTime;
        isWalkingRight = false;
        isBackwards = false;
        hasStoppedPressingJump = true;
    }

    void Update()
    {
        if (IsGrounded() && hasStoppedPressingJump)
        {
            canJump = true;
            jumpTimeCounter = jumpTime;
        }

        if (IsGrounded())
        {
            animator.SetBool("isJumping", false);
        }

        HandleMovement();
        LongJump();
    }

    private void LongJump()
    {
        if (Input.GetKey(KeyCode.W) && jumpTimeCounter > 0 && canJump)
        {
            //rb.velocity = Vector2.up * jumpVelocity;

            rb.AddForce(Vector2.up * jumpVelocity, ForceMode2D.Impulse);
            jumpTimeCounter = jumpTimeCounter - Time.deltaTime;
            hasStoppedPressingJump = false;
            animator.SetBool("isJumping", true);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            canJump = false; //si dejo de presionar espacio ya no puedo saltar más hasta que vuelva al suelo
            hasStoppedPressingJump = true;
        }
    }

    private void HandleMovement()
    {
        if (disabledControls) return;
        if (Input.GetKey(KeyCode.A))
        {
            if (isWalkingRight)
            {
                rb.transform.rotation *= Quaternion.Euler(0, 180f, 0);
                isWalkingRight = false;
            }

            rb.transform.position = rb.transform.position + Vector3.left * speed * Time.deltaTime;
            animator.SetBool("isWalking", true);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (!isWalkingRight)
            {
                rb.transform.rotation *= Quaternion.Euler(0, 180f, 0);
                isWalkingRight = true;
            }
            rb.transform.position += Vector3.right * speed * Time.deltaTime;
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    private bool IsGrounded()
    {
        Vector2 origin = boxCollider2D.bounds.center + new Vector3(0f, c, 0f);
        Vector2 size = boxCollider2D.bounds.size + new Vector3(-0.1f, s, 0f); ;

        RaycastHit2D raycast = Physics2D.BoxCast(origin, size, 0f, Vector2.down, 0.1f, platformsLayerMask);
        BoxCast(origin, size, 0f, Vector2.down, 0.1f, platformsLayerMask);
        return raycast.collider != null;
    }

    static public RaycastHit2D BoxCast(Vector2 origen, Vector2 size, float angle, Vector2 direction, float distance, int mask)
    {
        RaycastHit2D hit = Physics2D.BoxCast(origen, size, angle, direction, distance, mask);

        //Setting up the points to draw the cast
        Vector2 p1, p2, p3, p4, p5, p6, p7, p8;
        float w = size.x * 0.5f;
        float h = size.y * 0.5f;
        p1 = new Vector2(-w, h);
        p2 = new Vector2(w, h);
        p3 = new Vector2(w, -h);
        p4 = new Vector2(-w, -h);

        Quaternion q = Quaternion.AngleAxis(angle, new Vector3(0, 0, 1));
        p1 = q * p1;
        p2 = q * p2;
        p3 = q * p3;
        p4 = q * p4;

        p1 += origen;
        p2 += origen;
        p3 += origen;
        p4 += origen;

        Vector2 realDistance = direction.normalized * distance;
        p5 = p1 + realDistance;
        p6 = p2 + realDistance;
        p7 = p3 + realDistance;
        p8 = p4 + realDistance;


        //Drawing the cast
        Color castColor = hit ? Color.red : Color.green;
        Debug.DrawLine(p1, p2, castColor);
        Debug.DrawLine(p2, p3, castColor);
        Debug.DrawLine(p3, p4, castColor);
        Debug.DrawLine(p4, p1, castColor);

        Debug.DrawLine(p5, p6, castColor);
        Debug.DrawLine(p6, p7, castColor);
        Debug.DrawLine(p7, p8, castColor);
        Debug.DrawLine(p8, p5, castColor);

        Debug.DrawLine(p1, p5, Color.grey);
        Debug.DrawLine(p2, p6, Color.grey);
        Debug.DrawLine(p3, p7, Color.grey);
        Debug.DrawLine(p4, p8, Color.grey);
        if (hit)
        {
            Debug.DrawLine(hit.point, hit.point + hit.normal.normalized * 0.2f, Color.yellow);
        }

        return hit;
    }

}

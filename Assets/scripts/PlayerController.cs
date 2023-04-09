using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    enum State
    {
        STATE_IDLE,
        STATE_RUN,
        STATE_JUMP,
        STATE_ARROW,
        STATE_DEATH
    };

    State state = State.STATE_IDLE;
    private Rigidbody2D rb2d;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    private bool isGrounded;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayer;


    private Animator animator;
    private string currentState;
    const string idle = "PlayerIdle";
    const string run = "PlayerRun";
    const string jump = "PlayerJump";
    const string death = "PlayerDeath";
    const string idleArrow = "PlayerIdleArrow";
    bool facingRight = true;

    //Checkpoint
    public static Vector2 lastCheckpoint;
    public Camera mainCamera;
    Transform t;
    Vector3 cameraPos;
    int itemCount;
    void Awake()
    {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
    }
    void reborn()
    {
        transform.position = lastCheckpoint;
        ChangeAnimationState(idle);
        state = State.STATE_IDLE;
    }
    void Start()
    {
            t = transform;
            if (mainCamera)
        {
            cameraPos = mainCamera.transform.position;
        }
    }
    void Update()
    {
        switch (state)
        {
            case State.STATE_IDLE:
                if ((Input.GetAxisRaw("Horizontal") == 0))
                    ChangeAnimationState(idle);
                Idle();
                break;
            case State.STATE_RUN:
                if (isGrounded)
                    ChangeAnimationState(run);
                else
                    ChangeAnimationState(jump);
                Run();
                break;
            case State.STATE_JUMP:
                ChangeAnimationState(jump);
                Jump();
                break;
            case State.STATE_DEATH:
                ChangeAnimationState(death);
                break;
        }
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        checkScale();
        if (mainCamera)
        {
            mainCamera.transform.position = new Vector3(t.position.x, cameraPos.y, cameraPos.z);
        }
    }
    private void checkScale()
    {
        if ((Input.GetAxisRaw("Horizontal") == -1 && facingRight) || (!facingRight && Input.GetAxisRaw("Horizontal") == 1))
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            facingRight = !facingRight;
        }

    }
    public void deathState()
    {
        state = State.STATE_DEATH;

    }
    
    private void Run()
    {

        rb2d.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * speed, rb2d.velocity.y);


        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
            state = State.STATE_JUMP;
        }
        else if (Input.GetAxisRaw("Horizontal") == 0 && isGrounded && rb2d.velocity.y == 0)
            state = State.STATE_IDLE;

    }
    private void Jump()
    {

        if (Input.GetAxisRaw("Horizontal") != 0) //input al�yorsa
        {
            state = State.STATE_RUN;
        }
        else if (rb2d.velocity.y == 0)
        {
            state = State.STATE_IDLE;
        }

    }
    private void Idle()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
            state = State.STATE_JUMP;
        }
        else if (Input.GetAxisRaw("Horizontal") != 0)
            state = State.STATE_RUN;
    }

    void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    private void OnTriggerEnter2D(Collider2D trig)
    {
        if (trig.CompareTag("Item"))
        {
            Destroy(trig.gameObject);
            itemCount++;
            Debug.Log(itemCount);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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
    float coinspeed = 0.25f;
    [SerializeField] private float jumpForce;
    private bool isGrounded;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] public TextMeshProUGUI _text;
    private Animator animator;
    private string currentState;
    const string idle = "Player";
    const string run = "PlayerRun";
    const string jump = "PlayerJump";
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
        Debug.Log(state);
        state = State.STATE_IDLE;
        Debug.Log(state);
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
                reborn();
                break;
        }
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        checkScale();
        if (mainCamera)
        {
            mainCamera.transform.position = new Vector3(t.position.x, (t.position.y+1), cameraPos.z);
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
            scoreUpdate();
        }
        else if (trig.CompareTag("end"))
            EndScene();
        else if (trig.CompareTag("sea"))
        {
            Debug.Log("dead");
            deathState();
        }
          
    }
    void scoreUpdate()
    {
        speed += coinspeed;
        itemCount++;
        Debug.Log(itemCount.ToString());
        _text.text = "Coins: " + itemCount.ToString();
    }
    public void EndScene()
    {
        SceneManager.LoadScene("EndScene");
    }
}

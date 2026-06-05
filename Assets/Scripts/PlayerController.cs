using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;



public class PlayerController : MonoBehaviour
{
    public enum PlayerState
    {
        Idle,
        Run,
        Jump,
        Fall,
        Dash,
        Kick,
        Dead
    }
    [Header("References")]
    public Rigidbody2D playerBody;
    public Animator animator;

    [Header("Movement")]
    public float speed = 3f;
    public float jumpForce = 15f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.15f;
    public LayerMask groundLayer;


    // =======If we have time
    [Header("Dash")]
    public float dashSpeed = 1;
    public float dashDuration = 1;
    public float dashCooldown = 1;

    [Header("Kick")]
    public float kickDuration = 1;
    public float kickCooldown = 1;
    // ========
    
    [Header("Audio")]
    public AudioClip walkingClip;
    public AudioClip jumpClip;
    //public AudioClip dashClip;
    //public AudioClip kickClip;

    [Range(0f, 1f)] public float walkingVolume = 0.1f;
    [Range(0f, 1f)] public float jumpVolume = 0.7f;
    //[Range(0f, 1f)] public float actionVolume = 0.7f;

    private PlayerState currentState = PlayerState.Idle;
    private PlayerState previousState = PlayerState.Idle;

    private int moveDirection = 0;
    private int facingDirection = 1;

    private bool isGrounded;

    // not using them yet
    private float dashTimer = 0f;
    private float dashCooldownTimer = 0f;
    private float kickTimer = 0f;
    private float kickCooldownTimer = 0f;

    private bool wasWalking = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ReadInput();
        CheckGround();
        UpdateState();
        UpdateAnimator();

        if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
    void FixedUpdate()
    {
        ApplyMovement();
    }

    void ReadInput()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        moveDirection = 0;

        if (keyboard.aKey.isPressed)
        {
            moveDirection -= 1;
        }

        if (keyboard.dKey.isPressed)
        {
            moveDirection += 1;
        }

        if (keyboard.wKey.wasPressedThisFrame && isGrounded)
        {
            Jump();
        }
    }

    void CheckGround()
    {
        if (groundCheck == null)
        {
            isGrounded = false;
            return;
        }

        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );
    }

    void UpdateState()
    {
        previousState = currentState;

        if (currentState == PlayerState.Dead)
        {
            return;
        }

        if (!isGrounded && playerBody.linearVelocity.y > 0.05f)
        {
            currentState = PlayerState.Jump;
        }
        else if (!isGrounded && playerBody.linearVelocity.y < -0.05f)
        {
            currentState = PlayerState.Fall;
        }
        else if (isGrounded && moveDirection != 0)
        {
            currentState = PlayerState.Run;
        }
        else if (isGrounded && moveDirection == 0)
        {
            currentState = PlayerState.Idle;
        }

        if (currentState != previousState)
        {
            OnStateChanged(previousState, currentState);
        }
    }

    void ApplyMovement()
    {
        if (currentState == PlayerState.Dead)
        {
            playerBody.linearVelocity = Vector2.zero;
            return;
        }

        playerBody.linearVelocity = new Vector2(
            speed * moveDirection,
            playerBody.linearVelocity.y
        );
    }

    void Jump()
    {
        playerBody.linearVelocity = new Vector2(
            playerBody.linearVelocity.x,
            jumpForce
        );

        currentState = PlayerState.Jump;

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySFX(jumpClip, jumpVolume);
        }
    }

    void OnStateChanged(PlayerState oldState, PlayerState newState)
    {
        if (oldState == PlayerState.Run && newState != PlayerState.Run)
        {
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.StopLoop();
            }
        }

        if (newState == PlayerState.Run)
        {
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.StartLoop(walkingClip, walkingVolume);
            }
        }

        Debug.Log("Player State: " + oldState + " -->> " + newState);
    }

    void UpdateAnimator()
    {
        if (animator == null) return;

        animator.SetInteger("State", (int)currentState);
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetFloat("YVelocity", playerBody.linearVelocity.y);
        animator.SetInteger("MoveDirection", moveDirection);
    }

    public void Die()
    {
        previousState = currentState;
        currentState = PlayerState.Dead;

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.StopLoop();
        }

        OnStateChanged(previousState, currentState);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        Gizmos.DrawWireSphere(
            groundCheck.position,
            groundCheckRadius
        );
    }
}

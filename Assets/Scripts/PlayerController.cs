using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;



public class PlayerController : MonoBehaviour
{
    public GameManager gameManager;
    public enum PlayerState
    {
        Idle,
        Run,
        Jump,
        Fall,
        Dash,
        Slide,
        Crouch,
        Dead
    }
    [Header("References")]
    public Rigidbody2D playerBody;
    public Animator animator;

    [Header("Movement")]
    public float speed = 5f;
    public float jumpForce = 5f;

    [Header("Ground Check")]
    public LayerMask groundLayer;

    // =======If we have time
    [Header("Dash")]
    public float dashSpeed = 15;
    public float dashDuration = 0.18f;
    public float dashCooldown = 0.5f;
    public float dashTimer = 0f;
    private float dashCooldownTimer = 0f;

    [Header("Audio")]
    public AudioClip[] footstepClips;
    public AudioClip jumpStartClip;
    public AudioClip landClip;
    public AudioClip dashClip;

    [Range(0f, 1f)] public float walkingVolume = 0.5f;
    [Range(0f, 1f)] public float jumpVolume = 0.7f;
    [Range(0f, 2f)] public float landVolume = 1.0f;
    [Range(0f, 1f)] public float dashVolume = 0.8f;

    private PlayerState currentState = PlayerState.Idle;
    private PlayerState previousState = PlayerState.Idle;

    private int moveDirection = 0;
    //private int facingDirection = 1;
    private int dashDirection = 0;

    private bool isGrounded;
    private bool isCrouched;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ReadInput();
        UpdateState();
        UpdateAnimator();
        HandleFootstepAudio();

        if (playerBody.linearVelocity.x < 0)
        {
            this.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
        }else if (playerBody.linearVelocity.x > 0)
        {
            this.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        }

        if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (dashCooldownTimer > 0f)
        {
            dashCooldownTimer -= Time.deltaTime;
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

        if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
        {
            moveDirection = -1;
        }

        if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
        {
            moveDirection = 1;
        }
        
        if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed)
        {
            isCrouched = true;
        }
        else
        {
            isCrouched = false;
        }

        if ((keyboard.wKey.wasPressedThisFrame || keyboard.upArrowKey.wasPressedThisFrame) && isGrounded)
        {
            Jump();
        }

        if (keyboard.spaceKey.wasPressedThisFrame && CanDash())
        {
            Dash();
        }

    }
    bool CanDash()
    {
        return currentState != PlayerState.Dead
            && currentState != PlayerState.Dash
            && dashCooldownTimer <= 0f
            && moveDirection != 0;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Goal"))
        {
            gameManager.NextLevel();
        }
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        if (playerBody.linearVelocity.y > 0.05f)
        {
            return;
        }

        if (IsGroundCollision(collision))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (IsInLayerMask(collision.gameObject.layer, groundLayer))
        {
            isGrounded = false;
        }
    }

    bool IsGroundCollision(Collision2D collision)
    {
        if (!IsInLayerMask(collision.gameObject.layer, groundLayer))
        {
            return false;
        }

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                return true;
            }
        }

        return false;
    }

    bool IsInLayerMask(int layer, LayerMask layerMask)
    {
        return (layerMask.value & (1 << layer)) != 0;
    }

    void UpdateState()
    {
        previousState = currentState;
        if (currentState == PlayerState.Dash)
        {
            dashTimer -= Time.deltaTime;

            if (dashTimer <= 0f)
            {
                if (!isGrounded)
                {
                    currentState = playerBody.linearVelocity.y > 0f
                        ? PlayerState.Jump
                        : PlayerState.Fall;
                }
                else if (moveDirection != 0)
                {
                    currentState = PlayerState.Run;
                }
                else
                {
                    currentState = PlayerState.Idle;
                }
            }

            if (currentState != previousState)
            {
                OnStateChanged(previousState, currentState);
            }

            return;
        }

        if (currentState == PlayerState.Dead)
        {
            return;
        }


        else if (isGrounded && isCrouched && Mathf.Abs(playerBody.linearVelocity.x) > 0)
        {
            currentState = PlayerState.Slide;
        }
        else if (isGrounded && isCrouched)
        {
            currentState = PlayerState.Crouch;
        }
        else if (isGrounded && moveDirection != 0)
        {
            currentState = PlayerState.Run;
        }
        else if (isGrounded && moveDirection == 0)
        {
            currentState = PlayerState.Idle;
        }
        else if (currentState != PlayerState.Dash)
        {
            if (!isGrounded && playerBody.linearVelocity.y > 0)
            {
                currentState = PlayerState.Jump;
            }
            else if (!isGrounded && playerBody.linearVelocity.y < 0)
            {
                currentState = PlayerState.Fall;
            }
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

        if (currentState == PlayerState.Dash)
        {
            playerBody.linearVelocity = new Vector2(
                dashDirection * dashSpeed,
                playerBody.linearVelocity.y
            );
            return;
        }

        if (currentState == PlayerState.Slide)
        {
            return;
        }

        playerBody.linearVelocity = new Vector2(
            speed * moveDirection,
            playerBody.linearVelocity.y
        );
    }

    void Jump()
    {
        isGrounded = false;

        playerBody.linearVelocity = new Vector2(
            playerBody.linearVelocity.x,
            jumpForce
        );

        currentState = PlayerState.Jump;

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySFX(jumpStartClip, jumpVolume);
        }
    }

    void Dash()
    {
        dashDirection = moveDirection;

        currentState = PlayerState.Dash;
        dashTimer = dashDuration;
        dashCooldownTimer = dashCooldown;

        playerBody.linearVelocity = new Vector2(
            dashDirection * dashSpeed,
            playerBody.linearVelocity.y
        );

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySFX(dashClip, dashVolume);
        }
    }

    void HandleFootstepAudio()
    {
        bool shouldPlayFootstep =
            isGrounded &&
            moveDirection != 0 &&
            currentState == PlayerState.Run;

        if (!shouldPlayFootstep)
        {
            if (SoundManager.Instance != null)
            {
                return;
            }

            return;
        }

        if (SoundManager.Instance == null) return;

        if (!SoundManager.Instance.IsFootstepPlaying())
        {
            PlayRandomFootstep();
        }
    }

    void PlayRandomFootstep()
    {
        if (footstepClips == null || footstepClips.Length == 0) return;
        if (SoundManager.Instance == null) return;

        int index = Random.Range(0, footstepClips.Length);
        AudioClip clip = footstepClips[index];

        SoundManager.Instance.PlayFootstep(clip, walkingVolume);
    }

    void OnStateChanged(PlayerState oldState, PlayerState newState)
    {

        bool wasInAir = 
            oldState == PlayerState.Jump ||
            oldState == PlayerState.Fall;

        bool landed =
            newState == PlayerState.Idle ||
            newState == PlayerState.Run ||
            newState == PlayerState.Crouch ||
            newState == PlayerState.Slide;

        if (wasInAir && landed)
        {
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlaySFX(landClip, landVolume);
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
            SoundManager.Instance.StopFootstep();
        }

        OnStateChanged(previousState, currentState);
    }
}

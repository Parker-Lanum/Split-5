using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;



public class PlayerController : MonoBehaviour
{
    public Rigidbody2D playerBody;

    [Header("Movement")]
    public int speed = 3;
    public int jumpHeight = 15;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.15f;
    public LayerMask groundLayer;

    [Header("Audio")]
    public AudioClip walkingClip;
    [Range(0f, 1f)] public float walkingVolume = 0.1f;

    private bool wasWalking = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        int moveDirection = 0;

        if (keyboard.aKey.isPressed)
        {
            moveDirection -= 1;
        }

        if (keyboard.dKey.isPressed)
        {
            moveDirection += 1;
        }

        bool isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        if (keyboard.wKey.wasPressedThisFrame && isGrounded)
        {
            playerBody.linearVelocity = new Vector2(
                playerBody.linearVelocity.x,
                jumpHeight
            );
        }

        playerBody.linearVelocity = new Vector2(
            speed * moveDirection,
            playerBody.linearVelocity.y
        );

        HandleFootstepAudio(moveDirection, isGrounded);

        if (keyboard.rKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene("SampleScene");
        }
    }

    void HandleFootstepAudio(int moveDirection, bool isGrounded)
    {
        bool isWalking = moveDirection != 0 && isGrounded;

        if (isWalking && !wasWalking)
        {
            SoundManager.Instance.StartLoop(walkingClip, walkingVolume);
        }
        else if (!isWalking && wasWalking)
        {
            SoundManager.Instance.StopLoop();
        }

        wasWalking = isWalking;
    }
}

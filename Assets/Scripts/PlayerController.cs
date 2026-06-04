using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;



public class PlayerController : MonoBehaviour
{
    public Rigidbody2D playerBody;
    public int speed = 3;
    public int jumpHeight = 15;
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
        int moveDirection = 0;
        int jump = 0;
        if (keyboard.aKey.isPressed)
        {
            moveDirection -= 1; 
        }
        if (keyboard.dKey.isPressed)
        {
            moveDirection += 1; 
        }
        if (keyboard.wKey.isPressed && playerBody.linearVelocity.y == 0)
        {
            jump = 1;
        }
        playerBody.linearVelocity = new Vector2(speed*moveDirection, (jump*jumpHeight)+playerBody.linearVelocity.y);
        if (keyboard.rKey.isPressed)
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;



public class PlayerController : MonoBehaviour
{
    public Rigidbody2D playerBody;
    public int speed = 5;
    public int jump = 5;
    public Scene scene;
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
        if (keyboard.aKey.isPressed)
        {
            moveDirection -= 1; 
        }
        if (keyboard.dKey.isPressed)
        {
            moveDirection += 1; 
        }

        playerBody.linearVelocity = new Vector2(speed*moveDirection, playerBody.linearVelocity.y);

        if (keyboard.wKey.isPressed && playerBody.linearVelocity.y == 0)
        {
            playerBody.AddForce(new Vector2(0,10));
        }
        if (keyboard.rKey.isPressed)
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}

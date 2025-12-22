using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    
    public GameObject cam;
    public Rigidbody rb;
    public float speed = 10f;
    public float maxspeed = 10f;
    private Vector2 moveInput = Vector2.zero;

    void Start()
    {
        
    }

    public void OnMove(InputAction.CallbackContext c)
    {
        moveInput = c.ReadValue<Vector2>();
        //rb.AddForce(moveInput * speed, ForceMode.Impulse);
        Debug.Log(moveInput);
        Debug.Log(cam.transform.rotation.y);
    }
    public void OnJump(InputAction.CallbackContext c)
    {
        if (c.performed)
        {
            
        }
    }
    void FixedUpdate()
    {   
        Vector3 move = new Vector3(moveInput.y, 0f, -moveInput.x);
        rb.AddForce(move * speed * (maxspeed - rb.linearVelocity.magnitude) / maxspeed, ForceMode.VelocityChange);
    }
}

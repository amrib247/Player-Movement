using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    
    public GameObject cam;
    public Rigidbody rb;
    public float speed = 50f;
    public float airspeed = 10f;
    public float crouchspeed = 10f;
    public float crouchcounter = 15f;
    public float maxspeed = 7f;
    public float jump_power = 5f;
    public float counterStrength = 15f;
    public float groundCheckDistance = 2f;
    public float groundCheckRadius = 0.3f;
    public LayerMask groundMask;
    private Vector2 moveInput = Vector2.zero;
    private bool grounded = true;
    private bool crouching = false;

    void Start()
    {
        
    }

    public void OnMove(InputAction.CallbackContext c)
    {
        moveInput = c.ReadValue<Vector2>();
        Debug.Log(moveInput);
        Debug.Log(cam.transform.rotation.y);
    }
    public void OnJump(InputAction.CallbackContext c)
    {
        if (c.performed && grounded)
        {
            rb.AddForce(Vector3.up * jump_power, ForceMode.Impulse);
        }
    }

    public void OnCrouch(InputAction.CallbackContext c)
    {
        if (c.performed)
        {
            crouching = true;
            transform.localScale = new Vector3(transform.localScale.x, 0.5f, transform.localScale.z);
        }
        else
        {
            crouching = false;
            transform.localScale = new Vector3(transform.localScale.x, 1.0f, transform.localScale.z);
        }
    }

    void FixedUpdate()
    {   
        grounded = Physics.SphereCast(
            transform.position + Vector3.up * 0.1f,
            groundCheckRadius,
            Vector3.down,
            out RaycastHit hit,
            groundCheckDistance,
            groundMask,
            QueryTriggerInteraction.Ignore
        );
        Vector3 move = new Vector3(moveInput.y, 0f, -moveInput.x);
        float angle = Vector3.SignedAngle(new Vector3(1f, 0f, 0f), new Vector3(cam.transform.forward.x, 0, cam.transform.forward.z), Vector3.up);
        Quaternion rotation = Quaternion.Euler(0, angle, 0);
        float velocity = (new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z)).magnitude;

        //Player Movement
        if (grounded && !crouching) {
            rb.AddForce(rotation * move * speed, ForceMode.Acceleration); //Move
            //Counter Movement
            rb.AddForce(Quaternion.AngleAxis(180f, Vector3.up) * (rotation * move) * counterStrength * velocity / maxspeed, ForceMode.Acceleration);
        }
        else if(!grounded)
        {
            rb.AddForce(rotation * move * airspeed, ForceMode.Acceleration);
        }
        else if (crouching)
        {
            rb.AddForce(rotation * move * crouchspeed, ForceMode.Acceleration);
            //Counter Movement
            rb.AddForce(Quaternion.AngleAxis(180f, Vector3.up) * (rotation * move) * crouchcounter * velocity / maxspeed, ForceMode.Acceleration);
        }
    }
}

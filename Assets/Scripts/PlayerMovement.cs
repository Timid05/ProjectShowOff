using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;

    private float backUpMoveSpeed;
    [SerializeField] float moveSpeed;
    [SerializeField] float groundDrag;
    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    [SerializeField] float playerHeight;
    [SerializeField] LayerMask ThisIsGround;
    bool isGrounded;

    [SerializeField] AudioSource footstepsSound;

    void Start()
    {
        backUpMoveSpeed = moveSpeed;
        rb = GetComponent<Rigidbody>();

        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, ThisIsGround);

        if (isGrounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }

        if (footstepsSound != null)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                footstepsSound.enabled = true;
            }
            else
            {
                footstepsSound.enabled = false;
            }
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    }

    public void SetEnabledMove(bool enable)
    {
        if (enable) moveSpeed = backUpMoveSpeed;        
        else moveSpeed = 0;        
    }
}

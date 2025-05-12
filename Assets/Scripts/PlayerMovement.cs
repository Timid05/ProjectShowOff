using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;

    private float backUpMoveSpeed;
    [SerializeField] float moveSpeed = 3.5f;
    [SerializeField] float sprintSpeed = 4.75f;
    bool playerSprinting = false;

    [SerializeField] float maxStamina = 100f;
    float currentStamina;
    [SerializeField] int staminaRegenBufferTime = 5;
    float sprintStopTime;
    [SerializeField] float staminaRegenSpeed = 0.33f;

    [SerializeField] float groundDrag;
    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    [SerializeField] float playerHeight;
    [SerializeField] LayerMask ThisIsGround;
    bool isGrounded;

    [SerializeField] AudioSource footstepsSound;

    public static event Action<bool> OnMoveStatusChange;

    void Start()
    {
        backUpMoveSpeed = moveSpeed;
        currentStamina = maxStamina;
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
        if(Input.GetKey(KeyCode.LeftShift) && currentStamina > 0) { rb.AddForce(moveDirection.normalized * sprintSpeed * 10f, ForceMode.Force);
            currentStamina--;
            sprintStopTime = Time.time;
            //Debug.Log("Stamina: " + currentStamina);

            // Update the status of the player's sprint if doesn't match and send out a delegate to make the view bobbing amplitude match.
            if(playerSprinting != false && OnMoveStatusChange != null)
            {
                //Debug.Log("Player started sprinting.");
                playerSprinting = false;
                OnMoveStatusChange(playerSprinting);
            }
        }
        else 
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
            // Stamina starts regenning after a specific amount of seconds if the player isn't trying to sprint and the stamina isn't max.
            if (Time.time - sprintStopTime >= staminaRegenBufferTime && currentStamina < maxStamina)
            {
                currentStamina += staminaRegenSpeed;
                //Debug.Log("Stamina: " + currentStamina);
            }

            if (playerSprinting != true && OnMoveStatusChange != null)
            {
                //Debug.Log("Player stopped sprinting.");
                playerSprinting = true;
                OnMoveStatusChange(playerSprinting);
            }
        }
    }

    public void SetEnabledMove(bool enable)
    {
        if (enable) moveSpeed = backUpMoveSpeed;        
        else moveSpeed = 0;        
    }
}

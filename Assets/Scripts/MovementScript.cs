using System.Collections;
using UnityEngine;
public class MovementScript : MonoBehaviour
{
    private float moveSpeed = 5f;
    public float walkSpeed;
    public float sprintSpeed;
    public float attackDuration = 0.66f;
    public float blockSpeed;

    public KeyCode attackKey = KeyCode.Mouse0;
    public KeyCode blockKey = KeyCode.F;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode rollKey = KeyCode.Q;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;
    Vector3 inputDir;

    public bool actionable;
    public bool rollCoolDown;
    public float rollTimer = 0;
    public float rollTime = 0.6f;

    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;
    public float groundDrag;

    Rigidbody rb;

    public MovementState state;
    public OptionState optionState = OptionState.idle;
    public enum MovementState
    {
        walking,
        sprinting,
        air
    }

    public enum OptionState
    {
        attack,
        block,
        roll,
        idle
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        actionable = true;
    }
    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();

        if (grounded)
        {
            rb.linearDamping = groundDrag;
        }
        else
        {
            rb.linearDamping = 0;
        }

        SpeedControl();

        if (actionable)
        {
            StateHandler();
        }
        if (!actionable && optionState == OptionState.roll)
        {
            rollTimer -= Time.deltaTime;
            if (rollTimer <= 0)
            {
                EndRoll();
            }
        }
    }
    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void StateHandler()
    {
        
        if (grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }
        else if (grounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }
        else
        {
            state = MovementState.air;
        }

        if (Input.GetKeyDown(attackKey))
        {
            AttackMethod();
        }
        else if (Input.GetKey(blockKey))
        {
            optionState = OptionState.block;
            moveSpeed = blockSpeed;
        }
        else if (Input.GetKeyDown(rollKey))
        {
            RollMethodStart();
        }
        else
        {
            optionState = OptionState.idle;
        }
        
    }

    public void AttackMethod()
    {
        optionState = OptionState.attack;
        moveSpeed = walkSpeed;
        StartCoroutine(DurationTimer(1));
    }

    public void RollMethodStart()
    {
        optionState = OptionState.roll;
        actionable = false;

        inputDir = new Vector3(horizontalInput, 0, verticalInput).normalized;

        if (inputDir == Vector3.zero)
        {
            inputDir = transform.forward;
        }

        rollTimer = rollTime;

        rb.linearVelocity = Vector3.zero;
        rb.AddForce(inputDir * 35, ForceMode.VelocityChange);
    }
    
    public void EndRoll()
    {
        actionable = true;
        optionState = OptionState.idle;
        rollTimer = 0;
    }

    private IEnumerator DurationTimer(int call)
    {
        actionable = false;
        float duration = 0;

        if (call == 1)
        {
            duration = 0.66f;
        }
        if (call == 2)
        {
            duration = 0.33f;
        }

        yield return new WaitForSeconds(duration);
        actionable = true;
    }
    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }
}

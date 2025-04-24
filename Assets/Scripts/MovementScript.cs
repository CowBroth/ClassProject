using System.Collections;
using UnityEngine;
public class MovementScript : MonoBehaviour
{
    private float moveSpeed = 5f;
    public float walkSpeed;
    public float sprintSpeed;
    public float attackDuration = 0.66f;
    public float blockSpeed;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

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

    public Animator anim;
    CharacterController cc;
    public Transform cam;
    public CombatScript combatScript;
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
        cc = GetComponent<CharacterController>();
        combatScript = GetComponent<CombatScript>();
        actionable = true;
    }
    private void Update()
    {
        Vector3 direction = new Vector3(horizontalInput, 0f, verticalInput).normalized;
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        MovePlayer();

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
    public void MyInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
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
    public void MovePlayer()
    {
        Vector3 direction = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            cc.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
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
        combatScript.AttackHitbox();
        actionable = true;
    }

}
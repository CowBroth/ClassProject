using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
public class MovementScript : MonoBehaviour
{
    private float moveSpeed = 5f;
    public float walkSpeed;
    public float sprintSpeed;
    public float attackDuration = 0.66f;
    public float blockSpeed;
    public float turnSmoothTime = 1f;
    float turnSmoothVelocity;

    public KeyCode attackKey = KeyCode.Mouse0;
    public KeyCode blockKey = KeyCode.F;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode rollKey = KeyCode.Q;

    public Transform orientation;
    public int moveEnumInt;
    public int optionEnumInt;
    public bool stillAnim;

    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;
    Vector3 inputDir;

    public bool actionable;

    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;
    public float gravityValue;
    public float groundDrag;
    public Vector3 direction;
    Vector3 velocity;

    public Animator anim;
    CharacterController cc;
    public Transform cam;
    public MovementState state;
    public OptionState optionState = OptionState.idle;
    public bool blocking;
    public bool parrying;
    public bool stunned;
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
        stunned,
        idle
    }

    private void Start()
    {
        cc = GetComponent<CharacterController>();
        actionable = true;
    }
    private void Update()
    {
        grounded = cc.isGrounded;
        if (optionState == OptionState.block)
        {
            blocking = true;
        }
        else
        {
            blocking = false;
        }

            MyInput();
        MovePlayer();
        if (actionable && !stunned)
        {
            StateHandler();
        }
       
        moveEnumInt = Convert.ToInt32(state);
        optionEnumInt = Convert.ToInt32(optionState);

        anim.SetInteger("Movement", moveEnumInt);
        anim.SetInteger("Options", optionEnumInt);
        anim.SetBool("Still", stillAnim);
        anim.SetBool("Stunned", stunned);
    }
    public void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        direction = new Vector3(horizontalInput, 0f, verticalInput).normalized;
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
        else
        {
            optionState = OptionState.idle;
        }
    }
    public void MovePlayer()
    {
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            stillAnim = false;

            cc.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
        }
        else
        {
            stillAnim = true;
        }
        if (grounded && velocity.y < 0.0f)
        {
            velocity.y = -0.05f;
        }
        else
        {
            velocity.y += gravityValue * Time.deltaTime;
        }
        cc.Move(velocity);
    }

    public void AttackMethod()
    {
        optionState = OptionState.attack;
        moveSpeed = walkSpeed;
        StartCoroutine(DurationTimer(1));
    }

    public void PlayerHit()
    {
        StartCoroutine(StunTimer());
    }

    private IEnumerator DurationTimer(int call)
    {
        actionable = false;
        float duration = 0;

        if (call == 1)
        {
            duration = 1.1f;
        }
        if (call == 2)
        {
            duration = 0.75f;
        }

        yield return new WaitForSeconds(duration);
        actionable = true;
    }
    private IEnumerator StunTimer()
    {
        stunned = true;
        moveSpeed = 4f;
        float d = 0.33f;
        yield return new WaitForSeconds(d);
        stunned = false;
    }
}
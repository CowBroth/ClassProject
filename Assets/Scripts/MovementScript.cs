using System;
using System.Collections;
using TMPro;
using Unity.Cinemachine;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MovementScript : MonoBehaviour
{
    public float hitPoints = 100;
    private float moveSpeed = 5f;
    public float walkSpeed;
    public float sprintSpeed;
    public float attackDuration = 0.66f;
    public float blockSpeed;
    public float turnSmoothTime = 1f;
    float turnSmoothVelocity;
    public AudioPlayer aud;
    public TMP_Text hpText;

    public KeyCode attackKey = KeyCode.Mouse0;
    public KeyCode blockKey = KeyCode.F;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode rollKey = KeyCode.Q;
    public KeyCode lockKey = KeyCode.Tab;

    public Transform orientation;
    public int moveEnumInt;
    public int optionEnumInt;
    public bool stillAnim;

    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;
    Vector3 inputDir;

    public bool actionable;
    public bool grounded;
    public float playerHeight;
    public LayerMask whatIsGround;
    public float gravityValue;
    public float groundDrag;
    public Vector3 direction;
    Vector3 velocity;

    public GameObject corpse;
    public Animator anim;
    CharacterController cc;
    public GameObject cam;
    public MovementState state;
    public OptionState optionState = OptionState.idle;
    public bool blocking;
    public bool parrying;
    public bool stunned;
    bool isMoving;
    bool isLocked;
    public CinemachineOrbitalFollow orbit;
    private Vector3 axs;
    RideGruv rideScript;
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
        rideScript = GetComponent<RideGruv>();
        actionable = true;
        aud = GetComponent<AudioPlayer>();
    }
    public void Update()
    {
        hpText.text = Convert.ToString(hitPoints);
        Gravity();
        axs = new Vector3(0, orbit.HorizontalAxis.Value, 0);
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
        if (actionable && !stunned && !rideScript.isRiding)
        {
            StateHandler();
        }
        moveEnumInt = Convert.ToInt32(state);
        optionEnumInt = Convert.ToInt32(optionState);
        anim.SetInteger("Movement", moveEnumInt);
        anim.SetInteger("Options", optionEnumInt);
        anim.SetBool("Still", stillAnim);
        anim.SetBool("Stunned", stunned);
        anim.SetBool("Riding", rideScript.isRiding);
        if (Input.GetKeyDown(lockKey))
        {
            LockMethod();
        }
    }

    public void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        direction = new Vector3(horizontalInput, 0f, verticalInput).normalized;
    }

    private void StateHandler()
    {
        
        if (Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }
        else
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
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
        if (isLocked)
        {
            transform.rotation = Quaternion.Euler(axs);
        }
        if (direction.magnitude >= 0.1f && !rideScript.isRiding)
        {
            isMoving = true;
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            if (!isLocked)
            {
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            stillAnim = false;
            print(moveDir);
            cc.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
        }
        else
        {
            isMoving = false;
            stillAnim = true;
        }
        if (rideScript.isRiding)
        {
            float targetAngle = cam.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            if (!isLocked)
            {
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }

            Vector3 moveDir = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;

            cc.Move(moveDir.normalized * 25 * Time.deltaTime);
        }
    }
    void Gravity()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, 0.25f, whatIsGround);
        Debug.DrawRay(transform.position, Vector3.down * 0.05f, Color.green);

        if ((grounded || velocity.y == 0) && !rideScript.isRiding)
        {
            velocity.y = -0.1f;
        }
        if (grounded && rideScript.isRiding)
        {
            velocity.y = -10f;
        }
        
        velocity.y += gravityValue * Time.deltaTime;
        cc.Move(new Vector3(0, velocity.y * Time.deltaTime, 0));
    }
    public void AttackMethod()
    {
        optionState = OptionState.attack;
        moveSpeed = walkSpeed;
        StartCoroutine(DurationTimer(1));
    }

    public void PlayerHit()
    {
        aud.HitSound();
        hitPoints -= 25;
        
        print(hitPoints);
        if (hitPoints <= 0)
        {
            hpText.text = Convert.ToString(hitPoints);
            aud.DeathSound();
            print("DEATH !!");
            corpse.gameObject.SetActive(true);
            corpse.transform.position = gameObject.transform.position;
            corpse.GetComponent<Rigidbody>().AddExplosionForce(1500, new Vector3(corpse.transform.position.x, corpse.transform.position.y - 5f, corpse.transform.position.z + 5f), 500);
            cam.GetComponent<CinemachineCamera>().Follow = corpse.transform;
            ManagerScript.instance.DieMethod();
            Destroy(gameObject);
        }
        StartCoroutine(StunTimer());
        StartCoroutine(PassiveRegen());
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
    public IEnumerator PassiveRegen()
    {
        yield return new WaitForSeconds(10);
        hitPoints += 25f;
    }
    private IEnumerator StunTimer()
    {
        stunned = true;
        moveSpeed = 4f;
        float d = 0.33f;
        yield return new WaitForSeconds(d);
        stunned = false;
    }
    void LockMethod()
    {
        if (!isLocked)
        {
            Cursor.lockState = CursorLockMode.Locked; isLocked = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined; isLocked = false;
        }
    }
    public void OnParry()
    {
        aud.ParrySound();
        anim.SetTrigger("Parry");
    }
    public void OnBlock()
    {
        aud.BlockSound();
    }
}
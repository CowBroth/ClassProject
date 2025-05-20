using System;
using System.Collections;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [Header("Values")]
    public float hitPoints;
    public float speed;
    [SerializeField] private float effectiveRange;

    public bool actionable = true;
    public bool stunned = false;
    public Transform player;
    Animator anim;
    Rigidbody rb;
    public BehaviourState state;
    SphereCollider detectionRadius;
    public LayerMask playerLayer;
    public bool attackRange;
    CombatScript combatScript;
    [SerializeField] Vector3 lookAt;
    bool playerBlocking;
    public bool parried;
    public float currentSpeed;
    void Start()
    {
        detectionRadius = GetComponentInChildren<SphereCollider>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        combatScript = GetComponentInChildren<CombatScript>();
    }
    public enum BehaviourState
    {
        idle,
        aggro,
        attacking,
        stunned
    }
    private void FixedUpdate()
    {
        if (!stunned)
        {
            EnemyState();
        }
        else
        {
            state = BehaviourState.stunned;
        }
        anim.SetBool("Stunned", stunned);
        anim.SetBool("Parried", parried);
    }
    private void OnTriggerStay(Collider other)
    {
        if (!stunned)
        {
            state = BehaviourState.aggro;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!stunned)
        {
            state = BehaviourState.idle;
        }
    }
    void EnemyState()
    {
        Vector3 playerDirection = (new Vector3(player.position.x, transform.position.y, player.position.z) - transform.position).normalized;
        lookAt = new Vector3(player.position.x, transform.position.y, player.position.z);
        if (state == BehaviourState.idle)
        {
            anim.SetBool("Still", true);
        }
        else if (state == BehaviourState.aggro && !attackRange)
        {
            rb.AddForce(playerDirection * speed);
            transform.LookAt(lookAt);
            
            anim.SetBool("Still", false);
        }
        else if (state == BehaviourState.aggro && attackRange)
        {
            //currentSpeed = speed / Vector3.Distance(transform.position, player.position);
            if (Vector3.Distance(transform.position, player.position) >= effectiveRange)
            {
                rb.AddForce(playerDirection * speed);
            }
            if (Vector3.Distance(transform.position, player.position) <= effectiveRange)
            {
                rb.AddForce(new Vector3(playerDirection.x + 3.25f, playerDirection.z + 3.25f).normalized);
            }
            transform.LookAt(lookAt);
            if (actionable)
            {
                state = BehaviourState.attacking;
                anim.SetTrigger("Attack");
                StartCoroutine(DurationTimer());
            }
        }
    }
    public void OnStun(int i)
    {
        if (i == 0)
        {
            hitPoints -= 25;
            actionable = true;
            if (hitPoints <= 0)
            {
                rb.freezeRotation = false;
                GetComponentInChildren<Animator>().enabled = false;
                rb.AddForce(new Vector3(0, 0, 75), ForceMode.Impulse);
                Destroy(this);
            }
            StartCoroutine(StunTimer(0));
        }
        if (i == 1)
        {
            parried = true;
            StartCoroutine(StunTimer(1));
        }
    }
    private IEnumerator DurationTimer()
    {
        actionable = false;
        float duration = 1.1f;
        yield return new WaitForSeconds(duration);
        actionable = true;
        state = BehaviourState.aggro;
    }
    private IEnumerator StunTimer(int i)
    {
        stunned = true;
        float duration = 0.66f;
        if(i == 0)
        {
            duration = 0.125f;
        }
        yield return new WaitForSeconds(duration);
        stunned = false;
        parried = false;
        actionable = true;
    }
}
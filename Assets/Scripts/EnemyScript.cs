using System;
using System.Collections;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public bool actionable = true;
    public bool stunned = false;
    public float speed;
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
        Vector3 playerDirection = (player.position - transform.position).normalized;
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
        speed = 35;
        actionable = false;
        float duration = 1.1f;
        yield return new WaitForSeconds(duration);
        actionable = true;
        state = BehaviourState.aggro;
        speed = 70;
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
    }
}
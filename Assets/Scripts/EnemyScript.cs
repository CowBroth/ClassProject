using System;
using System.Collections;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public bool actionable = true;
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
        playerBlocking = playerScript.blocking;
        EnemyState();
    }
    private void OnTriggerStay(Collider other)
    {
        state = BehaviourState.aggro;
        print(state);
    }
    private void OnTriggerExit(Collider other)
    {
        state = BehaviourState.idle;
        print(state);
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
    public void EnemyAttack()
    {

    }
    private IEnumerator DurationTimer()
    {
        actionable = false;
        float duration = 1.1f;
        yield return new WaitForSeconds(duration);
        actionable = true;
        state = BehaviourState.aggro;
    }
}

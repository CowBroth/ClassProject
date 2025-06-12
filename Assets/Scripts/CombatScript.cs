using System.Collections;
using UnityEngine;

public class CombatScript : MonoBehaviour
{
    public GameObject attackHitbox;
    public MovementScript movementScript;
    public AudioPlayer aud;

    void Start()
    {
        movementScript = GetComponentInParent<MovementScript>();
        aud = GetComponentInParent<AudioPlayer>();
    }
    public void AttackStart()
    {
        attackHitbox.SetActive(true);
    }
    public void AttackEnd()
    {
        attackHitbox.SetActive(false);
    }
    public void ParryMethod()
    {
        StartCoroutine(ParryTimer());
    }
    public IEnumerator ParryTimer()
    {
        float t = 0.15f;
        movementScript.parrying = true;
        yield return new WaitForSeconds(t);
        movementScript.parrying = false;
    }
    public void SoundEvent()
    {
        aud.SlashSound();
    }
}
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public AudioSource aud;
    public List<AudioClip> clip;
    void Start()
    {
        aud = GetComponent<AudioSource>();
    }
    public void HitSound()
    {
        aud.PlayOneShot(clip[3], 0.5f);
    }
    public void DeathSound()
    {
        aud.PlayOneShot(clip[5], 0.5f);
    }
    public void SlashSound()
    {
        aud.PlayOneShot(clip[2], 0.2f);
    }
    public void BlockSound()
    {
        aud.PlayOneShot(clip[0], 0.5f);
    }
    public void ParrySound()
    {
        aud.PlayOneShot(clip[1], 0.5f);
    }

}

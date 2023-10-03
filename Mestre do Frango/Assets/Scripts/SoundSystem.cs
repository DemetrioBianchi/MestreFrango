using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSystem : MonoBehaviour
{
    [SerializeField] AudioClip coinCoxinha, coinCx;
    [SerializeField] AudioSource emisorEffect;
    // Start is called before the first frame update

    public void PlayAudioCoxinha()
    {
        emisorEffect.clip = coinCoxinha;
        emisorEffect.Play();
    }

    public void PlayAudioCx()
    {
        emisorEffect.clip = coinCx;
        emisorEffect.Play();
    }
}

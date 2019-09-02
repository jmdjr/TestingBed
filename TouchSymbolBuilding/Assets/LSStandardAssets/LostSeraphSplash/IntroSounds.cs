using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroSounds : MonoBehaviour
{
    public AudioClip SoundEffect;
    public void Start()
    {
        MainAudio.PlaySoundEffect(SoundEffect, 1.0f);
    }
}

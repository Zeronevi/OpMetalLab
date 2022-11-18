using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSystem : MonoBehaviour
{
    private static AudioSystem audioSystem = null;
    public static AudioSystem GetInstance()
    {
        return audioSystem;
    }

    public AudioSource sfx_effects;
    public AudioClip shoot_Sound, noammo_Sound;

    void Start()
    {
        audioSystem = this;
    }

    public void Shoot(bool noAmmo, Vector3 position)
    {
        if (!noAmmo)
        {
            sfx_effects.clip = shoot_Sound;
            sfx_effects.Play();
            NoiseSystem.MakeNoise(position, 5f);
        }
        else
        {
            sfx_effects.clip = noammo_Sound;
            sfx_effects.Play();
        }

    }
}

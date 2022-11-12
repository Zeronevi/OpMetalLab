using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Singleton　(X﹏X)。
//Todo: Deixar de ser Singleton, <<controlar as coisas por eventos>>

public class NoiseSystem : MonoBehaviour
{ 
    public static List<Noise> Noises;
    public GameObject Player;
    
    public void Start()
    {
        Noises = new List<Noise>();
    }

    public void Update()
    {
        DebugInfo();
    }

    public static void MakeNoise(Vector2 position, float strenght, Noise.SoundType soundType)
    {
        Noise noise = new Noise(position, strenght, soundType);
        foreach (Enemy enemy in Enemy.enemyList)
        {
            enemy.Listen(noise);
        }
        Noises.Add(noise);
    }

    void DebugInfo()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1)){
            NoiseSystem.MakeNoise(SharedContent.MousePosition, 15f, Noise.SoundType.Gunshot);
        }
    }
    
    
    public void OnDrawGizmos()
    {
        //TODO: Desenhar os Gizmos
        if (Noises != null && Noises.Count != 0)
        {
            foreach (var noise in Noises)
            {
                Gizmos.DrawWireSphere(noise.Position, noise.Strength);
            }
        }
    }
}


public class Noise
{
    public Vector2 Position;
    public float Strength;

    public enum SoundType
    {
        //TODO tweak
        AmbientNoise = 1,Steps = 3,BulletHit = 5,SilencedGunshot = 8,Gunshot = 15
    }
    public SoundType soundType;
    
    public Noise(Vector2 pos, float strength, SoundType soundType)
    {
        Position = pos;
        Strength = strength;
        this.soundType = soundType;
    }
}

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

    public static void MakeNoise(Vector2 position, float strength, Noise.SoundType soundType)
    {
        Noise noise = new Noise(position, strength, soundType);
        foreach (Enemy enemy in Enemy.enemyList)
        {
            enemy.Listen(noise);
        }
        Noises.Add(noise);
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

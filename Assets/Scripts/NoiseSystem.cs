using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseSystem : MonoBehaviour
{
    public GameObject Player;

    public GameObject SoundRadius;
    public const int SoundCircleSteps = 10;

    public void Start()
    {
        
    }

    public static void MakeNoise(Vector2 position, float strength, Noise.SoundType soundType)
    {
        Noise noise = new Noise(position, strength, soundType);
        foreach (Enemy enemy in Enemy.enemyList)
        {
            enemy.Listen(noise);
        }
        
    }

    public void AddEnemyStep(Vector2 position, float radius)
    {
        GameObject soundRadius = Instantiate(SoundRadius);
        soundRadius.transform.position = position;
        LineRenderer circleRenderer = soundRadius.GetComponent<LineRenderer>();

        circleRenderer.positionCount = SoundCircleSteps;
        for (int i = 0; i < SoundCircleSteps; i++)
        {
            float circProgress = (float)i / SoundCircleSteps;
            float currRad = circProgress * 2 * Mathf.PI;

            Vector2 curr_pos = new Vector2(radius * Mathf.Cos(currRad), radius * Mathf.Sin(currRad)) + position;
            circleRenderer.SetPosition(i, curr_pos);
        }

        Destroy(soundRadius, 0.30f);
    }

}

public class Noise
{
    public Vector2 Position;
    public float Strength;

    public enum SoundType
    {
        //TODO tweak
        AmbientNoise = 1, Steps = 3, BulletHit = 5, SilencedGunshot = 8, Gunshot = 15
    }
    public SoundType soundType;

    public Noise(Vector2 pos, float strength, SoundType soundType)
    {
        Position = pos;
        Strength = strength;
        this.soundType = soundType;
    }
}